using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories.EFCore
{
    public class SystemMessageRepository : ISystemMessageRepository
    {
        private readonly RepositoryContext _context;

        public SystemMessageRepository(RepositoryContext context)
        {
            _context = context;
        }

        // --- OKUMA METODLARI ---

        public async Task<SystemMessage?> GetSystemMessageByCodeAsync(string code, bool trackChanges) =>
            !trackChanges ?
              await _context.SystemMessages.AsNoTracking().FirstOrDefaultAsync(m => m.Code.Equals(code)) :
              await _context.SystemMessages.FirstOrDefaultAsync(m => m.Code.Equals(code));

        public async Task<SystemMessage?> GetSystemMessageByIdAsync(int id, bool trackChanges) =>
            !trackChanges ?
              await _context.SystemMessages.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id) :
              await _context.SystemMessages.FirstOrDefaultAsync(m => m.Id == id);

        public async Task<IEnumerable<SystemMessage>> GetAllSystemMessagesAsync(bool trackChanges) =>
            !trackChanges ?
              await _context.SystemMessages.AsNoTracking().ToListAsync() :
              await _context.SystemMessages.ToListAsync();

        // --- YAZMA METODLARI (KAYDETME İŞLEMİ BURADA) ---

        public async Task CreateSystemMessageAsync(SystemMessage message)
        {
            _context.SystemMessages.Add(message);
            await _context.SaveChangesAsync(); // Değişikliği anında kaydet
        }

        public async Task UpdateSystemMessageAsync(SystemMessage message)
        {
            // Update için önce entity'nin takip edildiğinden emin ol
            // Veya daha basitçe:
            _context.SystemMessages.Update(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSystemMessageAsync(SystemMessage message)
        {
            _context.SystemMessages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
}
