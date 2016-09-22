using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Entities
{
    public class Route
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ServiceKind { get; set; }
        public string Distance { get; set; }
        public string TurnPerDay { get; set; }
        public string Duration { get; set; }
        public string NextBusTime { get; set; }
        public string WorkTime { get; set; }
        public string BusKind { get; set; }
        public string Owner { get; set; }
        public string TicketCost { get; set; }
        public string Note { get; set; }
        public double Speed { get; set; }

        public virtual ICollection<Connection> Connections { get; set; }
        public virtual ICollection<NewConnection> NewConnections { get; set; }
    }
}
