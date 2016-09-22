using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.ViewModels
{
    public class ViewModelStation
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public double DistanceTemp { get; set; }
    }    
}
