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
    [RoutePrefix("api/bookstore")]
    public class BookstoreController : ApiController
    {
        private readonly IBookstoreService bookstoreService;

        public BookstoreController(IBookstoreService bookstoreService)
        {
            this.bookstoreService = bookstoreService;
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<HttpResponseMessage> Get(Guid id)
        {
            try
            {
                if(id == null) return Request.CreateResponse(HttpStatusCode.BadRequest);

                Bookstore bookstore = await bookstoreService.GetAsync(id);

                if(bookstore == null) return Request.CreateResponse(HttpStatusCode.NotFound);
                return Request.CreateResponse(HttpStatusCode.OK, new BookstoreView(bookstore));

            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); };
        }

        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> Get(string searchQuery = "", string itemSorting = "", bool isAsc = false, int pageNum = 1, int pageSize = 3)
        {
            try
            {
                List<Bookstore> bookstores = await bookstoreService.GetAsync(searchQuery, itemSorting, isAsc, pageNum, pageSize);

                if (bookstores == null) return Request.CreateResponse(HttpStatusCode.NotFound);
                return Request.CreateResponse(HttpStatusCode.OK, bookstores.Select(x => new BookstoreView(x)));
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> Post(BookstoreView bookstore)
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

                bool isInserted = await bookstoreService.AddAsync(bookstoreNew);
                if (isInserted) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<HttpResponseMessage> Put(Guid id, [FromBody] BookstoreView bookstore)
        {
            try
            {
                if(bookstore == null) return Request.CreateResponse(HttpStatusCode.BadRequest);
                bool isUpdated = await bookstoreService.UpdateAsync(new Bookstore()
                {
                    Id = id,
                    Name = bookstore.Name,
                    Address = bookstore.Address,
                    Owner = bookstore.Owner
                });

                if (isUpdated) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            try
            {
                if(id == null) return Request.CreateResponse(HttpStatusCode.BadRequest);
                bool isDeleted = await bookstoreService.DeleteAsync(id);

                if (isDeleted) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            } 
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }
    }
}
