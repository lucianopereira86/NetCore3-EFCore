using Microsoft.EntityFrameworkCore;
using NetCore3WebAPI.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NetCore3WebAPI.Infra.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private DbSet<T> entity_;
        protected DBContext context;

        public BaseRepository(DBContext context)
        {
            this.context = context;
            entity_ = context.Set<T>();
        }

        public virtual IEnumerable<T> List(Expression<Func<T, bool>> expression)
        {
            return entity_.Where(expression).ToList();
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return entity_.Any(expression);
        }

        public T Save(T entity)
        {
            entity_.Add(entity);
            context.SaveChanges();

            return entity;
        }

        public T Update(T entity)
        {
            entity_.Update(entity);
            context.SaveChanges();
            return entity;
        }

        public void Delete(T entity)
        {
            entity_.Remove(entity);
            context.SaveChanges();
        }
    }
}
