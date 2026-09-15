using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Teams.Commands.AddTeamMember;

public record AddTeamMemberCommand(Guid TeamId, Guid UserId, TeamRole Role) : ICommand;
