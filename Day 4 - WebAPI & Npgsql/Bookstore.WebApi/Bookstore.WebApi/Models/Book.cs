using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore.WebApi.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        //public DateTime? YearOfIssue { get; set; }

        private List<BookstoreC> bookstores;

        public List<BookstoreC> GetBookstores()
        {
            return bookstores;
        }

        public void SetBookstores(List<BookstoreC> value)
        {
            bookstores = value;
        }
    }
}