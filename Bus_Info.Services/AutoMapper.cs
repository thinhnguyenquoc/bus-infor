using AutoMapper;
using Bus_Info.Entities;
using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Services
{
    public static class AutoMapper
    {
        private static bool mappingConfigured = false;

        public static void Configure()
        {
            // Used to create maps for whole viewmodels
            if (mappingConfigured)
            {
                return;
            }
           
            //route
            Mapper.CreateMap<Route, ViewModelRoute>();
            Mapper.CreateMap<ViewModelRoute, Route>();

            //station
            Mapper.CreateMap<Station, ViewModelStation>();
            Mapper.CreateMap<ViewModelStation, Station>();

            //connection
            Mapper.CreateMap<Connection, ViewModelConnection>();
            Mapper.CreateMap<ViewModelConnection, Connection>();

            //new connection
            Mapper.CreateMap<NewConnection, ViewModelNewConnection>();
            Mapper.CreateMap<ViewModelNewConnection, NewConnection>();

            //new station
            Mapper.CreateMap<NewStation, ViewModelNewStation>();
            Mapper.CreateMap<ViewModelNewStation, NewStation>();

            //view new station
            Mapper.CreateMap<ViewModelStation, ViewModelNewStation>();
            Mapper.CreateMap<ViewModelNewStation, ViewModelStation>();

            //view new connection
            Mapper.CreateMap<ViewModelConnection, ViewModelNewConnection>();
            Mapper.CreateMap<ViewModelNewConnection, ViewModelConnection>();

            //user
            Mapper.CreateMap<ViewModelUser, User>();
            Mapper.CreateMap<User, ViewModelUser>();

            //News
            Mapper.CreateMap<ViewModelNews, News>();
            Mapper.CreateMap<News, ViewModelNews>();

            //walkingconnection
            Mapper.CreateMap<WalkingConnection, ViewModelWalkingConnection>();
            Mapper.CreateMap<ViewModelWalkingConnection, WalkingConnection>();


            mappingConfigured = true;
        }

        public static void Reset()
        {
            Mapper.Reset();
            mappingConfigured = false;
        }
    }
}
