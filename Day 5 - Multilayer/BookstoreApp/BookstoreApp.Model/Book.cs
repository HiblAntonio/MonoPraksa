using BookstoreApp.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookstoreApp.Model
{
    public class Book : IBook
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public List<Bookstore> Bookstores = new List<Bookstore>();
    }
}
