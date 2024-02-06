﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonoPraksa.WebApi.Models
{
    public class BookstoreView
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Owner { get; set; }

        public BookstoreView(Bookstore bookstore) 
        {
            Name = bookstore.Name;
            Address = bookstore.Address;
            Owner = bookstore.Owner;
        }
    }
}