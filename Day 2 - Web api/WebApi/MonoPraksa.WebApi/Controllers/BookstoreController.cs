using Microsoft.Ajax.Utilities;
using MonoPraksa.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;


namespace MonoPraksa.WebApi.Controllers
{
    public class BookstoreController : ApiController
    {
        // C - Post
        // R - Get
        // U - Put
        // D - Delete

        private static List<Bookstore> bookstores = new List<Bookstore>();

        [HttpGet]
        public HttpResponseMessage Get([FromBody] Bookstore bookstore)
        {
            try
            {
                List<Bookstore> returnBookstore = bookstores;

                if (!string.IsNullOrEmpty(bookstore.Address)) returnBookstore = returnBookstore.Where(x => x.Address.ToLower().Contains(bookstore.Address.ToLower())).ToList();
                if (!string.IsNullOrEmpty(bookstore.Name)) returnBookstore = returnBookstore.Where(x => x.Name.ToLower().Contains(bookstore.Name.ToLower())).ToList();
                if (!string.IsNullOrEmpty(bookstore.Owner)) returnBookstore = returnBookstore.Where(x => x.Owner.ToLower().Contains(bookstore.Owner.ToLower())).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, returnBookstore.Select(x => new BookstoreView(x)));
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] CreateBookstore newBookstore)
        {
            try
            {
                // Make a seperate function that checks is all properties of an object are filled
                // This is the first one and the second one is in "Put" method - Violation of a "DRY" principle
                if (string.IsNullOrEmpty(newBookstore.Name) || string.IsNullOrEmpty(newBookstore.Address) || string.IsNullOrEmpty(newBookstore.Owner))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "All properties must be filled!");

                Bookstore bookstore = new Bookstore()
                {
                    Id = bookstores.Count() == 0 ? 1 : bookstores.Max(x => x.Id) + 1,
                    Name = newBookstore.Name,
                    Address = newBookstore.Address,
                    Owner = newBookstore.Owner
                };

                bookstores.Add(bookstore);
                return Request.CreateResponse(HttpStatusCode.OK, new BookstoreView(bookstore));
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody] UpdateBookstore bookstore)
        {
            try
            {
                if (string.IsNullOrEmpty(bookstore.Name) || string.IsNullOrEmpty(bookstore.Address) || string.IsNullOrEmpty(bookstore.Owner))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "All properties must be filled!");

                Bookstore oldBookstoreInfo = bookstores.FirstOrDefault(x => x.Id == id);

                if (oldBookstoreInfo == null) return Request.CreateResponse(HttpStatusCode.NotFound, "Invalid ID");
                oldBookstoreInfo.Name = bookstore.Name;
                oldBookstoreInfo.Address = bookstore.Address;
                oldBookstoreInfo.Owner = bookstore.Owner;

                return Request.CreateResponse(HttpStatusCode.OK, new BookstoreView(oldBookstoreInfo));
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); };
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (bookstores.FirstOrDefault(x => x.Id == id) == null) return Request.CreateResponse(HttpStatusCode.NotFound, "Invalid ID");
                bookstores.Remove(bookstores.FirstOrDefault(x => x.Id == id));

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }
    }
}