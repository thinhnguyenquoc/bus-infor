using Bus_Info.Services.Interfaces;
using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Bus_Info.Web.Controllers
{
    public class MemberController : Controller
    {
        private IUserService _IUserService;
        private IRouteService _IRouteService;
        private INewsService _INewsService;
        public MemberController(IUserService IUserService, IRouteService IRouteService, INewsService INewsService)
        {
            _IUserService = IUserService;
            _IRouteService = IRouteService;
            _INewsService = INewsService;
        }
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ViewModelUser user)
        {
            if (_IUserService.CheckLogin(user.Email, user.Pass))
            {
                FormsAuthentication.SetAuthCookie(user.Email, false);
                return RedirectToAction("Dashboard");
            }
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

        [Authorize]
        [HttpPost]
        public ActionResult PostRoute(ViewModelRoute route)
        {
            _IRouteService.UpdateRoute(route);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AddNews()
        {
            return View("_AddNews");
        }

        [Authorize]
        public ActionResult SubmitNews(ViewModelNews model)
        {
            _INewsService.UpdateNews(model);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditNews()
        {
            return View("_EditNews");
        }

        [Authorize]
        [HttpPost]
        public ActionResult FileUpload()
        {
            HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
            string wanted_path = System.AppDomain.CurrentDomain.BaseDirectory;
            var baseUrl = wanted_path + @"NewsImages\\" + hpf.FileName;
            using (Stream fileStream = new FileStream(baseUrl, FileMode.Create, FileAccess.Write))
            {
                hpf.InputStream.CopyTo(fileStream);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchNews(int Id)
        {
            var result = _INewsService.Search(Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
	}
}