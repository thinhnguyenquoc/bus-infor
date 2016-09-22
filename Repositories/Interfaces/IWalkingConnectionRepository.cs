using Bus_Info.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Repositories.Interfaces
{
    public interface IWalkingConnectionRepository : IDisposable
    {
        IQueryable<WalkingConnection> All { get; }
        IQueryable<WalkingConnection> AllIncluding(params Expression<Func<WalkingConnection, object>>[] includeProperties);
        WalkingConnection Find(int id);
        void InsertOrUpdate(WalkingConnection WalkingConnection);
        void Delete(int id);
        void Save();
    }
}
