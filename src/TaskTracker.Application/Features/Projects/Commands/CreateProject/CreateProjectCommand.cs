using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Projects.DTOs;

namespace TaskTracker.Application.Features.Projects.Commands.CreateProject;

public record CreateProjectCommand(Guid TeamId, string Name, string? Description) : ICommand<ProjectDto>;
