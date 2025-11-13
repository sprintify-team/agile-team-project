using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        [MinLength(3), MaxLength(30)]
        public string UserName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(2)]
        public string Password { get; init; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;
    }

    public class LoginDto
    {

        [Required(ErrorMessage = "Username or email is required")]
        public string UserNameOrEmail { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }

    public class RefreshRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiration { get; set; }
    }

    public class AssignRoleDto
    {
        public string UserId { get; set; } = string.Empty;
        public string NewRole { get; set; } = string.Empty;
    }
}
