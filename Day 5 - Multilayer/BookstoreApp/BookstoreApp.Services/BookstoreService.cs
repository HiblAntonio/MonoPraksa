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
        public IBookstoreRepository bookstoreRepository;

        public BookstoreService()
        {
            bookstoreRepository = new BookstoreRepository();
        }

        public Task<List<Bookstore>> Get()
        {
            return bookstoreRepository.Get();
        }

        public Task<Bookstore> Get(Guid id)
        {
            return bookstoreRepository.Get(id);
        }

        public Task<bool> Add(Bookstore bookstore)
        {
            return bookstoreRepository.Add(bookstore);
        }
        public Task<bool> Update(Bookstore bookstore)
        {
            return bookstoreRepository.Update(bookstore);
        }
        public Task<bool> Delete(Guid id)
        {
            return bookstoreRepository.Delete(id);
        }
    }
}
