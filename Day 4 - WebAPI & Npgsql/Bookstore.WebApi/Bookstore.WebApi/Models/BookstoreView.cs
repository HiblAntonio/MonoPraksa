using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore.WebApi.Models
{
    public class BookstoreView
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Owner { get; set; }
        public List<Book> Books { get; set; }

        public BookstoreView(BookstoreC bookstore)
        {
            Name = bookstore.Name;
            Address = bookstore.Address;
            Owner = bookstore.Owner;
            //if(bookstore.Books != null) Books = bookstore.Books;
        }
    }
}