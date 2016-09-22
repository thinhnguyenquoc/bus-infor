using Bus_Info.Services;
using Bus_Info.Services.Interfaces;
using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Bus_Info.Web.Controllers
{
    public class StationController : Controller
    {
        private IStationService _IStationService;
        public StationController(IStationService IStationService)
        {
            _IStationService = IStationService;
        }
        //
        // GET: /Station/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStation(string query)
        {
           var result = _IStationService.SuggestStationName(query);
           return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSuggestStation(double Lat, double Lng)
        {
            List<ViewModelStation> result = _IStationService.OriginalSuggestStation(Lat, Lng, 10);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStationInfo(int Id)
        {
            var result = _IStationService.GetStationInfo(Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
	}
}