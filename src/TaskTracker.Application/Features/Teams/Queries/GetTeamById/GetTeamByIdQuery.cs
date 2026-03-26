using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Teams.DTOs;

namespace TaskTracker.Application.Features.Teams.Queries.GetTeamById;

public record GetTeamByIdQuery(Guid TeamId) : IQuery<TeamDetailDto?>;
