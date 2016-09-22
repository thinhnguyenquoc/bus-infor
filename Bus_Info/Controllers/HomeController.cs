using Bus_Info.Services;
using Bus_Info.Services.Interfaces;
using Bus_Info.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bus_Info.Web.Controllers
{
    
    public class HomeController : Controller
    {
        private IStationService _IStationService;
        private IPathSearchService _IPathSearchService;
        private IRouteService _IRouteService;
        private IConnectionService _IConnectionService;
        public HomeController(IStationService IStationService, IPathSearchService IPathSearchService, IRouteService IRouteService, IConnectionService IConnectionService)
        {
            _IStationService = IStationService;
            _IPathSearchService = IPathSearchService;
            _IRouteService = IRouteService;
            _IConnectionService = IConnectionService;            
        }

        // GET: Home       
        public ActionResult Index()
        {
            //search test case
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //List<ViewModelStation> fromStation = _IStationService.SuggestStation(10.75042663352999, 106.72385215759277,3);
            //List<ViewModelStation> toStation = _IStationService.SuggestStation(10.773277646189221, 106.62171363830566,3);
            //if (fromStation == null || toStation == null)
            //    return Json(null, JsonRequestBehavior.AllowGet);
            //var result = _IPathSearchService.LabelingSetting(fromStation.Select(x=>x.Id).ToList(), toStation.Select(x=>x.Id).ToList());
            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;
            return View();
        }

        public ActionResult SuggestStation(double lat, double lng)
        {
            List<ViewModelStation> stations = _IStationService.SuggestStation(lat, lng,3);
            return Json(stations, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(double fromLat, double fromLng, double toLat, double toLng)
        {
            while (Graph.InitCompleted == false)
            {

            }
            List<ViewModelStation> fromStation = _IStationService.SuggestStation(fromLat, fromLng, 5);
            List<ViewModelStation> toStation = _IStationService.SuggestStation(toLat, toLng, 5);
            if(fromStation == null || toStation == null)
                return Json(null, JsonRequestBehavior.AllowGet);
            //var result = _IPathSearchService.LabelingSetting(fromStation.Select(x => x.Id).ToList(), toStation.Select(x => x.Id).ToList());
            var result = _IPathSearchService.WalkingLabelingSetting(fromStation.Select(x => x.Id).ToList(), toStation.Select(x => x.Id).ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitGraph()
        {
            Graph.Init();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}