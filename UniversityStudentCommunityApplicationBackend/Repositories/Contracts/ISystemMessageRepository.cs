using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface ISystemMessageRepository
    {
        // Okuma (Read) Metodları
        Task<SystemMessage?> GetSystemMessageByCodeAsync(string code, bool trackChanges);
        Task<SystemMessage?> GetSystemMessageByIdAsync(int id, bool trackChanges);
        Task<IEnumerable<SystemMessage>> GetAllSystemMessagesAsync(bool trackChanges);

        // Yazma (Write) Metodları
        Task CreateSystemMessageAsync(SystemMessage message);
        Task UpdateSystemMessageAsync(SystemMessage message); // Güncellemeyi yapıp kaydedecek
        Task DeleteSystemMessageAsync(SystemMessage message); // Silmeyi yapıp kaydedecek
    }
}
