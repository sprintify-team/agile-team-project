using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemMessageController : ControllerBase
    {
        private readonly ISystemMessageService _systemMessageService;

        public SystemMessageController(ISystemMessageService systemMessageService)
        {
            _systemMessageService = systemMessageService;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetSystemMessageByCode([FromRoute] string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Code parameter is required.");

            var message = await _systemMessageService.GetMessageByCodeAsync(code);

            if (message is null)
                return NotFound($"Message with code '{code}' not found.");

            return Ok(message.Message);
        }
    }
}
