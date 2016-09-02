using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace StartIdea.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly StartIdeaContext _dbContext;

        public Repository()
        {
            _dbContext = new StartIdeaContext();
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public IEnumerable FindBy(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
               .Where(predicate)
               .AsEnumerable();
        }

        public IEnumerable GetAll()
        {
            return _dbContext.Set<T>().AsEnumerable();
        }

        public T GetById(object id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
