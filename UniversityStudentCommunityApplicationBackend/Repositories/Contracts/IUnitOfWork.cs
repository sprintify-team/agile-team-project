namespace Repositories.Contracts
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
