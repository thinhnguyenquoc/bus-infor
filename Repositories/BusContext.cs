using Bus_Info.Entities;
using Bus_Info.Entities.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Repositories
{
    public class BusContext: DbContext
    {
        static BusContext()
        {
            Database.SetInitializer<BusContext>(null);
            Type _Hack = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

        public BusContext()
            : base("Name=BusConnectionString")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Route> Routes { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<NewConnection> NewConnections { get; set; }
        public DbSet<NewStation> NewStations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<WalkingConnection> WalkingConnections { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RouteMap());
            modelBuilder.Configurations.Add(new StationMap());
            modelBuilder.Configurations.Add(new ConnectionMap());
            modelBuilder.Configurations.Add(new NewStationMap());
            modelBuilder.Configurations.Add(new NewConnectionMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new WalkingConnectionMap());
        }
    }
}
