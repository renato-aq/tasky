using Microsoft.AspNetCore.Identity;
using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;
using TaskTracker.Application.Features.Auth.DTOs;
using TaskTracker.Domain.Entities;
using RefreshTokenEntity = TaskTracker.Domain.Entities.RefreshToken;

namespace TaskTracker.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, AuthResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> HandleAsync(RegisterCommand command, CancellationToken ct = default)
    {
        var existingUser = await _userManager.FindByEmailAsync(command.Email);
        if (existingUser is not null)
            throw new InvalidOperationException($"Email '{command.Email}' is already registered.");

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Email = command.Email,
            UserName = command.Email,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create user: {errors}");
        }

        var accessToken = _tokenService.GenerateAccessToken(user);
        var rawRefreshToken = _tokenService.GenerateRefreshToken();
        var refreshToken = RefreshTokenEntity.Create(user.Id, rawRefreshToken, DateTime.UtcNow.AddDays(30));

        await _userRepository.AddRefreshTokenAsync(refreshToken, ct);
        await _unitOfWork.CommitAsync(ct);

        return new AuthResponse(
            accessToken,
            rawRefreshToken,
            DateTime.UtcNow.AddHours(1),
            new UserDto(user.Id, user.Name, user.Email!));
    }
}
