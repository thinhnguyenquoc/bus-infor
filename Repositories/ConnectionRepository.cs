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
    public class ConnectionRepository : IConnectionRepository
    {
        BusContext context = new BusContext();

        public IQueryable<Connection> All
        {
            get { return context.Connections; }
        }

        public IQueryable<Connection> AllIncluding(params Expression<Func<Connection, object>>[] includeProperties)
        {
            IQueryable<Connection> query = context.Connections;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Connection Find(int id)
        {
            return context.Connections.Find(id);
        }

        public void InsertOrUpdate(Connection connection)
        {
            if (connection.Id == default(int))
            {
                // New entity
                context.Connections.Add(connection);
            }
            else
            {
                // Existing entity
                context.Entry(connection).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var connection = context.Connections.Find(id);
            context.Connections.Remove(connection);
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
