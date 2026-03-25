using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Auth.DTOs;

namespace TaskTracker.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken) : ICommand<AuthResponse>;
