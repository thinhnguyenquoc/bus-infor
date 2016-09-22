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
    public class NewsRepository : INewsRepository
    {
        BusContext context = new BusContext();

        public IQueryable<News> All
        {
            get { return context.News; }
        }

        public IQueryable<News> AllIncluding(params Expression<Func<News, object>>[] includeProperties)
        {
            IQueryable<News> query = context.News;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public News Find(int id)
        {
            return context.News.Find(id);
        }

        public void InsertOrUpdate(News News)
        {
            if (News.Id == default(int))
            {
                // New entity
                context.News.Add(News);
            }
            else
            {
                // Existing entity
                context.Entry(News).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var News = context.News.Find(id);
            context.News.Remove(News);
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
