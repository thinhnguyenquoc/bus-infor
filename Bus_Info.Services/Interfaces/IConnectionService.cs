using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Services.Interfaces
{
    public interface IConnectionService
    {
        List<ViewModelConnection> GetRouteConnection(int id_route, int is_checked);
        List<ViewModelConnection> GetAllConnection();
    }
}
