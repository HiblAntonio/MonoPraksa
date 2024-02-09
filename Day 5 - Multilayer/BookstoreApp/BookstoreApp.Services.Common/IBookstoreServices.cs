using BookstoreApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreApp.Service.Common
{
    public interface IBookstoreService
    {
        List<Bookstore> Get();
        Bookstore Get(Guid id);
        bool Add(Bookstore bookstore);
        bool Update(Bookstore bookstore);
        bool Delete(Guid id);
    }
}
