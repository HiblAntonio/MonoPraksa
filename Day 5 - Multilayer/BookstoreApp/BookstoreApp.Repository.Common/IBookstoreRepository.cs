using System.Collections.Generic;
using System;
using BookstoreApp.Model;

namespace BookstoreApp.Repository.Common
{
    public interface IBookstoreRepository
    {
        List<Bookstore> Get();
        Bookstore Get(Guid id);
        bool Add(Bookstore bookstore);
        bool Update(Bookstore bookstore);
        bool Delete(Guid id);
    }
}