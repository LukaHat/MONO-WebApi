using Autofac.Integration.WebApi;
using Autofac;
using Example.Service.Interfaces;
using Example.Service;
using Example.WebApi.Controllers;
using Example.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

namespace Example.WebApi.App_Start
{
    public class DIConfig
    {
        public static void Register(HttpConfiguration config)
            {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<PokemonService>().As<IPokemonService>();
            builder.RegisterType<TrainerService>().As<ITrainerService>();
            builder.RegisterType<PokemonRepository>().As<IPokemonRepository>();
            builder.RegisterType<TrainerRepository>().As<ITrainerRepository>();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}