using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;

namespace TaskTracker.Application.Features.Teams.Commands.RemoveTeamMember;

public class RemoveTeamMemberCommandHandler : ICommandHandler<RemoveTeamMemberCommand>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveTeamMemberCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(RemoveTeamMemberCommand command, CancellationToken ct = default)
    {
        var team = await _teamRepository.GetByIdWithMembersAsync(command.TeamId, ct)
            ?? throw new InvalidOperationException($"Team '{command.TeamId}' not found.");

        team.RemoveMember(command.UserId);
        await _unitOfWork.CommitAsync(ct);
    }
}
