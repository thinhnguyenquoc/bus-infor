using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        IQueryable<T> All();
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);
        IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);
        bool Contains(Expression<Func<T, bool>> predicate);
        T Find(params object[] keys);
        T Find(Expression<Func<T, bool>> predicate);
        T Create(T t);
        int Update(T t);
        int Delete(Expression<Func<T, bool>> predicate);
        void Save();
    }
}
