using AutoMapper;
using Bus_Info.Entities;
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
    public class ConnectionService : IConnectionService
    {
        private IConnectionRepository _IConnectionRepository;

        public ConnectionService(IConnectionRepository IConnectionRepository)
        {
            _IConnectionRepository = IConnectionRepository;
        }
        public List<ViewModelConnection> GetRouteConnection(int id_route, int is_checked)
        {
            bool arrive = Convert.ToBoolean(is_checked);
            return Mapper.Map<List<Connection>,List<ViewModelConnection>>(_IConnectionRepository.AllIncluding(x=>x.Station).Where(x => x.RouteId == id_route && x.Arrive == arrive).ToList<Connection>());
        }

        public List<ViewModelConnection> GetAllConnection()
        {
           return Mapper.Map<List<Connection>, List<ViewModelConnection>>(_IConnectionRepository.AllIncluding(x => x.Station,x=>x.Route).ToList<Connection>());
        }
    }
}
