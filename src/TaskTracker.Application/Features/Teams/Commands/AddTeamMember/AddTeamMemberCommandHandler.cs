using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;

namespace TaskTracker.Application.Features.Teams.Commands.AddTeamMember;

public class AddTeamMemberCommandHandler : ICommandHandler<AddTeamMemberCommand>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTeamMemberCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(AddTeamMemberCommand command, CancellationToken ct = default)
    {
        var team = await _teamRepository.GetByIdWithMembersAsync(command.TeamId, ct)
            ?? throw new InvalidOperationException($"Team '{command.TeamId}' not found.");

        team.AddMember(command.UserId, command.Role);
        await _unitOfWork.CommitAsync(ct);
    }
}
