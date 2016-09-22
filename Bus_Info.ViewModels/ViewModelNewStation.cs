using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.ViewModels
{
    public class ViewModelNewStation
    {
        public int Id { get; set; }      
        public int OldId { get; set; }
        public bool? outVertex { get; set; }
    }
}
