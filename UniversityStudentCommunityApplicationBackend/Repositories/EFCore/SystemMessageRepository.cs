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

        // --- OKUMA METODLARI 
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


        // --- YAZMA METODLARI 

        public void CreateSystemMessage(SystemMessage message)
        {
            _context.SystemMessages.Add(message);
        }

        public void UpdateSystemMessage(SystemMessage message)
        {
            _context.SystemMessages.Update(message);
        }

        public void DeleteSystemMessage(SystemMessage message)
        {
            _context.SystemMessages.Remove(message);
        }
    }
}
