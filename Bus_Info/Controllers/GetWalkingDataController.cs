using Bus_Info.Services.Interfaces;
using Bus_Info.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Bus_Info.Web.Controllers
{
    public static class ModelToJSon
    {
        public static string ToJson(this object obj)
        {
            var wr = new StringWriter();
            new JsonSerializer().Serialize(new JsonTextWriter(wr) { StringEscapeHandling = StringEscapeHandling.EscapeHtml }, obj);
            return wr.ToString();
        }

    }
    public class GetWalkingDataController : Controller
    {
        private IStationService _IStationService;
        private IWalkingConnectionService _IWalkingConnectionService;

        public GetWalkingDataController(IStationService IStationService, IWalkingConnectionService IWalkingConnectionService)
        {
            _IStationService = IStationService;
            _IWalkingConnectionService = IWalkingConnectionService;
        }

        //
        // GET: /GetWalkingData/
        public ActionResult Index()
        {
            var stations = new List<List<ViewModelStation>>();// _IStationService.GetWalkingStations(250);
            List<ViewModelWalkingConnection> walkingConnections = ReadData();
            return View(walkingConnections);
        }

        [HttpPost]
        public ActionResult WriteData(string data)
        {
            using (StreamWriter w = System.IO.File.AppendText(@"D:\walk.txt"))
            {
                w.WriteLine(data);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public List<ViewModelWalkingConnection> ReadData()
        {
            List<ViewModelWalkingConnection> results = new List<ViewModelWalkingConnection>();
            using (StreamReader file = new StreamReader(@"D:\walk.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    RootObject geo = JsonConvert.DeserializeObject<RootObject>(line);
                    ViewModelWalkingConnection wm = new ViewModelWalkingConnection();
                    var from = _IStationService.GetSation(geo.origin.geometry.coordinates[1], geo.origin.geometry.coordinates[0]);
                    var to = _IStationService.GetSation(geo.destination.geometry.coordinates[1], geo.origin.geometry.coordinates[0]);
                    wm.ToStationId = to.Id;
                    wm.FromStationId = from.Id;
                    wm.Distance = geo.routes[0].distance;
                    wm.Time = geo.routes[0].duration;
                    wm.PolyLine = new JavaScriptSerializer().Serialize(geo.routes[0].geometry.coordinates).ToString();
                    results.Add(wm);
                }
            }
            return results;
        }

        [HttpPost]
        public ActionResult Submit(List<ViewModelWalkingConnection> data, int totalRow)
        {
            if (Session["temp"] != null)
            {
                List<ViewModelWalkingConnection> old = (List<ViewModelWalkingConnection>)Session["temp"];
                List<ViewModelWalkingConnection> result = old.Concat(data).ToList();
                if (result.Count == totalRow)
                {
                    _IWalkingConnectionService.InsertOrUpdate(result);
                }
                else
                {
                    Session["temp"] = result;
                }
            }
            else
            {
                Session["temp"] = data;
            }
            return Json(1, JsonRequestBehavior.AllowGet);

        }
    }
    public class Geometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
    }

    public class Origin
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

    public class Geometry2
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Properties2
    {
        public string name { get; set; }
    }

    public class Destination
    {
        public string type { get; set; }
        public Geometry2 geometry { get; set; }
        public Properties2 properties { get; set; }
    }

    public class Location
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Maneuver
    {
        public string instruction { get; set; }
        public string type { get; set; }
        public Location location { get; set; }
    }

    public class Step
    {
        public int distance { get; set; }
        public int duration { get; set; }
        public string way_name { get; set; }
        public string mode { get; set; }
        public string direction { get; set; }
        public int heading { get; set; }
        public Maneuver maneuver { get; set; }
    }

    public class Geometry3
    {
        public string type { get; set; }
        public List<List<double>> coordinates { get; set; }
    }

    public class Route
    {
        public int distance { get; set; }
        public int duration { get; set; }
        public List<Step> steps { get; set; }
        public Geometry3 geometry { get; set; }
        public string summary { get; set; }
    }

    public class RootObject
    {
        public Origin origin { get; set; }
        public Destination destination { get; set; }
        public List<object> waypoints { get; set; }
        public List<Route> routes { get; set; }
    }

}