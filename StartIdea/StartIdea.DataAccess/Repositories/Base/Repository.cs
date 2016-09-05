using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace StartIdea.DataAccess.Repositories.Base
{
    public class Repository<T> : IDisposable, IRepository<T> where T : class
    {
        private readonly StartIdeaDBContext _dbContext;

        public Repository()
        {
            _dbContext = new StartIdeaDBContext();
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IEnumerable FindBy(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).AsEnumerable();
        }

        public IEnumerable GetAll()
        {
            return _dbContext.Set<T>().AsEnumerable();
        }

        public T GetById(object id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}