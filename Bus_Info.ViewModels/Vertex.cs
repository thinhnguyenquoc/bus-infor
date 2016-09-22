using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.ViewModels
{
    public class Vertex : IComparable<Vertex>
    {
        public int Id { get; set; }
        public int SourceDistance { get; set; }
        public int ParentId { get; set; }
        public int ConnectionId { get; set; }

        public int CompareTo(Vertex obj)
        {
            if (this.SourceDistance > obj.SourceDistance)
                return 1;
            else if (this.SourceDistance == obj.SourceDistance)
                return 0;
            else
                return -1;
        }
    }
}
