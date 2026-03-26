using TaskTracker.Application.Abstractions.CQRS;

namespace TaskTracker.Application.Features.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(Guid ProjectId) : ICommand;
