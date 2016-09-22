using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Entities
{
    public class Station
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }

        public virtual ICollection<Connection> Connections { get; set; }
        public virtual ICollection<WalkingConnection> FromWalkingConnections { get; set; }
        public virtual ICollection<WalkingConnection> ToWalkingConnections { get; set; }
    }
}
