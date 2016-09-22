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
    public class UserRepository : IUserRepository
    {
        BusContext context = new BusContext();

        public IQueryable<User> All
        {
            get { return context.Users; }
        }

        public IQueryable<User> AllIncluding(params Expression<Func<User, object>>[] includeProperties)
        {
            IQueryable<User> query = context.Users;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public User Find(int id)
        {
            return context.Users.Find(id);
        }

        public void InsertOrUpdate(User User)
        {
            if (User.Id == default(int))
            {
                // New entity
                context.Users.Add(User);
            }
            else
            {
                // Existing entity
                context.Entry(User).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var User = context.Users.Find(id);
            context.Users.Remove(User);
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
