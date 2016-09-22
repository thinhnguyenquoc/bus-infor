using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.ViewModels
{
    public class ViewModelConnection
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public bool Arrive { get; set; }
        public string PolyLine { get; set; }
        public int Distance { get; set; }
        public int RouteId { get; set; }
        public int StationId { get; set; }

        public ViewModelStation Station { get; set; }
        public ViewModelRoute Route { get; set; }
    }
}
