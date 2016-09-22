using Bus_Info.Services.Interfaces;
using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bus_Info.Admin.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _IUserService;
        private IRouteService _IRouteService;
        public HomeController(IUserService IUserService, IRouteService IRouteService)
        {
            _IUserService = IUserService;
            _IRouteService = IRouteService;
        }
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Empty()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ViewModelUser user)
        {
            if (_IUserService.CheckLogin(user.Email, user.Pass))
                return RedirectToAction("Dashboard");
            else
                return View("Index");
        }

        public ActionResult RouteView()
        {
            return View("_Route");
        }

        public ActionResult GetRoute()
        {
            List<ViewModelRoute> routes = _IRouteService.GetRoute();
            return Json(routes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PostRoute(ViewModelRoute route)
        {
            _IRouteService.UpdateRoute(route);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
	}
}