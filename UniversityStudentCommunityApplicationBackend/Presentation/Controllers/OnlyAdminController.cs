using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OnlyAdminController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly UserManager<User> _userManager;

        public OnlyAdminController(IAuthService service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }


        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            // 1️⃣ Önce users'ı ve refresh token'larını birlikte çekelim
            var users = await _userManager.Users
                .Include(u => u.RefreshTokens) // navigation varsa
                .ToListAsync();

            // 2️⃣ Sonra her user'ın rollerini ayrı çekelim (UserManager API)
            var result = new List<object>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.CreatedAt,
                    Roles = roles,
                    RefreshTokens = user.RefreshTokens?.Select(rt => new
                    {
                        rt.Token,
                        rt.Expires,
                        rt.IsRevoked
                    })
                });
            }

            return Ok(result);
        }

    }
}
