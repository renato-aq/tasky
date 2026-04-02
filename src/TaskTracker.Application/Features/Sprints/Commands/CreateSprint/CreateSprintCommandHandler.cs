using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Sprints.DTOs;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.Sprints.Commands.CreateSprint;

public class CreateSprintCommandHandler : ICommandHandler<CreateSprintCommand, SprintDto>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ISprintReadRepository _sprintReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSprintCommandHandler(
        ISprintRepository sprintRepository,
        ISprintReadRepository sprintReadRepository,
        IUnitOfWork unitOfWork)
    {
        _sprintRepository = sprintRepository;
        _sprintReadRepository = sprintReadRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SprintDto> HandleAsync(CreateSprintCommand command, CancellationToken ct = default)
    {
        var sprint = Sprint.Create(command.ProjectId, command.Name, command.Goal, command.DurationDays);

        await _sprintRepository.AddAsync(sprint, ct);
        await _unitOfWork.CommitAsync(ct);

        return (await _sprintReadRepository.GetByIdAsync(sprint.Id, ct))!;
    }
}
