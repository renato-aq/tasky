using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Queries.GetSprintsByProject;

public record GetSprintsByProjectQuery(Guid ProjectId) : IQuery<IEnumerable<SprintDto>>;
