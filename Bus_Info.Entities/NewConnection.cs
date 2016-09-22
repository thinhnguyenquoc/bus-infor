using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Entities
{
    public class NewConnection
    {
        public int Id { get; set; }
        public int Distance { get; set; }
        public int? RouteId { get; set; }
        public int? FromStationId { get; set; }
        public int? ToStationId { get; set; }
        public int OldId { get; set; }
    }
}
