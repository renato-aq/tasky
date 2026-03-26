using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Commands.StartSprint;

public class StartSprintCommandHandler : ICommandHandler<StartSprintCommand, SprintDto>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ISprintReadRepository _sprintReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartSprintCommandHandler(
        ISprintRepository sprintRepository,
        ISprintReadRepository sprintReadRepository,
        IUnitOfWork unitOfWork)
    {
        _sprintRepository = sprintRepository;
        _sprintReadRepository = sprintReadRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SprintDto> HandleAsync(StartSprintCommand command, CancellationToken ct = default)
    {
        var sprint = await _sprintRepository.GetByIdAsync(command.SprintId, ct)
            ?? throw new InvalidOperationException("Sprint not found.");

        var hasActive = await _sprintReadRepository.HasActiveSprintAsync(sprint.ProjectId, ct);
        if (hasActive)
            throw new InvalidOperationException("This project already has an active sprint.");

        sprint.Start(DateTime.UtcNow);
        await _unitOfWork.CommitAsync(ct);

        return (await _sprintReadRepository.GetByIdAsync(sprint.Id, ct))!;
    }
}
