using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Commands.CompleteSprint;

public class CompleteSprintCommandHandler : ICommandHandler<CompleteSprintCommand, SprintDto>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ISprintReadRepository _sprintReadRepository;
    private readonly ITaskReadRepository _taskReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public CompleteSprintCommandHandler(
        ISprintRepository sprintRepository,
        ISprintReadRepository sprintReadRepository,
        ITaskReadRepository taskReadRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService)
    {
        _sprintRepository = sprintRepository;
        _sprintReadRepository = sprintReadRepository;
        _taskReadRepository = taskReadRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<SprintDto> HandleAsync(CompleteSprintCommand command, CancellationToken ct = default)
    {
        var sprint = await _sprintRepository.GetByIdAsync(command.SprintId, ct)
            ?? throw new InvalidOperationException("Sprint not found.");

        var userIds = await _taskReadRepository.GetAssignedUsersBySprintAsync(sprint.Id, ct);
        var userIdList = userIds.ToList();

        sprint.Complete(DateTime.UtcNow);
        await _unitOfWork.CommitAsync(ct);

        if (userIdList.Count > 0)
            await _notificationService.NotifySprintCompletedAsync(userIdList, sprint.Id, sprint.Name, ct);

        return (await _sprintReadRepository.GetByIdAsync(sprint.Id, ct))!;
    }
}
