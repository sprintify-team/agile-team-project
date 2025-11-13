using Entities.DTOs;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.EFCore;
using Services.Contracts;
using System.Security.Cryptography;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(
            UserManager<User> userManager,
            JwtTokenService jwtService,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IConfiguration config,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _config = config;
            _roleManager = roleManager;
        }

        // -------------------
        // Helpers
        // -------------------
        private static string GenerateSecureToken(int bytes = 64)
        {
            var buffer = RandomNumberGenerator.GetBytes(bytes);
            return Convert.ToBase64String(buffer);
        }

        private int GetRefreshDays()
        {
            var s = _config["Jwt:RefreshTokenDays"];
            return int.TryParse(s, out var d) ? d : 7;
        }

        private async Task<AuthResponseDto> BuildResponse(User user)
        {
            var (accessToken, accessExp) = await _jwtService.GenerateTokenAsync(user);

            var refresh = new RefreshToken
            {
                Token = GenerateSecureToken(),
                Expires = DateTime.UtcNow.AddDays(GetRefreshDays()),
                IsRevoked = false,
                UserId = user.Id
            };

            await _refreshTokenRepository.AddAsync(refresh);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto
            {
                Token = accessToken,
                Expiration = accessExp,
                RefreshToken = refresh.Token,
                RefreshTokenExpiration = refresh.Expires
            };
        }

        // -------------------
        // Public API
        // -------------------
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // --- Duplicate kontrolü ---
            var existingByEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingByEmail != null)
                throw new ArgumentException("This email address is already registered.");

            var existingByUserName = await _userManager.FindByNameAsync(dto.UserName);
            if (existingByUserName != null)
                throw new ArgumentException("This username is already taken.");

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new ArgumentException(string.Join("; ", result.Errors.Select(e => e.Description)));

            var defaultRole = "Student";
            if (!await _roleManager.RoleExistsAsync(defaultRole))
                await _roleManager.CreateAsync(new IdentityRole(defaultRole));

            var addRoleResult = await _userManager.AddToRoleAsync(user, defaultRole);
            if (!addRoleResult.Succeeded)
                throw new ArgumentException(string.Join("; ", addRoleResult.Errors.Select(e => e.Description)));

            return await BuildResponse(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // kullanıcı hem email hem username ile giriş yapabilsin
            var user = await _userManager.FindByEmailAsync(dto.UserNameOrEmail)
                       ?? await _userManager.FindByNameAsync(dto.UserNameOrEmail);

            // kullanıcı bulunamadıysa
            if (user == null)
                throw new ArgumentException("Incorrect username or password.");

            // şifre yanlışsa
            var valid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!valid)
                throw new ArgumentException("Incorrect username or password.");

            // başarılı giriş → token üret
            return await BuildResponse(user);
        }

        public async Task<AuthResponseDto> RefreshAsync(RefreshRequestDto dto)
        {
            var existing = await _refreshTokenRepository.GetByTokenAsync(dto.RefreshToken);

            if (existing is null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            if (existing.Expires <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expired");

            if (existing.IsRevoked)
                throw new UnauthorizedAccessException("Refresh token revoked");

            var user = existing.User!;
            var (accessToken, accessExp) = await _jwtService.GenerateTokenAsync(user);

            existing.IsRevoked = true;
            _refreshTokenRepository.Update(existing);

            var newRefreshToken = new RefreshToken
            {
                Token = GenerateSecureToken(),
                Expires = existing.Expires,
                IsRevoked = false,
                UserId = user.Id
            };

            await _refreshTokenRepository.AddAsync(newRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto
            {
                Token = accessToken,
                Expiration = accessExp,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.Expires
            };
        }

        public async Task LogoutAsync(RefreshRequestDto dto)
        {
            var existing = await _refreshTokenRepository.GetByTokenAsync(dto.RefreshToken);
            if (existing is null) return;

            existing.IsRevoked = true;
            _refreshTokenRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssignRoleAsync(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User not found");

            var validRoles = new List<string> { "Admin", "ClubManager", "Student" };
            if (!validRoles.Contains(newRole))
                throw new ArgumentException("Invalid role");

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!await _roleManager.RoleExistsAsync(newRole))
                await _roleManager.CreateAsync(new IdentityRole(newRole));

            await _userManager.AddToRoleAsync(user, newRole);
        }
    }
}
