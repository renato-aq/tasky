using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Abstractions.Services;

public interface ITokenService
{
    string GenerateAccessToken(ApplicationUser user);
    string GenerateRefreshToken();
    Guid? GetUserIdFromExpiredToken(string token);
}
