using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Entities.Mapping
{
    public class StationMap : EntityTypeConfiguration<Station>
    {
        public StationMap()
        {
            this.ToTable("Stations");
            this.HasKey(x => x.Id);
            this.Property(x => x.Code).HasColumnName("Code");
            this.Property(x => x.Name).HasColumnName("Name");
            this.Property(x => x.Lat).HasColumnName("Lat");
            this.Property(x => x.Lng).HasColumnName("Lng");
            this.Property(x => x.Address).HasColumnName("Address");
            this.Property(x => x.Note).HasColumnName("Note");
        }
    }
}
