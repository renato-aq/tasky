using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Commands.CompleteSprint;

public class CompleteSprintCommandHandler : ICommandHandler<CompleteSprintCommand, SprintDto>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ISprintReadRepository _sprintReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteSprintCommandHandler(
        ISprintRepository sprintRepository,
        ISprintReadRepository sprintReadRepository,
        IUnitOfWork unitOfWork)
    {
        _sprintRepository = sprintRepository;
        _sprintReadRepository = sprintReadRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SprintDto> HandleAsync(CompleteSprintCommand command, CancellationToken ct = default)
    {
        var sprint = await _sprintRepository.GetByIdAsync(command.SprintId, ct)
            ?? throw new InvalidOperationException("Sprint not found.");

        sprint.Complete(DateTime.UtcNow);
        await _unitOfWork.CommitAsync(ct);

        return (await _sprintReadRepository.GetByIdAsync(sprint.Id, ct))!;
    }
}
