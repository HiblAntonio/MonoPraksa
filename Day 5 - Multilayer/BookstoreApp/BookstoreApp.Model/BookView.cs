using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreApp.Model
{
    internal class BookView
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishingDate { get; set; }

        public BookView(Book book)
        {
            Title = book.Title;
            Author = book.Author;
        }
    }
}
