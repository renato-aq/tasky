using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Projects.DTOs;

namespace TaskTracker.Application.Features.Projects.Queries.GetMyProjects;

public record GetMyProjectsQuery(Guid UserId) : IQuery<IEnumerable<ProjectDto>>;
