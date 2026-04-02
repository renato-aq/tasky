using TaskTracker.Application.Abstractions.CQRS;

namespace TaskTracker.Application.Features.Teams.Commands.RemoveTeamMember;

public record RemoveTeamMemberCommand(Guid TeamId, Guid UserId) : ICommand;
