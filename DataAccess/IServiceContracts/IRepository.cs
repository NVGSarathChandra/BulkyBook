using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.IServiceContracts
{
   public    interface IRepository<T> where T: class      //IRepository<T> T means type of repository and it will be a class
    {
        T Get(int id);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy=null,
            string includeProperties=null
            );

        T GetFirstOrDefault(
            Expression<Func<T, bool>> expression = null,
            string includeProperties = null
            );

        void Add(T entity);

        void Remove(int id);

        void RemoveEntity(T entity);

        void RemoveRange(IEnumerable<T> entity);
    }
}
