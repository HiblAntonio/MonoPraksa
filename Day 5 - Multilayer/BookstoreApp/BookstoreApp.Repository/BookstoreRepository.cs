using BookstoreApp.Model;
using BookstoreApp.Common;
using BookstoreApp.Repository.Common;
using Npgsql;
using System.Diagnostics.Metrics;
using System.Net;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreApp.Repository
{
    public class BookstoreRepository : IBookstoreRepository
    {
        public string connectionString = CommonProperty.connectionString;
        List<Bookstore> readBookstores;

        public async Task<List<Bookstore>> GetAsync()
        {
            readBookstores = new List<Bookstore>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string bookstoresQuery = "SELECT \"Bookstore\".\"Id\", \"Bookstore\".\"Name\", \"Bookstore\".\"Address\", \"Bookstore\".\"Owner\", \"Book\".\"Title\" FROM \"Bookstore\" LEFT JOIN \"BookstoreInventory\" ON \"Bookstore\".\"Id\" = \"BookstoreInventory\".\"BookstoreId\" LEFT JOIN \"Book\" ON \"Book\".\"Id\" = \"BookstoreInventory\".\"BookId\"";

                NpgsqlCommand bookstoreCommand = new NpgsqlCommand(bookstoresQuery, connection);

                connection.Open();
                NpgsqlDataReader reader = await bookstoreCommand.ExecuteReaderAsync();

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

        public async Task<Bookstore> GetAsync(Guid id)
        {
            Bookstore bookstore = null;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string bookstoreQuery = "SELECT * FROM \"Bookstore\" WHERE \"Id\" = @Id";
                NpgsqlCommand command = new NpgsqlCommand(bookstoreQuery, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                NpgsqlDataReader reader = await command.ExecuteReaderAsync();

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

        public async Task<List<Bookstore>> GetAsync(string searchQuery, string itemSorting, bool isAsc, int pageNum, int pageSize)
        {
            readBookstores = new List<Bookstore>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand bookstoreCommand = new NpgsqlCommand();
                bookstoreCommand.Connection = connection;

                List<string> filteredValues = new List<string>();
                string orderByClause = " ORDER BY \"Name\" ASC";
                string paginationClause = "";

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    filteredValues.Add(" WHERE \"Name\" LIKE '%" + searchQuery + "%'");
                    filteredValues.Add(" OR \"Address\" LIKE '%" + searchQuery + "%'");
                    filteredValues.Add(" OR \"Owner\" LIKE '%" + searchQuery + "%'");
                }

                if (!string.IsNullOrWhiteSpace(itemSorting))
                {
                    orderByClause = isAsc ? " ORDER BY @itemSorting ASC" : " ORDER BY @itemSorting DESC";
                    bookstoreCommand.Parameters.AddWithValue("@itemSorting", itemSorting);
                }
                
                paginationClause = " OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

                string bookstoreQuery = "SELECT * FROM \"Bookstore\"";

                if (filteredValues.Any())
                    bookstoreQuery += string.Join("", filteredValues);
                
                bookstoreQuery += orderByClause + paginationClause;

                bookstoreCommand.CommandText = bookstoreQuery;
                bookstoreCommand.Parameters.AddWithValue("@offset", pageSize * (pageNum - 1));
                bookstoreCommand.Parameters.AddWithValue("@pageSize", pageSize);

                connection.Open();
                NpgsqlDataReader reader = await bookstoreCommand.ExecuteReaderAsync();

                while (reader.Read() && reader.HasRows)
                {
                    readBookstores.Add(new Bookstore{ 
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"],
                        Address = (string)reader["Address"],
                        Owner = (string)reader["Owner"]
                        });
                }
            }
            return readBookstores;
        }

        public async Task<bool> AddAsync(Bookstore bookstore)
        {
            Guid bookstoreGuid = Guid.NewGuid();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                int books = 0;

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
                    bookCommand.Parameters.AddWithValue("@Title", book.Title);
                    bookCommand.Parameters.AddWithValue("@Author", book.Author);

                    bookstoreInventoryCommand.Parameters.AddWithValue("@Id", Guid.NewGuid());
                    bookstoreInventoryCommand.Parameters.AddWithValue("@BookstoreId", bookstoreGuid);
                    bookstoreInventoryCommand.Parameters.AddWithValue("@BookId", bookGuid);

                    books++;
                }

                connection.Open();
                NpgsqlTransaction transaction = connection.BeginTransaction();
                bookstoreCommand.Transaction = bookCommand.Transaction = bookstoreInventoryCommand.Transaction = transaction;

                int bookstoreTableValueChanged = await bookstoreCommand.ExecuteNonQueryAsync();

                if(books > 0)
                {
                    await bookCommand.ExecuteNonQueryAsync();
                    await bookstoreInventoryCommand.ExecuteNonQueryAsync();
                }

                // If books have not been inserted, then their value will not change so we don't have to check them
                if (bookstoreTableValueChanged != 0)
                {
                    transaction.Commit();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> UpdateAsync(Bookstore bookstore)
        {
            int rowChanged;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand bookstoreCommand = new NpgsqlCommand();
                List<string> updatedValues = new List<string>();

                bookstoreCommand.Connection = connection;

                // Check if the value has beed changed
                if (bookstore.Name != null)
                {
                    updatedValues.Add("\"Name\" = @Name");
                    bookstoreCommand.Parameters.AddWithValue("@Name", bookstore.Name);
                }

                if (bookstore.Address != null)
                {
                    updatedValues.Add("\"Address\" = @Address");
                    bookstoreCommand.Parameters.AddWithValue("@Address", bookstore.Address);
                }

                if (bookstore.Owner != null)
                {
                    updatedValues.Add("\"Owner\" = @Owner");
                    bookstoreCommand.Parameters.AddWithValue("@Owner", bookstore.Owner);
                }

                if (updatedValues.Count == 0) return false;

                bookstoreCommand.CommandText = "UPDATE \"Bookstore\" SET " + string.Join(", ", updatedValues) + " WHERE \"Id\" = @Id";
                bookstoreCommand.Parameters.AddWithValue("@Id", bookstore.Id);

                connection.Open();
                rowChanged = await bookstoreCommand.ExecuteNonQueryAsync();
            }

            return rowChanged != 0;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            bool isSuccessful = false;

            string bookstoreQuery = "DELETE FROM \"Bookstore\" WHERE \"Id\" = @Id";
            string bookstoreInventoryQuery = "DELETE FROM \"BookstoreInventory\" WHERE \"BookstoreId\" = @Id";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand bookstoreCommand = new NpgsqlCommand(bookstoreQuery, connection);
                NpgsqlCommand bookstoreInventoryCommand = new NpgsqlCommand(bookstoreInventoryQuery, connection);
                bookstoreCommand.Parameters.AddWithValue("@Id", id);
                bookstoreInventoryCommand.Parameters.AddWithValue("@Id", id);

                connection.Open();
                NpgsqlTransaction transaction = connection.BeginTransaction();

                bookstoreCommand.Transaction = bookstoreInventoryCommand.Transaction = transaction;

                int rowChangedBookstoreInventory = await bookstoreInventoryCommand.ExecuteNonQueryAsync();
                int rowChangedBookstore = await bookstoreCommand.ExecuteNonQueryAsync();

                if (rowChangedBookstore != 0 && rowChangedBookstoreInventory != 0)
                {
                    transaction.Commit();
                    return true;
                }
            }

            return isSuccessful;
        }

        private void ReadBookstore(NpgsqlDataReader bookstoreRow)
        {
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
