using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;

        public JwtTokenService(IConfiguration config, UserManager<User> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<(string token, DateTime expiration)> GenerateTokenAsync(User user)
        {

            var jwtSection = _config.GetSection("Jwt");

            var claims = new List<Claim>
            {
                new Claim("sub", user.Id),
                new Claim("userName", user.UserName ?? user.Email ?? ""),
                new Claim("email", user.Email ?? ""),
                new Claim("name", $"{user.FirstName} {user.LastName}")
            };

            // Role claims'lerini ekle
            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(role => new Claim("role", role)));
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:AccessTokenMinutes"]!));

            // Token oluştur
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}
