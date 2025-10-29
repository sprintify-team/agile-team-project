using Repositories.Contracts;

namespace Repositories.EFCore
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly RepositoryContext _context;

        public UnitOfWork(RepositoryContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
