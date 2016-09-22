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
    public class WalkingConnectionService : IWalkingConnectionService
    {
        private IWalkingConnectionRepository _IWalkingConnectionRepository;

        public WalkingConnectionService(IWalkingConnectionRepository IWalkingConnectionRepository)
        {
            _IWalkingConnectionRepository = IWalkingConnectionRepository;
        }
      
        public List<ViewModelWalkingConnection> GetAllWalkingConnection()
        {
           return Mapper.Map<List<WalkingConnection>, List<ViewModelWalkingConnection>>(_IWalkingConnectionRepository.AllIncluding(x => x.FromStation,x=>x.ToStation).ToList<WalkingConnection>());
        }

        public bool InsertOrUpdate(List<ViewModelWalkingConnection> walkingconnects)
        {
            try
            {
                foreach (var i in walkingconnects)
                {
                    if (i.Distance < 250)
                    {
                        _IWalkingConnectionRepository.InsertOrUpdate(Mapper.Map<ViewModelWalkingConnection, WalkingConnection>(i));
                    }
                }
                _IWalkingConnectionRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
