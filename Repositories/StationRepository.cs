using Bus_Info.Entities;
using Bus_Info.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Repositories
{
    public class StationRepository : IStationRepository
    {
        BusContext context = new BusContext();

        public IQueryable<Station> All
        {
            get { return context.Stations; }
        }

        public IQueryable<Station> AllIncluding(params Expression<Func<Station, object>>[] includeProperties)
        {
            IQueryable<Station> query = context.Stations;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Station Find(int id)
        {
            return context.Stations.Find(id);
        }

        public void InsertOrUpdate(Station station)
        {
            if (station.Id == default(int))
            {
                // New entity
                context.Stations.Add(station);
            }
            else
            {
                // Existing entity
                context.Entry(station).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var station = context.Stations.Find(id);
            context.Stations.Remove(station);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
