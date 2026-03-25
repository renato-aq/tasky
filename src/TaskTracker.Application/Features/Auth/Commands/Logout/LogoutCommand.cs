using TaskTracker.Application.Abstractions.CQRS;

namespace TaskTracker.Application.Features.Auth.Commands.Logout;

public record LogoutCommand(string RefreshToken) : ICommand;
