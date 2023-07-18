namespace MediSearch.Persistence.IRepositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
