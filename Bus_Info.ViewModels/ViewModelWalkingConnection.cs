using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.ViewModels
{
    public class ViewModelWalkingConnection
    {
        public int Id { get; set; }
        public int? FromStationId { get; set; }
        public int? ToStationId { get; set; }
        public int Distance { get; set; }
        public string PolyLine { get; set; }
        public int Time { get; set; }

        public ViewModelStation ToStation { get; set; }
        public ViewModelStation FromStation { get; set; }
    }
}
