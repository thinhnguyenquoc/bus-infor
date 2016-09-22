using Bus_Info.Services;
using Bus_Info.Services.Interfaces;
using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bus_Info.Web.Controllers
{
    public class RouteController : Controller
    {
        private IRouteService _IRouteService;
        private IConnectionService _IConnectionService;
        public RouteController(IRouteService IRouteService, IConnectionService IConnectionService)
        {
            _IRouteService = IRouteService;
            _IConnectionService = IConnectionService;
        }
        //
        // GET: /RouteStation/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetRoute()
        {
            Graph.Init();
            List<ViewModelRoute> routes = _IRouteService.GetRoute();
            return Json(routes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRouteConnection(int id_route, int is_checked)
        {
            List<ViewModelConnection> routeConnection = _IConnectionService.GetRouteConnection(id_route, is_checked);
            return Json(routeConnection, JsonRequestBehavior.AllowGet);
        }
	}
}