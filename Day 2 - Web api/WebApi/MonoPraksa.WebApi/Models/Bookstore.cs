﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonoPraksa.WebApi.Models
{
    public class Bookstore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Owner { get; set; }
    }
}