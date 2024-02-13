using System;

namespace BookstoreApp.Common
{
    public class CommonProperty
    {
        public static string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=5432;Database=Bookstore";

        public class Paging
        {
            public int PageNum { get; set; }
            public int PageSize { get; set; }
        }

        public class Sorting
        {
            public bool IsAsc { get; set; }
        }

        public class Filtering
        {
            public string SearchQuery { get; set; }
        }
    }
}
