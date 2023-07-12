using MediSearch.Persistence.IRepositories;
using System.Linq.Expressions;

namespace MediSearch.Persistence.Repositories
{
    public abstract class Repository<T> : IGenericRepository<T> where T : class
    {
        public IQueryable<T> Queryable => throw new NotImplementedException();

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(T item)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public bool Any()
        {
            throw new NotImplementedException();
        }

        public bool Any(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public long Count()
        {
            throw new NotImplementedException();
        }

        public long Count(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public void Delete(object key)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object key)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public T Get(object key)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(object key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> List()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(T item)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T item)
        {
            throw new NotImplementedException();
        }

        public void UpdatePartial(object item)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePartialAsync(object item)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }
    }

}
