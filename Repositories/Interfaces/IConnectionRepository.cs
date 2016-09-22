using Bus_Info.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Repositories.Interfaces
{
    public interface IConnectionRepository : IDisposable
    {
        IQueryable<Connection> All { get; }
        IQueryable<Connection> AllIncluding(params Expression<Func<Connection, object>>[] includeProperties);
        Connection Find(int id);
        void InsertOrUpdate(Connection Connection);
        void Delete(int id);
        void Save();
    }
}
