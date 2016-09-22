using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Entities
{
    public class NewStation
    {
        public int Id { get; set; }      
        public int OldId { get; set; }
        public bool? OutVertex { get; set; }
    }
}
