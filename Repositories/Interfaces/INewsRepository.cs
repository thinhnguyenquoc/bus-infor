using Bus_Info.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Repositories.Interfaces
{
    public interface INewsRepository : IDisposable
    {
        IQueryable<News> All { get; }
        IQueryable<News> AllIncluding(params Expression<Func<News, object>>[] includeProperties);
        News Find(int id);
        void InsertOrUpdate(News News);
        void Delete(int id);
        void Save();
    }
}
