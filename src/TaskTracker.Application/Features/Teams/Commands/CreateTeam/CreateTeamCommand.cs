using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Teams.DTOs;

namespace TaskTracker.Application.Features.Teams.Commands.CreateTeam;

public record CreateTeamCommand(Guid OrganizationId, string Name) : ICommand<TeamDto>;
