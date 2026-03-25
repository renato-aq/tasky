using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Auth.DTOs;

namespace TaskTracker.Application.Features.Auth.Commands.Login;

public record LoginCommand(
    string Email,
    string Password) : ICommand<AuthResponse>;
