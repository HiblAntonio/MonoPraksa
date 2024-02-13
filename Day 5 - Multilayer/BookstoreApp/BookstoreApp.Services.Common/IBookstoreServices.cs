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
        Task<List<Bookstore>> GetAsync();
        Task<Bookstore> GetAsync(Guid id);
        Task<List<Bookstore>> GetAsync(string searchQuery, string itemSorting, bool isAsc, int pageNum, int pageSize);
        Task<bool> AddAsync(Bookstore bookstore);
        Task<bool> UpdateAsync(Bookstore bookstore);
        Task<bool> DeleteAsync(Guid id);
    }
}
