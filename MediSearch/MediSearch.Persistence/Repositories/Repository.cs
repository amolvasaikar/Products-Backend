using MediSearch.Persistence.Context;
using MediSearch.Persistence.IRepositories;
using System.Linq.Expressions;

namespace MediSearch.Persistence.Repositories
{
    public abstract class Repository<T> : IGenericRepository<T> where T : class
    {

        private string _errorMessage = string.Empty;
        private bool _isDisposed;
        //While Creating an Instance of GenericRepository, we need to pass the UnitOfWork instance
        //That UnitOfWork instance contains the Context Object that our GenericRepository is going to use
        public Repository(IUnitOfWork<ApplicationDbContext> unitOfWork)
            : this(unitOfWork.Context)
        {
        }
        //If you don't want to use Unit of Work, then use the following Constructor 
        //which takes the context Object as a parameter
        public Repository(ApplicationDbContext context)
        {
            //Initialize _isDisposed to false and then set the Context Object
            _isDisposed = false;
            Context = context;
        }
        //The following Property is going to return the Context Object
        public ApplicationDbContext Context { get; set; }

        //The following Property is going to set and return the Entity
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
