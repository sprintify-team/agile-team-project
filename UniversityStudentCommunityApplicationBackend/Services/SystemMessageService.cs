using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class SystemMessageService : ISystemMessageService
    {
        // Gerekli bağımlılıklar: Repository ve UnitOfWork
        private readonly ISystemMessageRepository _systemMessageRepository;
        private readonly IUnitOfWork _unitOfWork;

        // Constructor ile bağımlılıkları enjekte et (DI)
        public SystemMessageService(
            ISystemMessageRepository systemMessageRepository,
            IUnitOfWork unitOfWork)
        {
            _systemMessageRepository = systemMessageRepository;
            _unitOfWork = unitOfWork;
        }

        // --- YAZMA METODLARI (İŞ MANTIĞI VE KAYDETME BURADA) ---

        public async Task CreateMessageAsync(SystemMessage message)
        {
            // 1. İş Kuralı (Business Logic)
            if (message is null)
                throw new ArgumentNullException(nameof(message)); // veya kendi özel hatan

            if (string.IsNullOrWhiteSpace(message.Code))
                throw new ArgumentException("Code alanı boş olamaz.", nameof(message.Code));

            // 2. Repository'ye "Ekle" komutu ver (daha kaydetmedi)
            _systemMessageRepository.CreateSystemMessage(message);

            // 3. Değişiklikleri kaydet (Transaction'ı onayla)
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateMessageAsync(SystemMessage message)
        {
            // 1. Güncellenecek kaydı bul (takip et - trackChanges: true)
            var entityToUpdate = await _systemMessageRepository.GetSystemMessageByIdAsync(message.Id, true);

            if (entityToUpdate is null)
                throw new Exception($"ID: {message.Id} ile mesaj bulunamadı."); // Kendi NotFound hatan

            // 2. Alanları güncelle
            entityToUpdate.Code = message.Code;
            entityToUpdate.Message = message.Message;

            // 3. Repository katmanında Update metodu olmasına bile gerek yok, 
            //    çünkü EF Core değişikliği zaten takip ediyor (trackChanges: true).
            // _messageRepo.UpdateSystemMessage(entityToUpdate); // Bu satır opsiyonel

            // 4. Değişiklikleri kaydet
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteMessageAsync(int id)
        {
            // 1. Silinecek kaydı bul (takip etmeye gerek yok)
            var entityToDelete = await _systemMessageRepository.GetSystemMessageByIdAsync(id, false);

            if (entityToDelete is null)
                throw new Exception($"ID: {id} ile mesaj bulunamadı.");

            // 2. Repository'ye "Sil" komutu ver
            _systemMessageRepository.DeleteSystemMessage(entityToDelete);

            // 3. Değişikliği kaydet
            await _unitOfWork.SaveChangesAsync();
        }


        // --- OKUMA METODLARI (SADECE VERİ ÇEKER) ---

        public async Task<SystemMessage?> GetMessageByCodeAsync(string code)
        {
            // Sadece okuma, trackChanges: false
            return await _systemMessageRepository.GetSystemMessageByCodeAsync(code, false);
        }

        public async Task<SystemMessage?> GetMessageByIdAsync(int id)
        {
            return await _systemMessageRepository.GetSystemMessageByIdAsync(id, false);
        }

        public async Task<IEnumerable<SystemMessage>> GetAllMessagesAsync()
        {
            return await _systemMessageRepository.GetAllSystemMessagesAsync(false);
        }
    }
}
