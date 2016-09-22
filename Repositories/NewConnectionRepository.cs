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
    public class NewConnectionRepository : INewConnectionRepository
    {
        BusContext context = new BusContext();

        public IQueryable<NewConnection> All
        {
            get { return context.NewConnections; }
        }

        public IQueryable<NewConnection> AllIncluding(params Expression<Func<NewConnection, object>>[] includeProperties)
        {
            IQueryable<NewConnection> query = context.NewConnections;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public NewConnection Find(int id)
        {
            return context.NewConnections.Find(id);
        }

        public void InsertOrUpdate(NewConnection newconnection)
        {
            if (newconnection.Id == default(int))
            {
                // New entity
                context.NewConnections.Add(newconnection);
            }
            else
            {
                // Existing entity
                context.Entry(newconnection).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var newconnection = context.NewConnections.Find(id);
            context.NewConnections.Remove(newconnection);
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
