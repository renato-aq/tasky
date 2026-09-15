using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Projects.DTOs;

namespace TaskTracker.Application.Features.Projects.Queries.GetProjectById;

public record GetProjectByIdQuery(Guid ProjectId) : IQuery<ProjectDto?>;
