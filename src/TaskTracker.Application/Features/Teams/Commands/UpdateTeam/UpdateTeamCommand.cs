using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Teams.DTOs;

namespace TaskTracker.Application.Features.Teams.Commands.UpdateTeam;

public record UpdateTeamCommand(Guid TeamId, string Name) : ICommand<TeamDto>;
