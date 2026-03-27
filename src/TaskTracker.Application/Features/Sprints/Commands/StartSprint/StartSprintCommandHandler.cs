using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Commands.StartSprint;

public class StartSprintCommandHandler : ICommandHandler<StartSprintCommand, SprintDto>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ISprintReadRepository _sprintReadRepository;
    private readonly ITaskReadRepository _taskReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public StartSprintCommandHandler(
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

    public async Task<SprintDto> HandleAsync(StartSprintCommand command, CancellationToken ct = default)
    {
        var sprint = await _sprintRepository.GetByIdAsync(command.SprintId, ct)
            ?? throw new InvalidOperationException("Sprint not found.");

        var hasActive = await _sprintReadRepository.HasActiveSprintAsync(sprint.ProjectId, ct);
        if (hasActive)
            throw new InvalidOperationException("This project already has an active sprint.");

        sprint.Start(DateTime.UtcNow);
        await _unitOfWork.CommitAsync(ct);

        var userIds = await _taskReadRepository.GetAssignedUsersBySprintAsync(sprint.Id, ct);
        var userIdList = userIds.ToList();
        if (userIdList.Count > 0)
            await _notificationService.NotifySprintStartedAsync(userIdList, sprint.Id, sprint.Name, ct);

        return (await _sprintReadRepository.GetByIdAsync(sprint.Id, ct))!;
    }
}
