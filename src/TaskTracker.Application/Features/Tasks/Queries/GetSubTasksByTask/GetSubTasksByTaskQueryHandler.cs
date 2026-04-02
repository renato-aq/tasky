using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Tasks.DTOs;

namespace TaskTracker.Application.Features.Tasks.Queries.GetSubTasksByTask;

public class GetSubTasksByTaskQueryHandler : IQueryHandler<GetSubTasksByTaskQuery, IEnumerable<SubTaskDto>>
{
    private readonly ISubTaskRepository _subTaskRepository;

    public GetSubTasksByTaskQueryHandler(ISubTaskRepository subTaskRepository)
    {
        _subTaskRepository = subTaskRepository;
    }

    public async Task<IEnumerable<SubTaskDto>> HandleAsync(GetSubTasksByTaskQuery query, CancellationToken ct = default)
    {
        var subTasks = await _subTaskRepository.GetByTaskIdAsync(query.TaskId, ct);
        return subTasks.Select(s => new SubTaskDto(
            s.Id, s.TaskId, s.Title,
            s.Status.ToString().ToLower(),
            s.AssignedUserId, null,
            s.CreatedAt));
    }
}
