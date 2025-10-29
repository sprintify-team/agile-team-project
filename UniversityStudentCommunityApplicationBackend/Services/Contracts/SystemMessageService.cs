using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Contracts;

namespace Services.Contracts
{
    public class SystemMessageService : ISystemMessageService
    {
        private readonly ISystemMessageRepository _repository;
        public SystemMessageService(ISystemMessageRepository repository)
        {
            _repository = repository;
        }


        public async Task<SystemMessage?> GetSystemMessageByCode(string code)
        {
            return await _repository.GetSystemMessageByCodeAsync(code, trackChanges: false);
        }
    }
}
