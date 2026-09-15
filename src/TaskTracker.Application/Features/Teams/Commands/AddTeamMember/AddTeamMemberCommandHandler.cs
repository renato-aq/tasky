using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;

namespace TaskTracker.Application.Features.Teams.Commands.AddTeamMember;

public class AddTeamMemberCommandHandler : ICommandHandler<AddTeamMemberCommand>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTeamMemberCommandHandler(
        ITeamRepository teamRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(AddTeamMemberCommand command, CancellationToken ct = default)
    {
        var team = await _teamRepository.GetByIdWithMembersAsync(command.TeamId, ct)
            ?? throw new KeyNotFoundException($"Team '{command.TeamId}' not found.");

        var user = await _userRepository.GetByIdAsync(command.UserId, ct)
            ?? throw new KeyNotFoundException($"User '{command.UserId}' not found.");

        team.AddMember(user.Id, command.Role);
        await _unitOfWork.CommitAsync(ct);
    }
}
