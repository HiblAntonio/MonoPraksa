using Autofac;
using Autofac.Integration.WebApi;
using BookstoreApp.Repository;
using BookstoreApp.Repository.Common;
using BookstoreApp.Service;
using BookstoreApp.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace BookstoreApp.App_Start
{
    public class DIConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<BookstoreService>().As<IBookstoreService>();
            builder.RegisterType<BookstoreRepository>().As<IBookstoreRepository>();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}