using AutoMapper;
using Bus_Info.Entities;
using Bus_Info.Repositories;
using Bus_Info.Repositories.Interfaces;
using Bus_Info.Services.Interfaces;
using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace Bus_Info.Services
{
    public static class Graph
    {
        public static List<ViewModelStation> oldStationList;
        public static List<ViewModelConnection> oldConnectionList;
        public static List<ViewModelWalkingConnection> oldWalkingConnectionList;
        public static List<ViewModelRoute> oldRouteList;
        public static List<ViewModelNewConnection> newConnectionList;
        public static List<ViewModelNewStation> newStationList;
        public static IDictionary<int, List<Vertex>> fanOut;
        public static bool Inited = false;
        public static bool InitCompleted = false;
        

        public static void Init()
        {
            if (Inited == false)
            {
                Inited = true;
                IRouteRepository _IRouteRepository = new RouteRepository();
                IRouteService _IRouteService = new RouteService(_IRouteRepository);
                IConnectionRepository _IConnectionRepository = new ConnectionRepository();
                IConnectionService _IConnectionService = new ConnectionService(_IConnectionRepository);
                IStationRepository _IStationRepository = new StationRepository();
                IStationService _IStationService = new StationService(_IStationRepository, _IConnectionRepository);
                IWalkingConnectionRepository _IWalkingConnectionRepository = new WalkingConnectionRepository();
                IWalkingConnectionService _IWalkingConnectionService = new WalkingConnectionService(_IWalkingConnectionRepository);
              
                oldStationList = _IStationService.GetAllStation();
                oldConnectionList = _IConnectionService.GetAllConnection();
                oldWalkingConnectionList = _IWalkingConnectionService.GetAllWalkingConnection();
                oldRouteList = _IRouteService.GetRoute();
                newConnectionList = new List<ViewModelNewConnection>();
                newStationList = new List<ViewModelNewStation>();
                fanOut = new Dictionary<int, List<Vertex>>();
                //Graph.CreateNewGraph();    
                Graph.CreateNewWalkingGraph(); 
                InitCompleted = true;
                _IRouteRepository.Dispose();
                _IConnectionRepository.Dispose();
                _IStationRepository.Dispose();
            }
        }
       
        public static void CreateNewGraph()
        {           
            int indexCon = 0;
            //new connection list
            foreach (var route in oldRouteList)
            {
                List<ViewModelConnection> tempconnectionList;
                for (int j = 0; j < 2; j++)
                {
                    tempconnectionList = oldConnectionList.Where(x => x.RouteId == route.Id && x.Arrive == Convert.ToBoolean(j)).OrderBy(x => x.Order).ToList();
                    //from A to B
                    for (int i = 0; i < tempconnectionList.Count - 1; i++)
                    {
                        ViewModelNewConnection newCon = new ViewModelNewConnection();
                        newCon.Id = indexCon++;
                        newCon.Distance = tempconnectionList[i + 1].Distance;
                        newCon.FromStationId = tempconnectionList[i].StationId;
                        newCon.ToStationId = tempconnectionList[i + 1].StationId;
                        newCon.RouteId = tempconnectionList[i + 1].RouteId;
                        newCon.OldId = tempconnectionList[i + 1].Id;
                        newConnectionList.Add(newCon);
                    }
                }
            }

            newStationList = new List<ViewModelNewStation>();
            int indexVertex = oldStationList.Count + 1;
            for (int i = 0; i < oldStationList.Count; i++)
            {
                //list fan in connection
                List<ViewModelNewConnection> faninConList = newConnectionList.Where(x => x.ToStationId == oldStationList[i].Id).ToList();

                //list fan out connection
                List<ViewModelNewConnection> fanoutConList = newConnectionList.Where(x => x.FromStationId == oldStationList[i].Id).ToList();

                //it's not cross or T cross
                if (faninConList.Count < 2 && fanoutConList.Count < 2)
                {
                    //do no thing
                    ViewModelNewStation newstation = new ViewModelNewStation();
                    newstation.Id = oldStationList[i].Id;
                    newstation.OldId = oldStationList[i].Id;
                    newStationList.Add(newstation);
                }
                // it's cross
                else
                {
                    List<ViewModelNewStation> faninVertexList = new List<ViewModelNewStation>();
                    // add fan in station
                    for (int j = 0; j < faninConList.Count; j++)
                    {
                        ViewModelNewStation inS = new ViewModelNewStation();
                        inS.Id = indexVertex++;
                        inS.OldId = oldStationList[i].Id;
                        inS.outVertex = false;
                        faninVertexList.Add(inS);
                        newStationList.Add(inS);
                    }

                    //list fan out station
                    List<ViewModelNewStation> fanoutVertexList = new List<ViewModelNewStation>();
                    // add fan out station
                    for (int j = 0; j < fanoutConList.Count; j++)
                    {
                        ViewModelNewStation outS = new ViewModelNewStation();
                        outS.Id = indexVertex++;
                        outS.OldId = oldStationList[i].Id;
                        outS.outVertex = true;
                        fanoutVertexList.Add(outS);
                        newStationList.Add(outS);
                    }

                    // map new fan in connection
                    for (int j = 0; j < faninConList.Count; j++)
                    {
                        ViewModelNewConnection con = newConnectionList.Where(x => x.ToStationId == oldStationList[i].Id && x.FromStationId == faninConList[j].FromStationId).FirstOrDefault();
                        con.ToStationId = faninVertexList[j].Id;
                    }

                    // map new fan out connection
                    for (int j = 0; j < fanoutConList.Count; j++)
                    {
                        ViewModelNewConnection con = newConnectionList.Where(x => x.FromStationId == oldStationList[i].Id && x.FromStationId == fanoutConList[j].FromStationId).FirstOrDefault();
                        con.FromStationId = fanoutVertexList[j].Id;
                    }

                    //map fan in to fan out
                    for (int j = 0; j < faninVertexList.Count; j++)
                    {
                        for (int k = 0; k < fanoutVertexList.Count; k++)
                        {
                            // the same route
                            if (faninConList[j].RouteId == fanoutConList[k].RouteId)
                            {
                                ViewModelNewConnection newCon = new ViewModelNewConnection();
                                newCon.Id = indexCon++;
                                newCon.Distance = 0;
                                newCon.FromStationId = faninConList[j].ToStationId;
                                newCon.ToStationId = fanoutConList[k].FromStationId;
                                newCon.RouteId = faninConList[j].RouteId;
                                newConnectionList.Add(newCon);
                            }
                            else
                            {
                                ViewModelNewConnection newCon = new ViewModelNewConnection();
                                newCon.Id = indexCon++;                               
                                newCon.Distance = Convert.ToInt32(ConfigurationManager.AppSettings["ChangeRoutePunishment"]) ;
                                newCon.FromStationId = faninConList[j].ToStationId;
                                newCon.ToStationId = fanoutConList[k].FromStationId;
                                newCon.RouteId = faninConList[j].RouteId;
                                newConnectionList.Add(newCon);
                            }
                        }
                    }
                }
            }
            //build fanout
            foreach (var station in newStationList)
            {              
                List<Vertex> neighbor = new List<Vertex>();
                var routePassList = newConnectionList.Where(x => x.FromStationId.HasValue && x.FromStationId.Value == station.Id).ToList();
                if (routePassList.Count > 0)
                {
                    foreach (var routePass in routePassList)
                    {
                        var neighborVertex = new Vertex()
                        {
                            Id = routePass.ToStationId.Value,
                            SourceDistance = routePass.Distance,
                            ParentId = station.Id,
                            ConnectionId = routePass.Id
                        };
                        neighbor.Add(neighborVertex);
                    }
                    //add fanout
                    fanOut.Add(station.Id, neighbor);
                }
            }

            //using (System.IO.StreamWriter file =
            //new System.IO.StreamWriter(@"D:\vertex.txt"))
            //{
            //    foreach (var line in newStationList)
            //    {
            //        string stringline = "";
            //        stringline = line.Id + "; " + line.OldId + "; " + line.outVertex;
            //        file.WriteLine(stringline);
            //    }
            //}

            //using (System.IO.StreamWriter file =
            //new System.IO.StreamWriter(@"D:\edge.txt"))
            //{
            //    foreach (var line in newConnectionList)
            //    {
            //        string stringline = "";
            //        stringline = line.Id + "; " + line.Distance + "; " + line.FromStationId + "; " + line.ToStationId + "; " + line.RouteId + "; " + line.OldId;
            //        file.WriteLine(stringline);
            //    }
            //}
            //using (var rep = new NewStationRepository())
            //{
            //    List<NewStation> res = Mapper.Map<List<ViewModelNewStation>, List<NewStation>>(newStationList);
            //    foreach (var i in res)
            //    {
            //        rep.InsertOrUpdate(i);
            //    }
            //    rep.Save();
            //}
            //using (var rep = new NewConnectionRepository())
            //{
            //    List<NewConnection> res = Mapper.Map<List<ViewModelNewConnection>, List<NewConnection>>(newConnectionList);
            //    foreach (var i in res)
            //    {
            //        rep.InsertOrUpdate(i);
            //    }
            //    rep.Save();
            //}
        }

        public static void CreateNewWalkingGraph()
        {
            string pathConnection = HttpContext.Current.Server.MapPath("~/App_Data/Graph/Connection.xml");
            string pathStation = HttpContext.Current.Server.MapPath("~/App_Data/Graph/Station.xml");
            string pathFanout = HttpContext.Current.Server.MapPath("~/App_Data/Graph/Fanout.xml");

            if (!File.Exists(pathConnection) || !File.Exists(pathStation) || !File.Exists(pathFanout))
            {
                int indexCon = 0;
                //new connection list
                foreach (var route in oldRouteList)
                {
                    List<ViewModelConnection> tempconnectionList;
                    for (int j = 0; j < 2; j++)
                    {
                        tempconnectionList = oldConnectionList.Where(x => x.RouteId == route.Id && x.Arrive == Convert.ToBoolean(j)).OrderBy(x => x.Order).ToList();
                        //from A to B
                        for (int i = 0; i < tempconnectionList.Count - 1; i++)
                        {
                            ViewModelNewConnection newCon = new ViewModelNewConnection();
                            newCon.Id = indexCon++;
                            newCon.Distance = tempconnectionList[i + 1].Distance;
                            newCon.FromStationId = tempconnectionList[i].StationId;
                            newCon.ToStationId = tempconnectionList[i + 1].StationId;
                            newCon.RouteId = tempconnectionList[i + 1].RouteId;
                            newCon.OldId = tempconnectionList[i + 1].Id;
                            newCon.PolyLine = tempconnectionList[i + 1].PolyLine;
                            newCon.Type = 1;
                            newConnectionList.Add(newCon);
                        }
                    }
                }

                // add walking connection
                foreach (var item in oldWalkingConnectionList)
                {
                    ViewModelNewConnection newCon = new ViewModelNewConnection();
                    newCon.Id = indexCon++;
                    newCon.Distance = item.Distance;
                    newCon.FromStationId = item.FromStationId;
                    newCon.ToStationId = item.ToStationId;
                    newCon.RouteId = -1;
                    newCon.OldId = item.Id;
                    newCon.PolyLine = item.PolyLine;
                    newCon.Type = 2;
                    newConnectionList.Add(newCon);
                }

                newStationList = new List<ViewModelNewStation>();
                int indexVertex = oldStationList.Count + 1;
                for (int i = 0; i < oldStationList.Count; i++)
                {
                    //list fan in connection
                    List<ViewModelNewConnection> faninConList = newConnectionList.Where(x => x.ToStationId == oldStationList[i].Id).ToList();

                    //list fan out connection
                    List<ViewModelNewConnection> fanoutConList = newConnectionList.Where(x => x.FromStationId == oldStationList[i].Id).ToList();

                    //it's not cross or T cross
                    if (faninConList.Count < 2 && fanoutConList.Count < 2)
                    {
                        //do no thing
                        ViewModelNewStation newstation = new ViewModelNewStation();
                        newstation.Id = oldStationList[i].Id;
                        newstation.OldId = oldStationList[i].Id;
                        newStationList.Add(newstation);
                    }
                    // it's cross
                    else
                    {
                        List<ViewModelNewStation> faninVertexList = new List<ViewModelNewStation>();
                        // add fanin station
                        for (int j = 0; j < faninConList.Count; j++)
                        {
                            ViewModelNewStation inS = new ViewModelNewStation();
                            inS.Id = indexVertex++;
                            inS.OldId = oldStationList[i].Id;
                            inS.outVertex = false;
                            faninVertexList.Add(inS);
                            newStationList.Add(inS);
                        }

                        //list fan out station
                        List<ViewModelNewStation> fanoutVertexList = new List<ViewModelNewStation>();
                        // add fan out station
                        for (int j = 0; j < fanoutConList.Count; j++)
                        {
                            ViewModelNewStation outS = new ViewModelNewStation();
                            outS.Id = indexVertex++;
                            outS.OldId = oldStationList[i].Id;
                            outS.outVertex = true;
                            fanoutVertexList.Add(outS);
                            newStationList.Add(outS);
                        }

                        // map new fan in connection
                        for (int j = 0; j < faninConList.Count; j++)
                        {
                            ViewModelNewConnection con = newConnectionList.Where(x => x.ToStationId == oldStationList[i].Id && x.FromStationId == faninConList[j].FromStationId).FirstOrDefault();
                            con.ToStationId = faninVertexList[j].Id;
                        }

                        // map new fan out connection
                        for (int j = 0; j < fanoutConList.Count; j++)
                        {
                            ViewModelNewConnection con = newConnectionList.Where(x => x.FromStationId == oldStationList[i].Id && x.FromStationId == fanoutConList[j].FromStationId).FirstOrDefault();
                            con.FromStationId = fanoutVertexList[j].Id;
                        }

                        //map fan in to fan out
                        for (int j = 0; j < faninVertexList.Count; j++)
                        {
                            for (int k = 0; k < fanoutVertexList.Count; k++)
                            {
                                ViewModelNewConnection newCon = new ViewModelNewConnection();
                                newCon.Id = indexCon++;
                                // the same route
                                if (faninConList[j].RouteId == fanoutConList[k].RouteId)
                                {
                                    if (fanoutConList[k].RouteId == -1)
                                    {
                                        newCon.Distance = Convert.ToInt32(ConfigurationManager.AppSettings["ChangeRoutePunishment"]);
                                    }
                                    else
                                    {
                                        newCon.Distance = 0;
                                    }
                                }
                                else
                                {
                                    if (fanoutConList[k].RouteId == -1)
                                        newCon.Distance = Convert.ToInt32(ConfigurationManager.AppSettings["ChangeWalkingRoutePunishment"]);
                                    else if (faninConList[j].RouteId != -1)
                                        newCon.Distance = Convert.ToInt32(ConfigurationManager.AppSettings["ChangeRoutePunishment"]);
                                    else
                                        newCon.Distance = 0;
                                }
                                newCon.FromStationId = faninConList[j].ToStationId;
                                newCon.ToStationId = fanoutConList[k].FromStationId;
                                newCon.RouteId = faninConList[j].RouteId;
                                newConnectionList.Add(newCon);
                            }
                        }
                    }
                }

                //build fanout
                var tempFanouts = new List<ViewModelFanOut>();
                foreach (var station in newStationList)
                {
                    List<Vertex> neighbor = new List<Vertex>();
                    var routePassList = newConnectionList.Where(x => x.FromStationId.HasValue && x.FromStationId.Value == station.Id).ToList();
                    if (routePassList.Count > 0)
                    {
                        foreach (var routePass in routePassList)
                        {
                            var neighborVertex = new Vertex()
                            {
                                Id = routePass.ToStationId.Value,
                                SourceDistance = routePass.Distance,
                                ParentId = station.Id,
                                ConnectionId = routePass.Id
                            };
                            neighbor.Add(neighborVertex);
                        }
                        var tempF = new ViewModelFanOut();
                        tempF.StationId = station.Id;
                        tempF.Neighbor = neighbor;
                        tempFanouts.Add(tempF);
                        //add fanout
                        fanOut.Add(station.Id, neighbor);
                    }
                }

                try
                {
                    //var writer = new System.Xml.Serialization.XmlSerializer(typeof(List<ViewModelNewConnection>),"connection");
                    //var wfile = new System.IO.StreamWriter(pathConnection);
                    //writer.Serialize(wfile, newConnectionList);
                    //wfile.Close();

                    //var writer2 = new System.Xml.Serialization.XmlSerializer(typeof(List<ViewModelNewStation>),"station");
                    //var wfile2 = new System.IO.StreamWriter(pathStation);
                    //writer2.Serialize(wfile2, newStationList);
                    //wfile2.Close();

                    using (StreamWriter myWriter = new StreamWriter(pathConnection,false))
                    {
                        XmlSerializer mySerializer = new XmlSerializer(typeof(List<ViewModelNewConnection>));
                        mySerializer.Serialize(myWriter, newConnectionList);
                    }

                    using (StreamWriter myWriter = new StreamWriter(pathStation, false))
                    {
                        XmlSerializer mySerializer = new XmlSerializer(typeof(List<ViewModelNewStation>));
                        mySerializer.Serialize(myWriter, newStationList);
                    }

                    using (StreamWriter myWriter = new StreamWriter(pathFanout, false))
                    {
                        XmlSerializer mySerializer = new XmlSerializer(typeof(List<ViewModelFanOut>));
                        mySerializer.Serialize(myWriter, tempFanouts);
                    }
                }
                catch (Exception e)
                {

                }
            }
            else
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(List<ViewModelNewConnection>));
                    System.IO.StreamReader file = new System.IO.StreamReader(pathConnection);
                    newConnectionList = (List<ViewModelNewConnection>)reader.Deserialize(file);
                    file.Close();
                                       
                    System.Xml.Serialization.XmlSerializer reader2 = new System.Xml.Serialization.XmlSerializer(typeof(List<ViewModelNewStation>));
                    System.IO.StreamReader file2 = new System.IO.StreamReader(pathStation);
                    newStationList = (List<ViewModelNewStation>)reader2.Deserialize(file2);
                    file2.Close();

                    System.Xml.Serialization.XmlSerializer reader3 = new System.Xml.Serialization.XmlSerializer(typeof(List<ViewModelFanOut>));
                    System.IO.StreamReader file3 = new System.IO.StreamReader(pathFanout);
                    var tempFanouts = (List<ViewModelFanOut>)reader3.Deserialize(file3);
                    file3.Close();

                    foreach (var i in tempFanouts)
                    {
                        fanOut.Add(i.StationId, i.Neighbor);
                    }
                }
                catch (Exception e)
                {

                }
            }

           
          
        }       

    }
}
