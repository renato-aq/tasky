using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;
using TaskTracker.Application.Features.Auth.DTOs;
using TaskTracker.Domain.Entities;
using RefreshTokenEntity = TaskTracker.Domain.Entities.RefreshToken;

namespace TaskTracker.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommandHandler(
        ITokenService tokenService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> HandleAsync(RefreshTokenCommand command, CancellationToken ct = default)
    {
        var userId = _tokenService.GetUserIdFromExpiredToken(command.AccessToken)
            ?? throw new UnauthorizedAccessException("Invalid access token.");

        var storedToken = await _userRepository.GetRefreshTokenAsync(command.RefreshToken, ct)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        if (!storedToken.IsValid() || storedToken.UserId != userId)
            throw new UnauthorizedAccessException("Refresh token is invalid or expired.");

        var user = await _userRepository.GetByIdAsync(userId, ct)
            ?? throw new UnauthorizedAccessException("User not found.");

        storedToken.Revoke();

        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRawRefreshToken = _tokenService.GenerateRefreshToken();
        var newRefreshToken = RefreshTokenEntity.Create(user.Id, newRawRefreshToken, DateTime.UtcNow.AddDays(30));

        await _userRepository.AddRefreshTokenAsync(newRefreshToken, ct);
        await _unitOfWork.CommitAsync(ct);

        return new AuthResponse(
            newAccessToken,
            newRawRefreshToken,
            DateTime.UtcNow.AddHours(1),
            new UserDto(user.Id, user.Name, user.Email!));
    }
}
