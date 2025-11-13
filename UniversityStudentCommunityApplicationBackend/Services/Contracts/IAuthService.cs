using Entities.DTOs;

namespace Services.Contracts
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RefreshAsync(RefreshRequestDto dto);
        Task LogoutAsync(RefreshRequestDto dto); // belirli refresh token'ı revoke
        Task AssignRoleAsync(string userId, string newRole);
    }
}
