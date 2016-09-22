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
    public class NewStationRepository : INewStationRepository
    {
        BusContext context = new BusContext();

        public IQueryable<NewStation> All
        {
            get { return context.NewStations; }
        }

        public IQueryable<NewStation> AllIncluding(params Expression<Func<NewStation, object>>[] includeProperties)
        {
            IQueryable<NewStation> query = context.NewStations;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public NewStation Find(int id)
        {
            return context.NewStations.Find(id);
        }

        public void InsertOrUpdate(NewStation newstation)
        {
            if (newstation.Id == default(int))
            {
                // New entity
                context.NewStations.Add(newstation);
            }
            else
            {
                // Existing entity
                context.Entry(newstation).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var newstation = context.NewStations.Find(id);
            context.NewStations.Remove(newstation);
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
