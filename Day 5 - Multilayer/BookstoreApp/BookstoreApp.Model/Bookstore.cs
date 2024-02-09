using BookstoreApp.Model.Common;
using System.Collections.Generic;
using System;

namespace BookstoreApp.Model
{
    public class Bookstore : IBookstore
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Owner { get; set; }
        public List<Book> Books = new List<Book>();
    }
}
