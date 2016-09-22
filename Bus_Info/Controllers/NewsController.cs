using Bus_Info.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bus_Info.Web.Controllers
{
    public class NewsController : Controller
    {
        private INewsService _INewsService;
        public NewsController(INewsService INewsService)
        {
            _INewsService = INewsService;
        }
        //
        // GET: /News/
        public ActionResult Index()
        {
            var model = _INewsService.GetAllNews();
            return View(model);
        }

        public ActionResult GetDetail(int id)
        {
            var model = _INewsService.GetDetail(id);
            return View("Detail", model);
        }
    }
}