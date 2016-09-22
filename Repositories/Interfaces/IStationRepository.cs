using Bus_Info.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Repositories.Interfaces
{
    public interface IStationRepository : IDisposable
    {
        IQueryable<Station> All { get; }
        IQueryable<Station> AllIncluding(params Expression<Func<Station, object>>[] includeProperties);
        Station Find(int id);
        void InsertOrUpdate(Station Station);
        void Delete(int id);
        void Save();
    }
}
