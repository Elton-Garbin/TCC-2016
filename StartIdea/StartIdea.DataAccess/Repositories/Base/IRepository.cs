using System;
using System.Collections;
using System.Linq.Expressions;

namespace StartIdea.DataAccess.Repositories.Base
{
    interface IRepository<T> where T : class
    {
        IEnumerable GetAll();
        T GetById(object id);
        IEnumerable FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
