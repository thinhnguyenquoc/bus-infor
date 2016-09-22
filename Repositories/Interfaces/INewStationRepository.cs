using Bus_Info.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Repositories.Interfaces
{
    public interface INewStationRepository : IDisposable
    {
        IQueryable<NewStation> All { get; }
        IQueryable<NewStation> AllIncluding(params Expression<Func<NewStation, object>>[] includeProperties);
        NewStation Find(int id);
        void InsertOrUpdate(NewStation NewStation);
        void Delete(int id);
        void Save();
    }
}
