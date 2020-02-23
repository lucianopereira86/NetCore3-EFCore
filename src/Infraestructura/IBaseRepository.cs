using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infraestructura
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> List(Expression<Func<T, bool>> expression);
        bool Any(Expression<Func<T, bool>> expression);
        T Save(T entity);
        T Update(T entity);
        void Delete(T entity);
    }
}
