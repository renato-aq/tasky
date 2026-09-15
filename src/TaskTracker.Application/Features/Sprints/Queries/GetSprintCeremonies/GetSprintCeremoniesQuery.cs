using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Queries.GetSprintCeremonies;

public record GetSprintCeremoniesQuery(Guid SprintId) : IQuery<IEnumerable<SprintCeremonyDto>>;
