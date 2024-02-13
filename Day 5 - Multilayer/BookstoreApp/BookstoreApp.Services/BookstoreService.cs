using BookstoreApp.Model;
using BookstoreApp.Repository;
using BookstoreApp.Repository.Common;
using BookstoreApp.Service.Common;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace BookstoreApp.Service
{
    public class BookstoreService : IBookstoreService
    {
        private readonly IBookstoreRepository bookstoreRepository;

        public BookstoreService(IBookstoreRepository bookstoreRepository)
        {
            this.bookstoreRepository = bookstoreRepository;
        }

        public async Task<List<Bookstore>> GetAsync()
        {
            return await bookstoreRepository.GetAsync();
        }

        public async Task<Bookstore> GetAsync(Guid id)
        {
            return await bookstoreRepository.GetAsync(id);
        }

        public async Task<List<Bookstore>> GetAsync(string searchQuery, string itemSorting, bool isAsc, int pageNum, int pageSize)
        {
            return await bookstoreRepository.GetAsync(searchQuery, itemSorting, isAsc, pageNum, pageSize);
        }

        public async Task<bool> AddAsync(Bookstore bookstore)
        {
            return await bookstoreRepository.AddAsync(bookstore);
        }
        public async Task<bool> UpdateAsync(Bookstore bookstore)
        {
            return await bookstoreRepository.UpdateAsync(bookstore);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await bookstoreRepository.DeleteAsync(id);
        }
    }
}
