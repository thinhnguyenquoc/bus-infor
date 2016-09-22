using AutoMapper;
using Bus_Info.Entities;
using Bus_Info.Repositories.Interfaces;
using Bus_Info.Services.Interfaces;
using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Services
{
    public class StationService : IStationService
    {
        private double yard = 0.00275;
        private IStationRepository _IStationRepository;
        private IConnectionRepository _IConnectionRepository;
        public StationService(IStationRepository IStationRepository, IConnectionRepository IConnectionRepository)
        {
            _IStationRepository = IStationRepository;
            _IConnectionRepository = IConnectionRepository;
        }
        public List<ViewModelStation> SuggestStation(double lat, double lng, int max){
            try
            {
                var original = new GeoCoordinate(lat, lng);
                //double u = lat + yard, d = lat - yard, l = lng - yard, r = lng + yard;
                List<ViewModelStation> result = new List<ViewModelStation>();
                double delta = 0;
                while (result.Count == 0)
                {
                    double u = lat + yard + delta, d = lat - yard- delta, l = lng - yard - delta, r = lng + yard + delta;
                    result = Graph.oldStationList.Where(x => x.Lat >= d && x.Lat <= u && x.Lng >= l && x.Lng <= r).ToList();
                    delta += 0.0005;
                }
                for (int i = 0; i < result.Count; i++ )
                {
                    var suggest = new GeoCoordinate(result[i].Lat, result[i].Lng);
                    result[i].DistanceTemp = original.GetDistanceTo(suggest);
                }
                if (max > 0)
                {
                    return result.OrderBy(x => x.DistanceTemp).Take(max).ToList();
                }
                else
                {
                    return result.OrderBy(x => x.DistanceTemp).Take(3).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<ViewModelStation> OriginalSuggestStation(double lat, double lng, int max)
        {
            try
            {
                var original = new GeoCoordinate(lat, lng);
                //double u = lat + yard, d = lat - yard, l = lng - yard, r = lng + yard;
                List<Station> rs = new List<Station>();
                double delta = 0;
                while (rs.Count == 0)
                {
                    double u = lat + yard + delta, d = lat - yard - delta, l = lng - yard - delta, r = lng + yard + delta;
                    rs = _IStationRepository.All.Where(x => x.Lat >= d && x.Lat <= u && x.Lng >= l && x.Lng <= r).ToList();
                    delta += 0.0005;
                }
                var result = Mapper.Map<List<Station>, List<ViewModelStation>>(rs);
                for (int i = 0; i < result.Count; i++)
                {
                    var suggest = new GeoCoordinate(result[i].Lat, result[i].Lng);
                    result[i].DistanceTemp = original.GetDistanceTo(suggest);
                }
                if (max > 0)
                {
                    return result.OrderBy(x => x.DistanceTemp).Take(max).ToList();
                }
                else
                {
                    return result.OrderBy(x => x.DistanceTemp).Take(3).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<List<ViewModelStation>> GetWalkingStations(int distance)
        {
            try
            {
                var result = new List<List<ViewModelStation>>();
                var rs = _IStationRepository.All.ToList();
                var rs1 = Mapper.Map<List<Station>, List<ViewModelStation>>(rs);
                for (int i = 0; i < rs1.Count; i++)
                {
                    for (int j = 0; j < rs1.Count; j++)
                    {
                        if (j != i)
                        {
                            var suggest1 = new GeoCoordinate(rs1[i].Lat, rs1[i].Lng);
                            var suggest2 = new GeoCoordinate(rs1[j].Lat, rs1[j].Lng);
                            var dis = suggest1.GetDistanceTo(suggest2);
                            if (dis <= distance)
                            {
                                var temp = new List<ViewModelStation>();
                                temp.Add(rs1[i]);
                                temp.Add(rs1[j]);
                                result.Add(temp);
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<ViewModelStation> SuggestStationName(string query)
        {
            try
            {
                var result = _IStationRepository.All.Where(x => x.Name.ToUpper().Contains(query.ToUpper()) || x.Address.ToUpper().Contains(query.ToUpper()) || x.Code.ToUpper().Contains(query.ToUpper())).ToList();
                var result1 = Mapper.Map<List<Station>, List<ViewModelStation>>(result);
                return result1.OrderBy(x => x.Address).Take(20).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<ViewModelStation> GetAllStation()
        {
            var stations = _IStationRepository.All.ToList();
            return Mapper.Map<List<Station>, List<ViewModelStation>>(stations);
        }

        public List<List<ViewModelConnection>> GetStationInfo(int Id)
        {
            List<List<ViewModelConnection>> Result = new List<List<ViewModelConnection>>();
            var route = _IConnectionRepository.All.Where(x => x.StationId == Id).Select(x => new { x.RouteId, x.Arrive }).ToList();
            foreach (var item in route)
            {
                List<ViewModelConnection> partOfResult = new List<ViewModelConnection>();
                var rs = _IConnectionRepository.AllIncluding(x=>x.Station,x=>x.Route).Where(x => x.RouteId == item.RouteId && x.Arrive == item.Arrive).ToList();
                partOfResult = Mapper.Map<List<Connection>, List<ViewModelConnection>>(rs);
                Result.Add(partOfResult);
            }
            return Result;
        }

        public ViewModelStation GetSation(double lat, double lng)
        {
            var temp = OriginalSuggestStation(lat,lng,1).FirstOrDefault();          
            return temp;
        }
    }
}
