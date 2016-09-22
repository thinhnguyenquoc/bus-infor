using AutoMapper;
using Bus_Info.Entities;
using Bus_Info.Repositories;
using Bus_Info.Repositories.Interfaces;
using Bus_Info.Services.Interfaces;
using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Services
{
    public class PathSearchService: IPathSearchService
    {
        private IStationRepository _IStationRepository;
        private IConnectionRepository _IConnectionRepository;
        private IRouteRepository _IRouteRepository;
        private INewConnectionRepository _INewConnectionRepository;
        private INewStationRepository _INewStationRepository;

        public PathSearchService(IStationRepository IStationRepository, IConnectionRepository IConnectionRepository,
            IRouteRepository IRouteRepository, INewConnectionRepository INewConnectionRepository, INewStationRepository INewStationRepository)
        {
            _IStationRepository = IStationRepository;
            _IConnectionRepository = IConnectionRepository;
            _IRouteRepository = IRouteRepository;
            _INewConnectionRepository = INewConnectionRepository;
            _INewStationRepository = INewStationRepository;

        }
        
        public List<List<ViewModelConnection>> LabelingSetting(List<int> sourceIdList, List<int> sinkIdList)
        {
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            List<ViewModelStation> oldStationList = Graph.oldStationList;
            List<ViewModelConnection> oldConnectionList = Graph.oldConnectionList;
            List<ViewModelRoute> oldRouteList = Graph.oldRouteList;
            List<ViewModelNewConnection> newConnectionList = Graph.newConnectionList;
            List<ViewModelNewStation> newStationList = Graph.newStationList;
            IDictionary<int, List<Vertex>> fanOut = Graph.fanOut;

            List<List<Vertex>> pathList = new List<List<Vertex>>();
            List<ViewModelNewStation> sinkStation = newStationList.Where(x => sinkIdList.Contains(x.OldId) && (x.outVertex == false || x.outVertex == null)).ToList();
            var sink = new Dictionary<int, int>();
            for (int k = 0; k < sinkStation.Count; k++)
            {
                sink.Add(sinkStation[k].Id, sinkStation[k].Id);
            };

            List<ViewModelNewStation> sourceStation = newStationList.Where(x => sourceIdList.Contains(x.OldId) && (x.outVertex == true)).ToList();
            List<Vertex> allSourceVertex =  new List<Vertex>();
            foreach(var stat in sourceStation){
                var source = new Vertex()
                {
                    Id =stat.Id,
                    SourceDistance = 0
                };
                allSourceVertex.Add(source);
            }
            foreach (var source in allSourceVertex)
            {                
                List<Vertex> queue0 = new List<Vertex>();
                queue0.Add(source);
                // init queue
                SortedDictionary<int, List<Vertex>> Queue = new SortedDictionary<int, List<Vertex>>();
                Dictionary<int,int> QueueIdList = new Dictionary<int,int>();
                Queue.Add(source.SourceDistance, queue0);
                QueueIdList.Add(source.Id, source.SourceDistance);

                var result = new Dictionary<int, Vertex>();
                while (Queue.Count != 0)
                {
                    Vertex u = Queue.ElementAt(0).Value.ElementAt(0);
                    var item = Queue.ElementAt(0).Value;
                    result.Add(u.Id,u);
                    if (sink.ContainsKey(u.Id))
                        break;
                    //remove minimum item in queue
                    if (item.Count < 2)
                    {
                        Queue.Remove(u.SourceDistance);
                    }
                    else
                    {
                        item.RemoveAt(0);
                    }
                    QueueIdList.Remove(u.Id);
                    
                    // check neighbor
                    if (fanOut.ContainsKey(u.Id))
                    {
                        List<Vertex> neigbourVertexs = fanOut[u.Id];
                        for (int ii = 0; ii < neigbourVertexs.Count; ii++)
                        {
                            bool checkExistKey = QueueIdList.ContainsKey(neigbourVertexs[ii].Id);
                            // don't stay in queue
                            if (!checkExistKey)
                            {
                                // don't stay in result => new vertex
                                if (!result.ContainsKey(neigbourVertexs[ii].Id))
                                {
                                    Vertex vertex = new Vertex()
                                    {
                                        Id = neigbourVertexs[ii].Id,
                                        SourceDistance = u.SourceDistance + neigbourVertexs[ii].SourceDistance,
                                        ParentId = u.Id
                                    };
                                    if (Queue.ContainsKey(vertex.SourceDistance))
                                    {
                                        Queue[vertex.SourceDistance].Add(vertex);
                                        QueueIdList.Add(vertex.Id, vertex.SourceDistance);
                                    }
                                    else
                                    {
                                        List<Vertex> queueV = new List<Vertex>();
                                        queueV.Add(vertex);
                                        Queue.Add(vertex.SourceDistance, queueV);
                                        QueueIdList.Add(vertex.Id, vertex.SourceDistance);
                                    }
                                }
                                //else it has been result that mean it's minimum
                            }
                            else
                            {
                                Vertex v = Queue[QueueIdList[neigbourVertexs[ii].Id]].Where(x => x.Id == neigbourVertexs[ii].Id).FirstOrDefault();

                                if (v.SourceDistance > u.SourceDistance + neigbourVertexs[ii].SourceDistance)
                                {
                                    //remove v
                                    if (Queue[QueueIdList[neigbourVertexs[ii].Id]].Count < 2)
                                    {
                                        Queue.Remove(QueueIdList[neigbourVertexs[ii].Id]);
                                    }
                                    else
                                    {
                                        Queue[QueueIdList[neigbourVertexs[ii].Id]].Remove(v);
                                    }
                                    QueueIdList.Remove(v.Id);

                                    // add new v into queue
                                    Vertex vertex = new Vertex()
                                    {
                                        Id = neigbourVertexs[ii].Id,
                                        SourceDistance = u.SourceDistance + neigbourVertexs[ii].SourceDistance,
                                        ParentId = u.Id
                                    };
                                    if (Queue.ContainsKey(vertex.SourceDistance))
                                    {
                                        Queue[vertex.SourceDistance].Add(vertex);
                                        QueueIdList.Add(vertex.Id, vertex.SourceDistance);
                                    }
                                    else
                                    {
                                        List<Vertex> queueV = new List<Vertex>();
                                        queueV.Add(vertex);
                                        Queue.Add(vertex.SourceDistance, queueV);
                                        QueueIdList.Add(vertex.Id, vertex.SourceDistance);
                                    }
                                }
                                
                            }
                        }
                    }
                   
                }

                List<Vertex> path = new List<Vertex>();
                Vertex temp = result.LastOrDefault().Value;
                while (true)
                {
                    path.Add(temp);
                    if (temp.Id == source.Id)
                        break;
                    temp = result[temp.ParentId];
                }
                pathList.Add(path);
                
            }
            if (pathList.Count == 0)
                return null;

            int min = Int32.MaxValue;
            List<Vertex> finalpath = new List<Vertex>();
            for (int o = 0; o < pathList.Count; o++)
            {
                if (min > pathList[o][0].SourceDistance)
                {
                    min = pathList[o][0].SourceDistance;
                    finalpath = pathList[o];
                }
            }
            List<ViewModelNewConnection> newConResult = new List<ViewModelNewConnection>();
            for (int i = 0; i < finalpath.Count - 1; i++)
            {
                ViewModelNewConnection newcon = newConnectionList.Where(x => x.ToStationId == finalpath[i].Id && x.FromStationId == finalpath[i + 1].Id).FirstOrDefault();
                if (newcon != null)
                    newConResult.Add(newcon);
            }

            //using (System.IO.StreamWriter file =
            //new System.IO.StreamWriter(@"D:\test1.txt"))
            //{
            //    foreach (var line in newConResult)
            //    {
            //        string stringline = "";
            //        stringline = line.Id + "; " + line.Distance + "; " + line.FromStationId + "; " + line.ToStationId + "; " + line.RouteId + "; " + line.OldId;
            //        file.WriteLine(stringline);
            //    }
            //}

            var pathResult = ChangeNewConnectionToOld(newConResult);
            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;
            return pathResult;
        }

        public List<List<ViewModelConnection>> ChangeNewConnectionToOld(List<ViewModelNewConnection> newConnectionList)
        {
            List<ViewModelNewConnection> newgroupconResult = new List<ViewModelNewConnection>();
            int idGroup = -1;
            List<List<ViewModelConnection>> totalResult = new List<List<ViewModelConnection>>();
            List<ViewModelConnection> partialResult = new List<ViewModelConnection>();
            int lastId = -1;
            for (int l = newConnectionList.Count - 1; l > -1; l--)
            {
                if (newConnectionList[l].OldId != 0)
                {
                    lastId = l;
                    break;
                }
            }
            for (int jj = 0; jj < newConnectionList.Count; jj++)
            {
                if (newConnectionList[jj].OldId != 0)
                {
                    if (idGroup == -1)
                    {
                        idGroup = newConnectionList[jj].RouteId.Value;
                        ViewModelConnection end = Graph.oldConnectionList.Where(x=>x.Id == newConnectionList[jj].OldId).FirstOrDefault();
                        partialResult.Add(end);
                    }
                    else
                    {
                        // same route
                        if (idGroup == newConnectionList[jj].RouteId.Value)
                        {
                            ViewModelConnection end = Graph.oldConnectionList.Where(x=>x.Id == newConnectionList[jj].OldId).FirstOrDefault();
                            partialResult.Add(end);
                        }
                        else
                        {
                            idGroup = newConnectionList[jj].RouteId.Value;
                            totalResult.Add(partialResult);
                            partialResult = new List<ViewModelConnection>();
                            ViewModelConnection end = Graph.oldConnectionList.Where(x=>x.Id == newConnectionList[jj].OldId).FirstOrDefault();
                            partialResult.Add(end);
                        }
                    }
                    if (jj == lastId)
                    {
                        ViewModelConnection start = new ViewModelConnection();
                        start.Route = partialResult.LastOrDefault().Route;
                        start.Station = Graph.oldStationList.Where(x => x.Id == Graph.newStationList.Where(t => t.Id == newConnectionList[jj].FromStationId).FirstOrDefault().OldId).FirstOrDefault();
                        start.PolyLine = "";
                        partialResult.Add(start);
                    }
                }
            }
            totalResult.Add(partialResult);
            //start         
            return totalResult;
        }

        public List<List<ViewModelNewConnection>> WalkingLabelingSetting(List<int> sourceIdList, List<int> sinkIdList)
        {
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            List<ViewModelStation> oldStationList = Graph.oldStationList;
            List<ViewModelConnection> oldConnectionList = Graph.oldConnectionList;
            List<ViewModelRoute> oldRouteList = Graph.oldRouteList;
            List<ViewModelNewConnection> newConnectionList = Graph.newConnectionList;
            List<ViewModelNewStation> newStationList = Graph.newStationList;
            IDictionary<int, List<Vertex>> fanOut = Graph.fanOut;

            List<List<Vertex>> pathList = new List<List<Vertex>>();
            List<ViewModelNewStation> sinkStation = newStationList.Where(x => sinkIdList.Contains(x.OldId) && (x.outVertex == false || x.outVertex == null)).ToList();
            var sink = new Dictionary<int, int>();
            for (int k = 0; k < sinkStation.Count; k++)
            {
                sink.Add(sinkStation[k].Id, sinkStation[k].Id);
            };

            List<ViewModelNewStation> sourceStation = newStationList.Where(x => sourceIdList.Contains(x.OldId) && (x.outVertex == true)).ToList();
            List<Vertex> allSourceVertex = new List<Vertex>();
            foreach (var stat in sourceStation)
            {
                var source = new Vertex()
                {
                    Id = stat.Id,
                    SourceDistance = 0
                };
                allSourceVertex.Add(source);
            }
            foreach (var source in allSourceVertex)
            {
                List<Vertex> queue0 = new List<Vertex>();
                queue0.Add(source);
                // init queue
                SortedDictionary<int, List<Vertex>> Queue = new SortedDictionary<int, List<Vertex>>();
                Dictionary<int, int> QueueIdList = new Dictionary<int, int>();
                Queue.Add(source.SourceDistance, queue0);
                QueueIdList.Add(source.Id, source.SourceDistance);

                var result = new Dictionary<int, Vertex>();
                while (Queue.Count != 0)
                {
                    Vertex u = Queue.ElementAt(0).Value.ElementAt(0);
                    var item = Queue.ElementAt(0).Value;
                    result.Add(u.Id, u);
                    if (sink.ContainsKey(u.Id))
                        break;
                    //remove minimum item in queue
                    if (item.Count < 2)
                    {
                        Queue.Remove(u.SourceDistance);
                    }
                    else
                    {
                        item.RemoveAt(0);
                    }
                    QueueIdList.Remove(u.Id);

                    // check neighbor
                    if (fanOut.ContainsKey(u.Id))
                    {
                        List<Vertex> neigbourVertexs = fanOut[u.Id];
                        for (int ii = 0; ii < neigbourVertexs.Count; ii++)
                        {
                            bool checkExistKey = QueueIdList.ContainsKey(neigbourVertexs[ii].Id);
                            // don't stay in queue
                            if (!checkExistKey)
                            {
                                // don't stay in result => new vertex
                                if (!result.ContainsKey(neigbourVertexs[ii].Id))
                                {
                                    Vertex vertex = new Vertex()
                                    {
                                        Id = neigbourVertexs[ii].Id,
                                        SourceDistance = u.SourceDistance + neigbourVertexs[ii].SourceDistance,
                                        ParentId = u.Id
                                    };
                                    if (Queue.ContainsKey(vertex.SourceDistance))
                                    {
                                        Queue[vertex.SourceDistance].Add(vertex);
                                        QueueIdList.Add(vertex.Id, vertex.SourceDistance);
                                    }
                                    else
                                    {
                                        List<Vertex> queueV = new List<Vertex>();
                                        queueV.Add(vertex);
                                        Queue.Add(vertex.SourceDistance, queueV);
                                        QueueIdList.Add(vertex.Id, vertex.SourceDistance);
                                    }
                                }
                                //else it has been result that mean it's minimum
                            }
                            else
                            {
                                Vertex v = Queue[QueueIdList[neigbourVertexs[ii].Id]].Where(x => x.Id == neigbourVertexs[ii].Id).FirstOrDefault();

                                if (v.SourceDistance > u.SourceDistance + neigbourVertexs[ii].SourceDistance)
                                {
                                    //remove v
                                    if (Queue[QueueIdList[neigbourVertexs[ii].Id]].Count < 2)
                                    {
                                        Queue.Remove(QueueIdList[neigbourVertexs[ii].Id]);
                                    }
                                    else
                                    {
                                        Queue[QueueIdList[neigbourVertexs[ii].Id]].Remove(v);
                                    }
                                    QueueIdList.Remove(v.Id);

                                    // add new v into queue
                                    Vertex vertex = new Vertex()
                                    {
                                        Id = neigbourVertexs[ii].Id,
                                        SourceDistance = u.SourceDistance + neigbourVertexs[ii].SourceDistance,
                                        ParentId = u.Id
                                    };
                                    if (Queue.ContainsKey(vertex.SourceDistance))
                                    {
                                        Queue[vertex.SourceDistance].Add(vertex);
                                        QueueIdList.Add(vertex.Id, vertex.SourceDistance);
                                    }
                                    else
                                    {
                                        List<Vertex> queueV = new List<Vertex>();
                                        queueV.Add(vertex);
                                        Queue.Add(vertex.SourceDistance, queueV);
                                        QueueIdList.Add(vertex.Id, vertex.SourceDistance);
                                    }
                                }

                            }
                        }
                    }

                }

                List<Vertex> path = new List<Vertex>();
                Vertex temp = result.LastOrDefault().Value;
                while (true)
                {
                    path.Add(temp);
                    if (temp.Id == source.Id)
                        break;
                    temp = result[temp.ParentId];
                }
                pathList.Add(path);

            }
            if (pathList.Count == 0)
                return null;

            int min = Int32.MaxValue;
            List<Vertex> finalpath = new List<Vertex>();
            for (int o = 0; o < pathList.Count; o++)
            {
                if (min > pathList[o][0].SourceDistance)
                {
                    min = pathList[o][0].SourceDistance;
                    finalpath = pathList[o];
                }
            }
            List<ViewModelNewConnection> newConResult = new List<ViewModelNewConnection>();
            for (int i = 0; i < finalpath.Count - 1; i++)
            {
                ViewModelNewConnection newcon = newConnectionList.Where(x => x.ToStationId == finalpath[i].Id && x.FromStationId == finalpath[i + 1].Id).FirstOrDefault();
                if (newcon != null)
                    newConResult.Add(newcon);
            }

            var pathResult = GroupConnection(newConResult);
            return pathResult;
        }

        public List<List<ViewModelNewConnection>> GroupConnection(List<ViewModelNewConnection> newConnectionList)
        {
            List<ViewModelNewConnection> newgroupconResult = new List<ViewModelNewConnection>();
            int idGroup = -2;
            List<List<ViewModelNewConnection>> totalResult = new List<List<ViewModelNewConnection>>();
            List<ViewModelNewConnection> partialResult = new List<ViewModelNewConnection>();
            int lastId = -1;
            for (int l = newConnectionList.Count - 1; l > -1; l--)
            {
                if (newConnectionList[l].OldId != 0)
                {
                    lastId = l;
                    break;
                }
            }
            for (int jj = 0; jj < newConnectionList.Count; jj++)
            {
                if (newConnectionList[jj].OldId != 0)
                {
                    if (idGroup == -2)
                    {
                        idGroup = newConnectionList[jj].RouteId.Value;
                        newConnectionList[jj].Route = Graph.oldRouteList.Where(x => x.Id == newConnectionList[jj].RouteId).FirstOrDefault();
                        var FromNewStation = Graph.newStationList.Where(x => x.Id == newConnectionList[jj].FromStationId).FirstOrDefault();
                        newConnectionList[jj].FromStation = Graph.oldStationList.Where(x => x.Id == FromNewStation.OldId).FirstOrDefault();
                        var ToNewStation = Graph.newStationList.Where(x => x.Id == newConnectionList[jj].ToStationId).FirstOrDefault();
                        newConnectionList[jj].ToStation = Graph.oldStationList.Where(x => x.Id == ToNewStation.OldId).FirstOrDefault();

                        partialResult.Add(newConnectionList[jj]);
                    }
                    else
                    {
                        // same route
                        if (idGroup == newConnectionList[jj].RouteId.Value)
                        {
                            newConnectionList[jj].Route = Graph.oldRouteList.Where(x => x.Id == newConnectionList[jj].RouteId).FirstOrDefault();
                            var FromNewStation = Graph.newStationList.Where(x => x.Id == newConnectionList[jj].FromStationId).FirstOrDefault();
                            newConnectionList[jj].FromStation = Graph.oldStationList.Where(x => x.Id == FromNewStation.OldId).FirstOrDefault();
                            var ToNewStation = Graph.newStationList.Where(x => x.Id == newConnectionList[jj].ToStationId).FirstOrDefault();
                            newConnectionList[jj].ToStation = Graph.oldStationList.Where(x => x.Id == ToNewStation.OldId).FirstOrDefault();

                            partialResult.Add(newConnectionList[jj]);
                        }
                        else
                        {
                            idGroup = newConnectionList[jj].RouteId.Value;
                            newConnectionList[jj].Route = Graph.oldRouteList.Where(x => x.Id == newConnectionList[jj].RouteId).FirstOrDefault();
                            var FromNewStation = Graph.newStationList.Where(x => x.Id == newConnectionList[jj].FromStationId).FirstOrDefault();
                            newConnectionList[jj].FromStation = Graph.oldStationList.Where(x => x.Id == FromNewStation.OldId).FirstOrDefault();
                            var ToNewStation = Graph.newStationList.Where(x => x.Id == newConnectionList[jj].ToStationId).FirstOrDefault();
                            newConnectionList[jj].ToStation = Graph.oldStationList.Where(x => x.Id == ToNewStation.OldId).FirstOrDefault();

                            totalResult.Add(partialResult);
                            partialResult = new List<ViewModelNewConnection>();
                            partialResult.Add(newConnectionList[jj]);
                        }
                    }
                    //if (jj == lastId)
                    //{
                    //    ViewModelConnection start = new ViewModelConnection();
                    //    start.Route = partialResult.LastOrDefault().Route;
                    //    start.Station = Graph.oldStationList.Where(x => x.Id == Graph.newStationList.Where(t => t.Id == newConnectionList[jj].FromStationId).FirstOrDefault().OldId).FirstOrDefault();
                    //    start.PolyLine = "";
                    //    partialResult.Add(start);
                    //}
                }
            }
            totalResult.Add(partialResult);
            //start         
            return totalResult;
        }

    }
}
