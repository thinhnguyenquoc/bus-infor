using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.ViewModels
{
    public class ViewModelNewConnection
    {
        public int Id { get; set; }
        public int? RouteId { get; set; }
        public int? FromStationId { get; set; }
        public int? ToStationId { get; set; }
        public int OldId { get; set; }
        public string PolyLine { get; set; }
        public int Distance { get; set; }
        public int Type { get; set; }  //1:bus 2:walk 3:train

        public ViewModelStation FromStation { get; set; }
        public ViewModelStation ToStation { get; set; }
        public ViewModelRoute Route { get; set; }
    }
}
