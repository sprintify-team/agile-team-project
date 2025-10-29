using Entities.Models;

namespace Services.Contracts
{
    public interface ISystemMessageService
    {
        // Okuma işlemleri
        Task<SystemMessage?> GetMessageByCodeAsync(string code);
        Task<SystemMessage?> GetMessageByIdAsync(int id);
        Task<IEnumerable<SystemMessage>> GetAllMessagesAsync();

        // Yazma işlemleri
        Task CreateMessageAsync(SystemMessage message);
        Task UpdateMessageAsync(SystemMessage message);
        Task DeleteMessageAsync(int id); // ID ile silmek daha yaygındır
    }
}
