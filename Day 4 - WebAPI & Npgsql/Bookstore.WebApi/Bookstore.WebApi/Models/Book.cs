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
        public DateTime YearOfIssue { get; set; }
        public DateTime LastModified { get; set;}
        public List<BookstoreC> Bookstores { get; set; }
    }
}