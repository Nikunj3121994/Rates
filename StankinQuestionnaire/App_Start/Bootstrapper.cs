using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using System.Web.Mvc;
using StankinQuestionnaire.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StankinQuestionnaire.Model;
using StankinQuestionnaire.Data.Infrastructure.Store;
using StankinQuestionnaire.Data;
using StankinQuestionnaire.Data.Infrastructure;
using StankinQuestionnaire.Data.Repository;
using StankinQuestionnaire.Mappings;


namespace StankinQuestionnaire
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
            AutoMapperConfiguration.Configure();
        }

        public static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.Register(c => new UserManager<ApplicationUser, long>(new CustomUserStore(new StankinQuestionnaireEntities())))
                .As<UserManager<ApplicationUser, long>>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }

    }
}