using Bus_Info.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Repositories.Interfaces
{
    public interface IRouteRepository: IDisposable
    {
        IQueryable<Route> All { get; }
        IQueryable<Route> AllIncluding(params Expression<Func<Route, object>>[] includeProperties);
        Route Find(int id);
        void InsertOrUpdate(Route route);
        void Delete(int id);
        void Save();
    }
}
