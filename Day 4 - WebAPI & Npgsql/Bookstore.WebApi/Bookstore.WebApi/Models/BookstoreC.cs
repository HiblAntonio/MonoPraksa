using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore.WebApi.Models
{
    public class BookstoreC
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Owner { get; set; }
        public List<Book> Books { get; set; }
    }
}