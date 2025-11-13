using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
            => Ok(await _service.RegisterAsync(dto));

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
            => Ok(await _service.LoginAsync(dto));

        [HttpPost("refresh")]
        [AllowAnonymous] // access token süresi geçmiş olabilir; refresh token ile yenilenir
        public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshRequestDto dto)
            => Ok(await _service.RefreshAsync(dto));

        [HttpPost("logout")]
        [Authorize] // kullanıcı login ise, elindeki (body'de gelen) refresh token'ı revoke eder
        public async Task<ActionResult> Logout([FromBody] RefreshRequestDto dto)
        {
            await _service.LogoutAsync(dto);
            return NoContent();
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")] // Sadece Admin role verebilir
        public async Task<ActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            try
            {
                await _service.AssignRoleAsync(dto.UserId, dto.NewRole);
                return Ok(new { message = "Role başarıyla atandı" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
