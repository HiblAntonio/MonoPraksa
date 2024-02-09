using System;

namespace BookstoreApp.Model.Common
{
    public interface IBookstore
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Address { get; set; }
        string Owner { get; set; }
    }
}
