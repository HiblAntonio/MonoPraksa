using System.Collections.Generic;
using System;
using BookstoreApp.Model;
using System.Threading.Tasks;

namespace BookstoreApp.Repository.Common
{
    public interface IBookstoreRepository
    {
        Task<List<Bookstore>> Get();
        Task<Bookstore> Get(Guid id);
        Task<bool> Add(Bookstore bookstore);
        Task<bool> Update(Bookstore bookstore);
        Task<bool> Delete(Guid id);
    }
}