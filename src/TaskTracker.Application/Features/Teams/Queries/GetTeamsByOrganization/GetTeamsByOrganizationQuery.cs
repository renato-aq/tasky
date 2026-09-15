using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Teams.DTOs;

namespace TaskTracker.Application.Features.Teams.Queries.GetTeamsByOrganization;

public record GetTeamsByOrganizationQuery(Guid OrganizationId) : IQuery<IEnumerable<TeamDto>>;
