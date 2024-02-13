using System.Collections.Generic;
using System;
using BookstoreApp.Model;
using System.Threading.Tasks;

namespace BookstoreApp.Repository.Common
{
    public interface IBookstoreRepository
    {
        Task<List<Bookstore>> GetAsync();
        Task<Bookstore> GetAsync(Guid id);
        Task<List<Bookstore>> GetAsync(string searchQuery, string itemSorting, bool isAsc, int pageNum, int pageSize);
        Task<bool> AddAsync(Bookstore bookstore);
        Task<bool> UpdateAsync(Bookstore bookstore);
        Task<bool> DeleteAsync(Guid id);
    }
}