using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Entities.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("Users");
            this.HasKey(x => x.Id);
            this.Property(x => x.Email).HasColumnName("Email");
            this.Property(x => x.Pass).HasColumnName("Pass");
            this.Property(x => x.CreatedDate).HasColumnName("CreatedDate");
        }
    }
}
