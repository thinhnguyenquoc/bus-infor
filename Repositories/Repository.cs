﻿using Bus_Info.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Repositories
{
    public class Repository<TObject> : IRepository<TObject> where TObject : class
    {
        protected BusContext Context;
        private bool shareContext = false;
        public Repository()
        {
            Context = new BusContext();
        }

        public Repository(BusContext context)
        {
            Context = context;
            shareContext = true;
        }
        protected DbSet<TObject> DbSet
        {
            get
            {
                return Context.Set<TObject>();
            }
        }
        public void Dispose()
        {
            if (shareContext && (Context != null))
                Context.Dispose();
        }
        public virtual IQueryable<TObject> All()
        {
            return DbSet.AsQueryable();
        }
        public IQueryable<TObject> AllIncluding(params Expression<Func<TObject, object>>[] includeProperties)
        {
            IQueryable<TObject> query = DbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable<TObject>();
        }
        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var _resetSet = filter != null ? DbSet.Where(filter).AsQueryable() :
                DbSet.AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) :
                _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }
        public bool Contains(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }
        public virtual TObject Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }
        public virtual TObject Find(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }
        public virtual TObject Create(TObject TObject)
        {
            var newEntry = DbSet.Add(TObject);
            if (!shareContext)
                Context.SaveChanges();
            return newEntry;
        }
        public virtual int Update(TObject TObject)
        {
            var entry = Context.Entry(TObject);
            DbSet.Attach(TObject);
            entry.State = EntityState.Modified;
            if (!shareContext)
                return Context.SaveChanges();
            return 0;
        }
        public virtual int Delete(TObject TObject)
        {
            DbSet.Remove(TObject);
            if (!shareContext)
                return Context.SaveChanges();
            return 0;
        }
        public virtual int Delete(Expression<Func<TObject, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                DbSet.Remove(obj);
            if (!shareContext)
                return Context.SaveChanges();
            return 0;
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
