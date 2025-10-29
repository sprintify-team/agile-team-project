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

        // Sadece Context'e ekler, kaydetmez.
        void CreateSystemMessage(SystemMessage message);

        // Sadece Context'te günceller, kaydetmez.
        void UpdateSystemMessage(SystemMessage message);

        // Sadece Context'ten silmek için işaretler, kaydetmez.
        void DeleteSystemMessage(SystemMessage message);
    }
}
