using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Commands.AddCeremony;

public class AddCeremonyCommandHandler : ICommandHandler<AddCeremonyCommand, SprintCeremonyDto>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCeremonyCommandHandler(ISprintRepository sprintRepository, IUnitOfWork unitOfWork)
    {
        _sprintRepository = sprintRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SprintCeremonyDto> HandleAsync(AddCeremonyCommand command, CancellationToken ct = default)
    {
        var sprint = await _sprintRepository.GetByIdWithCeremoniesAsync(command.SprintId, ct)
            ?? throw new InvalidOperationException("Sprint not found.");

        var ceremony = sprint.AddCeremony(command.Type, command.Notes, command.OccurredAt);
        await _sprintRepository.AddCeremonyAsync(ceremony, ct);
        await _unitOfWork.CommitAsync(ct);

        return new SprintCeremonyDto(
            ceremony.Id,
            ceremony.SprintId,
            ceremony.Type.ToString().ToLower(),
            ceremony.Notes,
            ceremony.OccurredAt);
    }
}
