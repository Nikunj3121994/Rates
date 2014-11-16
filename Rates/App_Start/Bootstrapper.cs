using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using System.Web.Mvc;
using Rates.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Rates.Model;
using Rates.Data.Infrastructure.Store;
using Rates.Data;
using Rates.Data.Infrastructure;
using Rates.Data.Repository;


namespace Rates
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
        }

        public static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>();

            builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.Register(c => new UserManager<ApplicationUser, long>(new CustomUserStore(new RatesEntities())))
                .As<UserManager<ApplicationUser, long>>().InstancePerRequest();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }

    }
}