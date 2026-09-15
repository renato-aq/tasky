using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Teams.DTOs;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.Teams.Commands.CreateTeam;

public class CreateTeamCommandHandler : ICommandHandler<CreateTeamCommand, TeamDto>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTeamCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TeamDto> HandleAsync(CreateTeamCommand command, CancellationToken ct = default)
    {
        var team = Team.Create(command.OrganizationId, command.Name);

        await _teamRepository.AddAsync(team, ct);
        await _unitOfWork.CommitAsync(ct);

        return new TeamDto(team.Id, team.OrganizationId, team.Name, team.CreatedAt);
    }
}
