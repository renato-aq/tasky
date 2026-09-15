using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Tasks.DTOs;

namespace TaskTracker.Application.Features.Tasks.Queries.GetTaskById;

public class GetTaskByIdQueryHandler : IQueryHandler<GetTaskByIdQuery, TaskDto?>
{
    private readonly ITaskReadRepository _taskReadRepository;

    public GetTaskByIdQueryHandler(ITaskReadRepository taskReadRepository)
    {
        _taskReadRepository = taskReadRepository;
    }

    public Task<TaskDto?> HandleAsync(GetTaskByIdQuery query, CancellationToken ct = default)
        => _taskReadRepository.GetByIdAsync(query.TaskId, ct);
}
