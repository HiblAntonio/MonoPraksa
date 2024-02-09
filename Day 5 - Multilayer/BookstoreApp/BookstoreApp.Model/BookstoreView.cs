using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreApp.Model
{
    public class BookstoreView
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Owner { get; set; }
        public List<Book> Books { get; set; }

        public BookstoreView(Bookstore bookstore)
        {
            Name = bookstore.Name;
            Address = bookstore.Address;
            Owner = bookstore.Owner;
        }
    }
}
