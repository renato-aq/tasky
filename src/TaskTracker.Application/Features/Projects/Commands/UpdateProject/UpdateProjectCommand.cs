using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Projects.DTOs;

namespace TaskTracker.Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(Guid ProjectId, string Name, string? Description) : ICommand<ProjectDto>;
