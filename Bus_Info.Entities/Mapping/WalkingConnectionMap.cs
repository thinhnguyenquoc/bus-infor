using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Entities.Mapping
{
    public class WalkingConnectionMap : EntityTypeConfiguration<WalkingConnection>
    {
         public WalkingConnectionMap()
        {
            this.ToTable("WalkingConnections");
            this.HasKey(x => x.Id);
            this.Property(x => x.PolyLine).HasColumnName("PolyLine");
            this.Property(x => x.Distance).HasColumnName("Distance");
            this.Property(x => x.FromStationId).HasColumnName("FromStationId");
            this.Property(x => x.ToStationId).HasColumnName("ToStationId");
            this.Property(x => x.Time).HasColumnName("Time");

            this.HasOptional(x => x.FromStation).WithMany(x => x.FromWalkingConnections).HasForeignKey(x => x.FromStationId);
            this.HasOptional(x => x.ToStation).WithMany(x => x.ToWalkingConnections).HasForeignKey(x => x.ToStationId);
        }
    }
}
