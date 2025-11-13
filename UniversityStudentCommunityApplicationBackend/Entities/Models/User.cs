using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}
