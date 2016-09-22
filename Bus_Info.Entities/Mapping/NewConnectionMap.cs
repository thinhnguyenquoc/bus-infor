using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Entities.Mapping
{
    public class NewConnectionMap: EntityTypeConfiguration<NewConnection>
    {
        public NewConnectionMap()
        {
            this.ToTable("NewConnections");
            this.HasKey(x => x.Id);
            this.Property(x => x.Distance).HasColumnName("Distance");
            this.Property(x => x.RouteId).HasColumnName("RouteId");
            this.Property(x => x.FromStationId).HasColumnName("FromStationId");
            this.Property(x => x.ToStationId).HasColumnName("ToStationId");
            this.Property(x => x.OldId).HasColumnName("OldId");
        }
    }  
}
