using BookstoreApp.Model;
using BookstoreApp.Service;
using BookstoreApp.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BookstoreApp.Controllers
{
    public class BookstoreController : ApiController
    {
        private readonly IBookstoreService bookstoreService;

        public BookstoreController()
        {
            bookstoreService = new BookstoreService();
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                var bookstores = await bookstoreService.Get(); //.Select(x => new BookstoreView(x)).ToList();
                var bookstoresView = bookstores.Select(x => new BookstoreView(x)).ToList();

                if(bookstoresView == null) return Request.CreateResponse(HttpStatusCode.NotFound);
                return Request.CreateResponse(HttpStatusCode.OK, bookstoresView);
            }
            catch(Exception e) { return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message); };
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Get(Guid id)
        {
            try
            {
                if(id == null) return Request.CreateResponse(HttpStatusCode.BadRequest);

                Bookstore bookstore = await bookstoreService.Get(id);

                if(bookstore == null) return Request.CreateResponse(HttpStatusCode.NotFound);
                return Request.CreateResponse(HttpStatusCode.OK, new BookstoreView(bookstore));

            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); };
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(Bookstore bookstore)
        {
            try
            {
                if (bookstore == null) return Request.CreateResponse(HttpStatusCode.BadRequest);
                Bookstore bookstoreNew = new Bookstore()
                {
                    Id = Guid.NewGuid(),
                    Name = bookstore.Name,
                    Address = bookstore.Address,
                    Owner = bookstore.Owner,
                    Books = bookstore.Books
                };

                bool inserted = await bookstoreService.Add(bookstoreNew);
                if (inserted) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(Guid id, [FromBody] Bookstore bookstore)
        {
            try
            {
                if(bookstore == null) return Request.CreateResponse(HttpStatusCode.BadRequest);
                bool updated = await bookstoreService.Update(new Bookstore()
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

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            try
            {
                if(id == null) return Request.CreateResponse(HttpStatusCode.BadRequest);
                bool deleted = await bookstoreService.Delete(id);

                if (deleted) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            } 
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }
    }
}
