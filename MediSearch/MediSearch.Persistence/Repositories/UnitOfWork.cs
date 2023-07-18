using MediSearch.Persistence.Context;
using MediSearch.Persistence.IRepositories;

namespace MediSearch.Persistence.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        ApplicationDbContext applicationDbContext;
        public UnitOfWork(ApplicationDbContext context)
        {
            applicationDbContext = context;
        }

        public async Task<int> SaveChangesAsync()
        {

            return await applicationDbContext.SaveChangesAsync();
        }
    }
}
