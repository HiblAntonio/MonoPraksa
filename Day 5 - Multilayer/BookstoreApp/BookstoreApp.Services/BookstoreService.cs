using BookstoreApp.Model;
using BookstoreApp.Repository;
using BookstoreApp.Repository.Common;
using BookstoreApp.Service.Common;
using System.Collections.Generic;
using System;

namespace BookstoreApp.Service
{
    public class BookstoreService : IBookstoreService
    {
        public IBookstoreRepository bookstoreRepository;

        public BookstoreService()
        {
            bookstoreRepository = new BookstoreRepository();
        }

        public List<Bookstore> Get()
        {
            return bookstoreRepository.Get();
        }

        public Bookstore Get(Guid id)
        {
            return (Bookstore)bookstoreRepository.Get(id);
        }

        public bool Add(Bookstore bookstore)
        {
            return bookstoreRepository.Add(bookstore);
        }
        public bool Update(Bookstore bookstore)
        {
            return bookstoreRepository.Update(bookstore);
        }
        public bool Delete(Guid id)
        {
            return bookstoreRepository.Delete(id);
        }
    }
}
