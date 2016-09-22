using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.ViewModels
{
    public class ViewModelFanOut
    {
        public int StationId { get; set; }
        public List<Vertex> Neighbor { get; set; }
    }
}
