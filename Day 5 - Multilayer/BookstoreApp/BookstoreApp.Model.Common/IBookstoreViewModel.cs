using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreApp.Model.Common
{
    public interface IBookstoreViewModel
    {
        string Name { get; set; }
        string Address { get; set; }
        string Owner { get; set; }
    }
}
