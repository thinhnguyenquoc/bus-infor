using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Services.Interfaces
{
    public interface IWalkingConnectionService
    {
        List<ViewModelWalkingConnection> GetAllWalkingConnection();
        bool InsertOrUpdate(List<ViewModelWalkingConnection> walkingconnects);
    }
}
