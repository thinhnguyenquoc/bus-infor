using Bus_Info.Repositories;
using Bus_Info.Repositories.Interfaces;
using Bus_Info.Services;
using Bus_Info.Services.Interfaces;
using Bus_Info.ViewModels;
using Bus_Info.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bus_Info.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AutofacMapperConfig.ConfigureContainer();
            AutoMapper.Configure();
        }
    }
}
