using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace Bookstore.WebApi.Models
{
    public class BookstoreController : ApiController
    {
        private NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=5432;Database=Bookstore");
        private static List<Guid> bookstoresId;
        List<BookstoreC> readBookstores;
        List<Book> books;

        // GET: Bookstore
        public HttpResponseMessage Get()
        {
            try
            {
                readBookstores = new List<BookstoreC>();

                using (NpgsqlConnection connectionSt = connection)
                {
                    string bookstoresQuery = "SELECT \"Bookstore\".\"Id\", \"Bookstore\".\"Name\", \"Bookstore\".\"Address\", \"Bookstore\".\"Owner\", \"Book\".\"Title\" FROM \"Bookstore\" LEFT JOIN \"BookstoreInventory\" ON \"Bookstore\".\"Id\" = \"BookstoreInventory\".\"BookstoreId\" LEFT JOIN \"Book\" ON \"Book\".\"Id\" = \"BookstoreInventory\".\"BookId\"";

                    NpgsqlCommand bookstoreCommand = new NpgsqlCommand(bookstoresQuery, connectionSt);

                    connectionSt.Open();
                    NpgsqlDataReader reader = bookstoreCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ReadBookstore(reader);
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, readBookstores.Select(x => new BookstoreView(x)));
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        public HttpResponseMessage Get(Guid id)
        {
            try
            {
                BookstoreC bookstore = null;

                using (NpgsqlConnection connectionSt = connection)
                {
                    string bookstoreQuery = "SELECT * FROM \"Bookstore\" WHERE \"Id\" = @Id";
                    NpgsqlCommand command = new NpgsqlCommand(bookstoreQuery, connectionSt);
                    command.Parameters.AddWithValue("id", id);

                    connectionSt.Open();

                    NpgsqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows && reader.Read())
                    {
                        bookstore = new BookstoreC
                        {
                            Id = id,
                            Name = (string)reader["Name"],
                            Address = (string)reader["Address"],
                            Owner = (string)reader["Owner"]
                        };
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new BookstoreView(bookstore));
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        // WORKS
        public HttpResponseMessage Post(BookstoreC bookstore)
        {
            // NpgSqlTransactions
            try
            {
                Guid bookstoreId = Guid.NewGuid();
                int rowsChangedBooks = 0;

                if (bookstore == null) return Request.CreateResponse(HttpStatusCode.BadRequest);

                int rowChangedBookstore, rowChangedBookstoreInventory = 0;
                string queryBookstore = "INSERT INTO \"Bookstore\" VALUES (@Id, @Name, @Address, @Owner)";
                string queryBookInformation = "INSERT INTO \"Book\" VALUES " +
                                                           "(@Id, @Title, @Author)";
                string sql = "INSERT INTO \"BookstoreInventory\" VALUES (@Id, @BookId, @BookstoreId)";

                using (NpgsqlCommand cmd = new NpgsqlCommand(queryBookstore, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", bookstoreId);
                    cmd.Parameters.AddWithValue("@Name", bookstore.Name);
                    cmd.Parameters.AddWithValue("@Address", bookstore.Address);
                    cmd.Parameters.AddWithValue("@Owner", bookstore.Owner);

                    connection.Open();

                    rowChangedBookstore = cmd.ExecuteNonQuery();
                }

                List<Guid> booksIds = new List<Guid>();
                int counter = 0;
                foreach (var book in bookstore.Books)
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand(queryBookInformation, connection))
                    {
                        booksIds.Add(Guid.NewGuid());

                        cmd.Parameters.AddWithValue("@Id", booksIds[counter]);
                        cmd.Parameters.AddWithValue("@Title", book.Title);
                        cmd.Parameters.AddWithValue("@Author", book.Author);

                        rowsChangedBooks += cmd.ExecuteNonQuery();

                    }

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Guid.NewGuid());
                        command.Parameters.AddWithValue("@BookstoreId", bookstoreId);
                        command.Parameters.AddWithValue("@BookId", booksIds[counter]);

                        rowChangedBookstoreInventory += command.ExecuteNonQuery();
                    }

                    counter++;
                }

                return RowChanged(rowChangedBookstore, rowsChangedBooks);
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        public HttpResponseMessage Put(Guid id,[FromBody] BookstoreC bookstore)
        {
            try
            {
                if (id == null || bookstore == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                int rowChanged;

                using (NpgsqlConnection connections = connection)
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    List<string> updatedValues = new List<string>();

                    cmd.Parameters.AddWithValue("@Id", bookstore.Id);

                    if (bookstore.Address != null)
                    {
                        updatedValues.Add("\"Address\" = @Address");
                        cmd.Parameters.AddWithValue("@Address", bookstore.Address);
                    }

                    if (bookstore.Name != null)
                    {
                        updatedValues.Add("\"Name\" = @Name");
                        cmd.Parameters.AddWithValue("@Name", bookstore.Name);
                    }

                    if (bookstore.Owner != null)
                    {
                        updatedValues.Add("\"Owner\" = @Owner");
                        cmd.Parameters.AddWithValue("@Owner", bookstore.Owner);
                    }

                    if (updatedValues.Count == 0) return Request.CreateResponse(HttpStatusCode.BadRequest);
                    cmd.CommandText = "UPDATE \"Bookstore\" SET " + string.Join(", ", updatedValues) + "WHERE \"Id\" = @Id";

                    connection.Open();

                    rowChanged = cmd.ExecuteNonQuery();
                }

                return RowChanged(rowChanged);
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                if (id == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                int rowChanged;
                string commandText = "DELETE FROM \"Bookstore\" WHERE Id = @Id";

                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    rowChanged = cmd.ExecuteNonQuery();
                }

                return RowChanged(rowChanged);
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError);}
        }

        private HttpResponseMessage RowChanged(int rowsChanged)
        {
            if (rowsChanged == 0) return Request.CreateResponse(HttpStatusCode.NotFound);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private HttpResponseMessage RowChanged(int rowsChangedQueryOne, int rowsChangedQueryTwo)
        {
            if (rowsChangedQueryOne == 0 || rowsChangedQueryTwo == 0) return Request.CreateResponse(HttpStatusCode.NotFound);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // Provjeriti postoji li već taj bookstore, ako postoji onda samo appendati taj book na listu
        // Gdje spremiti sve te prethodno učitane bookstorove?
        private void ReadBookstore(NpgsqlDataReader bookstoreRow)
        {
            // Title is the only field that can be null
            // Checking if the BookstoreId has been seen before
            string title = bookstoreRow["Title"] as string;

            BookstoreC targetBookstore = readBookstores.FirstOrDefault(x => x.Id == (Guid)bookstoreRow["Id"]);
            if (targetBookstore == null)
            {
                readBookstores.Add(new BookstoreC
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
                }) ;
            }
        }

        private List<Book> GetBook(dynamic bookstoreRow)
        {
            List<Book> books = new List<Book>();

            string query = "SELECT b.* FROM \"Book\" b " +
                           "INNER JOIN \"BookstoreInventory\" bi ON b.\"Id\" = bi.\"BookId\" " +
                           "WHERE bi.\"BookstoreId\" = @BookstoreId";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BookstoreId", (Guid)bookstoreRow["Id"]);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Book book = new Book
                        {
                            Title = (string)reader["Title"],
                        };

                        books.Add(book);
                    }
                }
            }

            return books;
        }
    }
}