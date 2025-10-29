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
        private readonly ISystemMessageService _service;

        public SystemMessageController(ISystemMessageService service)
        {
            _service = service;
        }
        [HttpGet("{code}")]
        public async Task<IActionResult> GetSystemMessageByCode([FromRoute] string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Code parameter is required.");

            var message = await _service.GetSystemMessageByCode(code);

            if (message is null)
                return NotFound($"Message with code '{code}' not found.");

            return Ok(message.Message);
        }
    }
}
