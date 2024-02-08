using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Bookstore.WebApi.Models
{
    public class BookstoreController : ApiController
    {
        private NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=5432;Database=Bookstore");
        private static List<BookstoreC> bookstores;

        // GET: Bookstore
        public HttpResponseMessage Get()
        {
            try
            {
                bookstores = new List<BookstoreC>();
                string commandText = "SELECT \"Bookstore\".\"Id\", \"Bookstore\".\"Name\", \"Bookstore\".\"Address\", \"Bookstore\".\"Owner\", \"Book\".\"Title\", \"Book\".\"YearOfIssue\" FROM \"Bookstore\"" +
                                     "LEFT JOIN \"BookstoreInventory\" ON \"Bookstore\".\"Id\" = \"BookstoreInventory\".\"BookstoreId\"" +
                                     "LEFT JOIN \"Book\" ON \"Book\".\"Id\" = \"BookstoreInventory\".\"BookId\"";
                
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
                {
                    connection.Open();
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            bookstores.Add(ReadBookstore(reader));
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, bookstores.Select(x => new BookstoreView(x)));
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        public HttpResponseMessage Get(Guid id)
        {
            try
            {
                if (id == null) return Request.CreateResponse(HttpStatusCode.BadRequest);
                string commandText = "SELECT \"Bookstore\".\"Id\", \"Bookstore\".\"Name\", \"Bookstore\".\"Address\", \"Bookstore\".\"Owner\", \"Book\".\"Title\", \"Book\".\"YearOfIssue\" FROM \"Bookstore\"" +
                                     "LEFT JOIN \"BookstoreInventory\" ON \"Bookstore\".\"Id\" = \"BookstoreInventory\".\"BookstoreId\"" +
                                     "LEFT JOIN \"Book\" ON \"Book\".\"Id\" = \"BookstoreInventory\".\"BookId\" WHERE \"Id\" = @Id";

                BookstoreC bookstore = null;

                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows && reader.Read())
                    {
                        bookstore = ReadBookstore(reader);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new BookstoreView(bookstore));
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        public HttpResponseMessage Post(BookstoreC bookstore)
        {
            try
            {
                int rowChangedBookstore, rowChangedBookstoreInventory;
                string queryBookstore = "INSERT INTO \"Bookstore\" VALUES (@Id, @Name, @Address, @Owner)";
                
                using (NpgsqlCommand cmd = new NpgsqlCommand(queryBookstore, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("@Name", bookstore.Name);
                    cmd.Parameters.AddWithValue("@Address", bookstore.Address);
                    cmd.Parameters.AddWithValue("@Owner", bookstore.Owner);

                    connection.Open();

                    rowChangedBookstore = cmd.ExecuteNonQuery();
                    connection.Close();
                }

                // Doesn't work
                using (NpgsqlConnection connectionOne = connection)
                {
                    connection.Open();

                    foreach (var book in bookstore.Books)
                    {
                        string queryBookstoreInformation = "INSERT INTO \"BookstoreInventory\" VALUES " +
                                                           "(@Id, @BookstoreId, @BookId)";

                        using (NpgsqlCommand cmd = new NpgsqlCommand(queryBookstoreInformation, connectionOne))
                        {
                            cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                            cmd.Parameters.AddWithValue("@BookstoreId", bookstore.Id);
                            cmd.Parameters.AddWithValue("@BookId", Guid.NewGuid());

                            rowChangedBookstoreInventory = cmd.ExecuteNonQuery();
                        }
                    }

                    connection.Close();
                }

                return RowChanged(rowChangedBookstore);
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
            if (rowsChanged == 0) return Request.CreateResponse(HttpStatusCode.InternalServerError);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private HttpResponseMessage RowChanged(int rowsChangedQueryOne, int rowsChangedQueryTwo)
        {
            if (rowsChangedQueryOne == 0 || rowsChangedQueryTwo == 0) return Request.CreateResponse(HttpStatusCode.InternalServerError);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private BookstoreC ReadBookstore(NpgsqlDataReader bookstoreRow)
        {
            try
            {
                bool bookstoreExists = false;
                BookstoreC existingBookstore = null;

                foreach (var bookstore in bookstores)
                {
                    if (bookstore.Id == (Guid)bookstoreRow["Id"])
                    {
                        bookstoreExists = true;
                        existingBookstore = bookstore;
                        break;
                    }
                }
                if (!bookstoreExists)
                {
                    return new BookstoreC
                    {
                        //Title, YearOfIssue
                        Id = (Guid)bookstoreRow["Id"],
                        Name = (string)bookstoreRow["Name"],
                        Address = (string)bookstoreRow["Address"],
                        Owner = (string)bookstoreRow["Owner"],
                        Books = GetBook(bookstoreRow)
                    };
                }
                else
                {
                    existingBookstore.Books.AddRange(GetBook(bookstoreRow));
                    return null;
                }
            }
            catch { return null; }
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