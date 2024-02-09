using BookstoreApp.Model;
using BookstoreApp.Common;
using BookstoreApp.Repository.Common;
using Npgsql;
using System.Diagnostics.Metrics;
using System.Net;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BookstoreApp.Repository
{
    public class BookstoreRepository : IBookstoreRepository
    {
        public string connectionString = CommonProperty.connectionString;
        List<Bookstore> readBookstores;
        // Get with optional parameters

        public List<Bookstore> Get()
        {
            readBookstores = new List<Bookstore>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string bookstoresQuery = "SELECT \"Bookstore\".\"Id\", \"Bookstore\".\"Name\", \"Bookstore\".\"Address\", \"Bookstore\".\"Owner\", \"Book\".\"Title\" FROM \"Bookstore\" LEFT JOIN \"BookstoreInventory\" ON \"Bookstore\".\"Id\" = \"BookstoreInventory\".\"BookstoreId\" LEFT JOIN \"Book\" ON \"Book\".\"Id\" = \"BookstoreInventory\".\"BookId\"";

                NpgsqlCommand bookstoreCommand = new NpgsqlCommand(bookstoresQuery, connection);

                connection.Open();
                NpgsqlDataReader reader = bookstoreCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ReadBookstore(reader);
                    }
                }
            }

            return readBookstores;
        }

        public Bookstore Get(Guid id)
        {
            Bookstore bookstore = null;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string bookstoreQuery = "SELECT * FROM \"Bookstore\" WHERE \"Id\" = @Id";
                NpgsqlCommand command = new NpgsqlCommand(bookstoreQuery, connection);
                command.Parameters.AddWithValue("id", id);

                connection.Open();

                NpgsqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                {
                    bookstore = new Bookstore
                    {
                        Id = id,
                        Name = (string)reader["Name"],
                        Address = (string)reader["Address"],
                        Owner = (string)reader["Owner"]
                    };
                }
            }

            return bookstore;
        }

        public bool Add(Bookstore bookstore)
        {
            Guid bookstoreGuid = Guid.NewGuid();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                // Inserting data into "Bookstore" table
                string bookstoreQuery = "INSERT INTO \"Bookstore\" VALUES (@Id, @Name, @Address, @Owner)";
                NpgsqlCommand bookstoreCommand = new NpgsqlCommand(bookstoreQuery, connection);

                bookstoreCommand.Parameters.AddWithValue("@Id", bookstoreGuid);
                bookstoreCommand.Parameters.AddWithValue("@Name", bookstore.Name);
                bookstoreCommand.Parameters.AddWithValue("@Address", bookstore.Address);
                bookstoreCommand.Parameters.AddWithValue("@Owner", bookstore.Owner);

                // Inserting data into "Book" and "BookstoreInventory" tables
                string bookQuery = "INSERT INTO \"Book\" VALUES (@Id, @Title, @Author)";
                string bookstoreInventoryQuery = "INSERT INTO \"BookstoreInventory\" VALUES (@Id, @BookId, @BookstoreId)";
                NpgsqlCommand bookCommand = new NpgsqlCommand(bookQuery, connection);
                NpgsqlCommand bookstoreInventoryCommand = new NpgsqlCommand(bookstoreInventoryQuery, connection);

                foreach (var book in bookstore.Books)
                {
                    Guid bookGuid = Guid.NewGuid();

                    bookCommand.Parameters.AddWithValue("@Id", bookGuid);
                    bookCommand.Parameters.AddWithValue("@Name", bookstore.Name);
                    bookCommand.Parameters.AddWithValue("@Address", bookstore.Address);
                    bookCommand.Parameters.AddWithValue("@Owner", bookstore.Owner);

                    bookstoreInventoryCommand.Parameters.AddWithValue("@Id", Guid.NewGuid());
                    bookstoreInventoryCommand.Parameters.AddWithValue("@BookstoreId", bookstoreGuid);
                    bookstoreInventoryCommand.Parameters.AddWithValue("@BookId", bookGuid);
                }

                connection.Open();
                NpgsqlTransaction transaction = connection.BeginTransaction();
                bookstoreCommand.Transaction = bookCommand.Transaction = bookstoreInventoryCommand.Transaction = transaction;

                int bookstoreTableValueChanged = bookstoreCommand.ExecuteNonQuery();
                int bookTableValueChanged = bookCommand.ExecuteNonQuery();
                int bookstoreInventoryValueChanged = bookstoreInventoryCommand.ExecuteNonQuery();

                if (bookstoreTableValueChanged != 0 && bookTableValueChanged != 0 && bookstoreInventoryValueChanged != 0)
                {
                    transaction.Commit();
                    return true;
                }
            }

            return false;
        }

        // NpgSqlTransaction
        public bool Update(Bookstore bookstore)
        {
            int rowChanged;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand bookstoreCommand = new NpgsqlCommand();
                List<string> updatedValues = new List<string>();

                bookstoreCommand.Connection = connection;
                bookstoreCommand.Parameters.AddWithValue("@id", bookstore.Id);

                if (bookstore.Address != null)
                {
                    updatedValues.Add("\"Address\" = @Address");
                    bookstoreCommand.Parameters.AddWithValue("@Address", bookstore.Address);
                }

                if (bookstore.Name != null)
                {
                    updatedValues.Add("\"Name\" = @Name");
                    bookstoreCommand.Parameters.AddWithValue("@Name", bookstore.Name);
                }

                if (bookstore.Owner != null)
                {
                    updatedValues.Add("\"Owner\" = @Owner");
                    bookstoreCommand.Parameters.AddWithValue("@Owner", bookstore.Owner);
                }

                if (updatedValues.Count == 0) return false;

                bookstoreCommand.CommandText = "UPDATE \"Bookstore\" SET " + string.Join(", ", updatedValues) + "WHERE \"Id\" = @Id";
                connection.Open();
                rowChanged = bookstoreCommand.ExecuteNonQuery();
            }

            return rowChanged != 0;
        }
        public bool Delete(Guid id)
        {
            bool successful = false;

            string commandText = "DELETE FROM \"Bookstore\" WHERE Id = @Id";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@Id", id);

                NpgsqlTransaction transaction = connection.BeginTransaction();

                command.Transaction = transaction;

                int rowChanged = command.ExecuteNonQuery();

                if (rowChanged != 0)
                {
                    transaction.Commit();
                    return true;
                }
            }

            return successful;
        }

        private void ReadBookstore(NpgsqlDataReader bookstoreRow)
        {
            // Title is the only field that can be null
            // Checking if the BookstoreId has been seen before
            string title = bookstoreRow["Title"] as string;

            Bookstore targetBookstore = readBookstores.FirstOrDefault(x => x.Id == (Guid)bookstoreRow["Id"]);
            if (targetBookstore == null)
            {
                readBookstores.Add(new Bookstore
                {
                    Id = (Guid)bookstoreRow["Id"],
                    Name = (string)bookstoreRow["Name"],
                    Address = (string)bookstoreRow["Address"],
                    Owner = (string)bookstoreRow["Owner"],
                    Books = new List<Book>
                    {
                        new Book {Title = title ?? "No Books"}
                    }
                });
            }
            else
            {
                targetBookstore.Books.Add(new Book
                {
                    Title = title ?? "No Books"
                });
            }
        }
    }
}
