using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Teams.DTOs;

namespace TaskTracker.Application.Features.Teams.Commands.UpdateTeam;

public class UpdateTeamCommandHandler : ICommandHandler<UpdateTeamCommand, TeamDto>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTeamCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TeamDto> HandleAsync(UpdateTeamCommand command, CancellationToken ct = default)
    {
        var team = await _teamRepository.GetByIdWithMembersAsync(command.TeamId, ct)
            ?? throw new InvalidOperationException($"Team '{command.TeamId}' not found.");

        team.Update(command.Name);
        await _unitOfWork.CommitAsync(ct);

        return new TeamDto(team.Id, team.OrganizationId, team.Name, team.CreatedAt);
    }
}
