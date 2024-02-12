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
        Task<List<Bookstore>> Get();
        Task<Bookstore> Get(Guid id);
        Task<bool> Add(Bookstore bookstore);
        Task<bool> Update(Bookstore bookstore);
        Task<bool> Delete(Guid id);
    }
}
