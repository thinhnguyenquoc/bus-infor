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
    public class WalkingConnectionRepository : IWalkingConnectionRepository
    {
        BusContext context = new BusContext();

        public IQueryable<WalkingConnection> All
        {
            get { return context.WalkingConnections; }
        }

        public IQueryable<WalkingConnection> AllIncluding(params Expression<Func<WalkingConnection, object>>[] includeProperties)
        {
            IQueryable<WalkingConnection> query = context.WalkingConnections;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public WalkingConnection Find(int id)
        {
            return context.WalkingConnections.Find(id);
        }

        public void InsertOrUpdate(WalkingConnection walkingconnection)
        {
            if (walkingconnection.Id == default(int))
            {
                // New entity
                context.WalkingConnections.Add(walkingconnection);
            }
            else
            {
                // Existing entity
                context.Entry(walkingconnection).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var walkingconnection = context.WalkingConnections.Find(id);
            context.WalkingConnections.Remove(walkingconnection);
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
