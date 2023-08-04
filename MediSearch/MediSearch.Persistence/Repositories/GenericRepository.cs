using MediSearch.Persistence.Context;
using MediSearch.Persistence.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MediSearch.Persistence.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private string _errorMessage = string.Empty;
        private bool _isDisposed;
        private DbSet<T> _entities;

        //While Creating an Instance of GenericRepository, we need to pass the UnitOfWork instance
        //That UnitOfWork instance contains the Context Object that our GenericRepository is going to use
        public GenericRepository(IUnitOfWork<ApplicationDbContext> unitOfWork)
            : this(unitOfWork.Context)
        {
        }
        //If you don't want to use Unit of Work, then use the following Constructor 
        //which takes the context Object as a parameter
        public GenericRepository(ApplicationDbContext context)
        {
            //Initialize _isDisposed to false and then set the Context Object
            _isDisposed = false;
            Context = context;
        }
        //The following Property is going to return the Context Object
        public ApplicationDbContext Context { get; set; }

        //The following Property is going to set and return the Entity
        public IQueryable<T> Queryable => throw new NotImplementedException();

        protected virtual DbSet<T> Entities
        {
            get { return _entities ?? (_entities = Context.Set<T>()); }
        }

        public void Add(T item)
        {
            _entities.Add(item);
        }

        public async Task AddAsync(T item)
        {
            await _entities.AddAsync(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            _entities.AddRange(items);
        }

        public async Task AddRangeAsync(IEnumerable<T> items)
        {
            await _entities.AddRangeAsync(items);
        }

        public bool Any()
        {
            return _entities.Any();
        }

        public bool Any(Expression<Func<T, bool>> where)
        {
            return _entities.Any(where);
        }

        public async Task<bool> AnyAsync()
        {
            return await _entities.AnyAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> where)
        {
            return await _entities.AnyAsync(where);
        }

        public long Count()
        {
            return Entities.Count();
        }

        public long Count(Expression<Func<T, bool>> where)
        {
            return Entities.Count(where);
        }

        public async Task<long> CountAsync()
        {
            return await Entities.CountAsync();
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> where)
        {
            return await Entities.CountAsync(where);
        }

        public void Delete(object key)
        {
            T existing = Entities.Find(key);
            Entities.Remove(existing);
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            T existing = Entities.Find(where);
            Entities.Remove(existing);
        }

        public async Task DeleteAsync(object key)
        {
            T existing = await Entities.FindAsync(key);
            Entities.Remove(existing);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> where)
        {
            T existing = await Entities.FindAsync(where);
            Entities.Remove(existing);
        }

        public T Get(object key)
        {
            return Entities.Find(key);
        }

        public async Task<T> GetAsync(object key)
        {
            return await Entities.FindAsync(key);
        }

        public IEnumerable<T> List()
        {
            return Entities.ToList();
        }

        public async Task<IEnumerable<T>> ListAsync()
        {
            return await Entities.ToListAsync();
        }

        public void Update(T item)
        {
            Entities.Update(item);
        }

        public Task UpdateAsync(T item)
        {
            throw new NotImplementedException();
        }

        public void UpdatePartial(object item)
        {
            T existing = Entities.Find(item);
            Entities.Update(existing);
        }

        public Task UpdatePartialAsync(object item)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<T> items)
        {
            Entities.UpdateRange(items);
        }

        public Task UpdateRangeAsync(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }
    }
}
