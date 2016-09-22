using Bus_Info.Services;
using Bus_Info.Services.Interfaces;
using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Bus_Info.Web.Controllers
{
    public class Assignment2016Controller : Controller
    {
        private IPathSearchService _IPathSearchService;
        private IStationService _IStationService;
        public Assignment2016Controller(IPathSearchService IPathSearchService, IStationService IStationService)
        {
            _IPathSearchService = IPathSearchService;
            _IStationService = IStationService;
        }
        // GET: Assignment2016
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
                else if (file.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 3; //3 MB
                    string[] AllowedFileExtensions = new string[] { ".txt" };

                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    }

                    else if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                    }
                    else
                    {
                        List<string> list = new List<string>();
                        using (StreamReader reader = new StreamReader(file.InputStream))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                list.Add(line); // Add to list.
                            }
                        }
                        List<ViewModelNewConnection> newConnectionList = new List<ViewModelNewConnection>();
                        foreach (var line in list)
                        {
                            var items = line.Split(';');
                            var connect = new ViewModelNewConnection();
                            //stringline = line.Id + "; " + line.Distance + "; " + line.FromStationId + "; " + line.ToStationId + "; " + line.RouteId + "; " + line.OldId;
                            connect.Id = Convert.ToInt32(items[0].Trim());
                            connect.Distance = Convert.ToInt32(items[1].Trim());
                            connect.FromStationId = Convert.ToInt32(items[2].Trim());
                            connect.ToStationId = Convert.ToInt32(items[3].Trim());
                            connect.RouteId = Convert.ToInt32(items[4].Trim());
                            connect.OldId = Convert.ToInt32(items[5].Trim());
                            newConnectionList.Add(connect);
                        }
                        while (Graph.InitCompleted == false)
                        {

                        }
                        var model = _IPathSearchService.ChangeNewConnectionToOld(newConnectionList);

                        return View(model);
                    }
                }
            }
            return View();
        }

        // GET: Assignment2016
        public ActionResult ShowStation()
        {
            return View();
        }

        public ActionResult SuggestStation(double lat, double lng)
        {
            while (Graph.InitCompleted == false)
            {

            }
            List<ViewModelStation> stations = _IStationService.SuggestStation(lat, lng, 1);
            if (stations.Count > 0)
            {
                ViewModelNewStation sourceStation = Graph.newStationList.Where(x => x.OldId == stations[0].Id && (x.outVertex == false || x.outVertex == null)).FirstOrDefault();
                return Json(sourceStation, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}