using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Entities.Mapping
{
    public class ConnectionMap : EntityTypeConfiguration<Connection>
    {
        public ConnectionMap()
        {
            this.ToTable("Connections");
            this.HasKey(x => x.Id);
            this.Property(x => x.Order).HasColumnName("Order");
            this.Property(x => x.Arrive).HasColumnName("Arrive");
            this.Property(x => x.PolyLine).HasColumnName("PolyLine");
            this.Property(x => x.Distance).HasColumnName("Distance");
            this.Property(x => x.RouteId).HasColumnName("RouteId");
            this.Property(x => x.StationId).HasColumnName("StationId");

            this.HasOptional(x => x.Station).WithMany(x => x.Connections).HasForeignKey(x => x.StationId);
            this.HasOptional(x => x.Route).WithMany(x => x.Connections).HasForeignKey(x => x.RouteId);
        }
    }
}
