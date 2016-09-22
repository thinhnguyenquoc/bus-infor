using Autofac;
using Bus_Info.Repositories;
using Bus_Info.Repositories.Interfaces;
using Bus_Info.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Services
{
    public class AutofacMapper : Module
    {
        public AutofacMapper()
        {            
        }
        protected override void Load(ContainerBuilder builder)
        {
           
            //Station
            builder.RegisterType<StationRepository>().As<IStationRepository>().InstancePerLifetimeScope();
            builder.RegisterType<StationService>().As<IStationService>().InstancePerLifetimeScope();

            //route
            builder.RegisterType<RouteRepository>().As<IRouteRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RouteService>().As<IRouteService>().InstancePerLifetimeScope();

            //Connection
            builder.RegisterType<ConnectionRepository>().As<IConnectionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ConnectionService>().As<IConnectionService>().InstancePerLifetimeScope();

            //new connection
            builder.RegisterType<NewConnectionRepository>().As<INewConnectionRepository>().InstancePerLifetimeScope();

            //new station
            builder.RegisterType<NewStationRepository>().As<INewStationRepository>().InstancePerLifetimeScope();
            
            //PathSearchService
            builder.RegisterType<PathSearchService>().As<IPathSearchService>().InstancePerLifetimeScope();

            //user
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();

            //news
            builder.RegisterType<NewsRepository>().As<INewsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<NewsService>().As<INewsService>().InstancePerLifetimeScope();

            //WalkingConnection
            builder.RegisterType<WalkingConnectionRepository>().As<IWalkingConnectionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<WalkingConnectionService>().As<IWalkingConnectionService>().InstancePerLifetimeScope();


            base.Load(builder);
        }
    }
}
