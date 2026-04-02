using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Queries.GetSprintById;

public record GetSprintByIdQuery(Guid SprintId) : IQuery<SprintDto?>;
