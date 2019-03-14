
namespace BankWork.App_Start
{
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Autofac.Integration.WebApi;
    using BankWork.Applibs;
    using BankWork.Domain.Repository;
    using BankWork.Persistent;
    using MongoDB.Driver;

    internal static class AutofacConfig
    {
        public static IContainer Container;

        public static void RegisterDependies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<MongoAccountRepository>()
                .WithParameter("mongoClient", new MongoClient(ConfigHelper.Mongodb))
                .As<IAccountRepository>();

            Container = builder.Build();

            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }
    }
}