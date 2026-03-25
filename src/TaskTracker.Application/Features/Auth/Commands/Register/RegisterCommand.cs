using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Auth.DTOs;

namespace TaskTracker.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Name,
    string Email,
    string Password) : ICommand<AuthResponse>;
