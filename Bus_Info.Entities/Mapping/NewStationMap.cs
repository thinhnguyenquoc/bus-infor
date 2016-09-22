using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Entities.Mapping
{
    public class NewStationMap : EntityTypeConfiguration<NewStation>
    {
        public NewStationMap()
        {
            this.ToTable("NewStations");
            this.HasKey(x => x.Id);           
            this.Property(x => x.OldId).HasColumnName("OldId");
            this.Property(x => x.OutVertex).HasColumnName("OutVertex");
        }
    }
}
