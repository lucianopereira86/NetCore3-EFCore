using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infraestructura.Impl
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbSet<T> entity;
        protected CablemodemContext context;

        public BaseRepository(CablemodemContext context)
        {
            this.context = context;
            this.entity = context.Set<T>();
        }

        public virtual IEnumerable<T> Search(Expression<Func<T, bool>> expression)
        {
            return entity.Where(expression).ToList();
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return entity.Any(expression);
        }

        public T Save(T entity)
        {
            this.entity.Add(entity);
            context.SaveChanges();

            return entity;
        }

        public T Update(T entity)
        {
            this.entity.Update(entity);
            context.SaveChanges();
            return entity;
        }

        public void Delete(T entity)
        {
            this.entity.Remove(entity);
            context.SaveChanges();
        }
    }
}
