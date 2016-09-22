using Bus_Info.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Services.Interfaces
{
    public interface IStationService
    {
        List<ViewModelStation> SuggestStation(double lat, double lng, int max);
        List<ViewModelStation> GetAllStation();
        List<ViewModelStation> SuggestStationName(string query);
        List<List<ViewModelConnection>> GetStationInfo(int Id);
        List<ViewModelStation> OriginalSuggestStation(double lat, double lng, int max);
        List<List<ViewModelStation>> GetWalkingStations(int distance);
        ViewModelStation GetSation(double lat, double lng);
    }
}
