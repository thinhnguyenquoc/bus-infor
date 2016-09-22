using Bus_Info.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Repositories.Interfaces
{
    public interface INewConnectionRepository : IDisposable
    {
        IQueryable<NewConnection> All { get; }
        IQueryable<NewConnection> AllIncluding(params Expression<Func<NewConnection, object>>[] includeProperties);
        NewConnection Find(int id);
        void InsertOrUpdate(NewConnection NewConnection);
        void Delete(int id);
        void Save();
    }
}
