using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories.EFCore
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly RepositoryContext _context;

        public RefreshTokenRepository(RepositoryContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
        }

        public void Update(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(r => r.User) // Refresh sırasında User bilgisine erişmek için
                .FirstOrDefaultAsync(r => r.Token == token);
        }
    }
}
