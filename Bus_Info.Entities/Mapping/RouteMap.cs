using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Entities.Mapping
{
    public class RouteMap : EntityTypeConfiguration<Route>
    {
        public RouteMap()
        {
            this.ToTable("Routes");
            this.HasKey(x=>x.Id);
            this.Property(x => x.Code).HasColumnName("Code");
            this.Property(x => x.Name).HasColumnName("Name");
            this.Property(x => x.Note).HasColumnName("Note");
            this.Property(x => x.BusKind).HasColumnName("BusKind");
            this.Property(x => x.Distance).HasColumnName("Distance");
            this.Property(x => x.Duration).HasColumnName("Duration");
            this.Property(x => x.NextBusTime).HasColumnName("NextBusTime");
            this.Property(x => x.ServiceKind).HasColumnName("ServiceKind");
            this.Property(x => x.TurnPerDay).HasColumnName("TurnPerDay");
            this.Property(x => x.WorkTime).HasColumnName("WorkTime");
            this.Property(x => x.Owner).HasColumnName("Owner");
            this.Property(x => x.TicketCost).HasColumnName("TicketCost");
            this.Property(x => x.Speed).HasColumnName("Speed");
        }
    }
}
