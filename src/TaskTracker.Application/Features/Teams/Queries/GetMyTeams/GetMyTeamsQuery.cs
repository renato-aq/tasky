using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Teams.DTOs;

namespace TaskTracker.Application.Features.Teams.Queries.GetMyTeams;

public record GetMyTeamsQuery(Guid UserId) : IQuery<IEnumerable<TeamDto>>;
