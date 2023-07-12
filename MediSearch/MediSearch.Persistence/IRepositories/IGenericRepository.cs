namespace MediSearch.Persistence.IRepositories
{
    public interface IGenericRepository<T> : ICommandRepository<T>, IQueryRepository<T> where T : class
    {

    }
}
