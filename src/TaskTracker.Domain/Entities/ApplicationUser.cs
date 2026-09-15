using Microsoft.AspNetCore.Identity;

namespace TaskTracker.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid? OrganizationId { get; set; }

    public Organization? Organization { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
