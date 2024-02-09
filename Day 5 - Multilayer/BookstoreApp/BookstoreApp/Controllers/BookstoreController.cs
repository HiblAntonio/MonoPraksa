using BookstoreApp.Model;
using BookstoreApp.Service;
using BookstoreApp.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookstoreApp.Controllers
{
    public class BookstoreController : ApiController
    {
        BookstoreService bookstoreService = new BookstoreService();

        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                List<BookstoreView> bookstores = (List<BookstoreView>)bookstoreService.Get().Select(x => new BookstoreView(x)).ToList();

                if(bookstores == null) return Request.CreateResponse(HttpStatusCode.NotFound);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception e) { return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message); };
        }

        [HttpGet]
        public HttpResponseMessage Get(Guid id)
        {
            try
            {
                if(id == null) return Request.CreateResponse(HttpStatusCode.BadRequest);

                Bookstore bookstore = bookstoreService.Get(id);

                if(bookstore == null) return Request.CreateResponse(HttpStatusCode.NotFound);
                return Request.CreateResponse(HttpStatusCode.OK, new BookstoreView(bookstore));

            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); };
        }

        public HttpResponseMessage Post(Bookstore bookstore)
        {
            try
            {
                if (bookstore == null) return Request.CreateResponse(HttpStatusCode.BadRequest);
                Bookstore bookstoreNew = new Bookstore()
                {
                    Id = Guid.NewGuid(),
                    Name = bookstore.Name,
                    Owner = bookstore.Owner,
                    Books = bookstore.Books
                };

                bool inserted = bookstoreService.Add(bookstoreNew);
                if (inserted) return Request.CreateResponse(HttpStatusCode.InternalServerError);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        public HttpResponseMessage Put(Guid id,[FromBody] Bookstore bookstore)
        {
            try
            {
                if(bookstore == null) return Request.CreateResponse(HttpStatusCode.BadRequest);
                bool updated = bookstoreService.Update(new Bookstore()
                {
                    Id = id,
                    Name = bookstore.Name,
                    Address = bookstore.Address,
                    Owner = bookstore.Owner
                });

                if (updated) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                if(id == null) return Request.CreateResponse(HttpStatusCode.BadRequest);
                bool deleted = bookstoreService.Delete(id);

                if (deleted) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            } 
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }
    }
}
