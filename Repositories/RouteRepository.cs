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
    public class RouteRepository : IRouteRepository
    {
        BusContext context = new BusContext();

        public IQueryable<Route> All
        {
            get { return context.Routes; }
        }

        public IQueryable<Route> AllIncluding(params Expression<Func<Route, object>>[] includeProperties)
        {
            IQueryable<Route> query = context.Routes;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Route Find(int id)
        {
            return context.Routes.Find(id);
        }

        public void InsertOrUpdate(Route route)
        {
            if (route.Id == default(int))
            {
                // New entity
                context.Routes.Add(route);
            }
            else
            {
                // Existing entity
                context.Entry(route).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var route = context.Routes.Find(id);
            context.Routes.Remove(route);
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
