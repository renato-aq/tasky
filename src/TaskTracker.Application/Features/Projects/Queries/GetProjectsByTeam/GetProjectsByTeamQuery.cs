using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Projects.DTOs;

namespace TaskTracker.Application.Features.Projects.Queries.GetProjectsByTeam;

public record GetProjectsByTeamQuery(Guid TeamId) : IQuery<IEnumerable<ProjectDto>>;
