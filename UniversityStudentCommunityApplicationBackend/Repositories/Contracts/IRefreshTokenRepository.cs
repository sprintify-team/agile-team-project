using Entities.Models;

namespace Repositories.Contracts
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        void Update(RefreshToken refreshToken);
        Task<RefreshToken?> GetByTokenAsync(string token);
    }
}
