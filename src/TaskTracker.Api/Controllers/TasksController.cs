using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Models;
using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Tasks.Commands.AssignTask;
using TaskTracker.Application.Features.Tasks.Commands.CreateTask;
using TaskTracker.Application.Features.Tasks.Commands.DeleteTask;
using TaskTracker.Application.Features.Tasks.Commands.UpdateTask;
using TaskTracker.Application.Features.Tasks.Commands.UpdateTaskStatus;
using TaskTracker.Application.Features.Tasks.DTOs;
using TaskTracker.Application.Features.Tasks.Commands.CreateSubTask;
using TaskTracker.Application.Features.Tasks.Commands.DeleteSubTask;
using TaskTracker.Application.Features.Tasks.Commands.UpdateSubTaskStatus;
using TaskTracker.Application.Features.Tasks.Queries.GetBacklogByProject;
using TaskTracker.Application.Features.Tasks.Queries.GetSubTasksByTask;
using TaskTracker.Application.Features.Tasks.Queries.GetTaskById;
using TaskTracker.Application.Features.Tasks.Queries.GetTasksByProject;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Api.Controllers;

[Authorize]
[Route("api/tasks")]
public class TasksController : BaseApiController
{
    public TasksController(IDispatcher dispatcher) : base(dispatcher) { }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TaskDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByProject([FromQuery] Guid projectId, CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetTasksByProjectQuery, IEnumerable<TaskDto>>(
            new GetTasksByProjectQuery(projectId), ct);
        return Ok(ApiResponse<IEnumerable<TaskDto>>.Ok(result));
    }

    [HttpGet("backlog")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TaskDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBacklog([FromQuery] Guid projectId, CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetBacklogByProjectQuery, IEnumerable<TaskDto>>(
            new GetBacklogByProjectQuery(projectId), ct);
        return Ok(ApiResponse<IEnumerable<TaskDto>>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TaskDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetTaskByIdQuery, TaskDto?>(
            new GetTaskByIdQuery(id), ct);

        if (result is null)
            return NotFound(ApiResponse<object>.Fail("Task not found."));

        return Ok(ApiResponse<TaskDto>.Ok(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TaskDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request, CancellationToken ct)
    {
        if (!Enum.TryParse<TaskPriority>(request.Priority ?? "medium", true, out var priority))
            return BadRequest(ApiResponse<object>.Fail($"Invalid priority: {request.Priority}"));

        var command = new CreateTaskCommand(
            request.ProjectId,
            request.Title,
            User.Identity!.Name ?? CurrentUserId.ToString(),
            request.Description,
            priority,
            request.DueDate,
            request.SprintId,
            request.CeremonyId,
            request.AssignedUserId,
            request.AssignedTeamId);

        var result = await Dispatcher.SendAsync<CreateTaskCommand, TaskDto>(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<TaskDto>.Ok(result));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request, CancellationToken ct)
    {
        if (!Enum.TryParse<TaskPriority>(request.Priority ?? "medium", true, out var priority))
            return BadRequest(ApiResponse<object>.Fail($"Invalid priority: {request.Priority}"));

        try
        {
            await Dispatcher.SendAsync(new UpdateTaskCommand(id, request.Title, request.Description, priority, request.DueDate), ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await Dispatcher.SendAsync(new DeleteTaskCommand(id), ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request, CancellationToken ct)
    {
        if (!Enum.TryParse<TaskItemStatus>(request.Status, true, out var status))
            return BadRequest(ApiResponse<object>.Fail($"Invalid status: {request.Status}"));

        try
        {
            await Dispatcher.SendAsync(new UpdateTaskStatusCommand(id, status), ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpPatch("{id:guid}/assign")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Assign(Guid id, [FromBody] AssignTaskRequest request, CancellationToken ct)
    {
        try
        {
            await Dispatcher.SendAsync(new AssignTaskCommand(id, request.AssignedUserId, request.AssignedTeamId), ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message));
        }
    }

    // --- SubTasks ---

    [HttpGet("{taskId:guid}/subtasks")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<SubTaskDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubTasks(Guid taskId, CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetSubTasksByTaskQuery, IEnumerable<SubTaskDto>>(
            new GetSubTasksByTaskQuery(taskId), ct);
        return Ok(ApiResponse<IEnumerable<SubTaskDto>>.Ok(result));
    }

    [HttpPost("{taskId:guid}/subtasks")]
    [ProducesResponseType(typeof(ApiResponse<SubTaskDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSubTask(Guid taskId, [FromBody] CreateSubTaskRequest request, CancellationToken ct)
    {
        var result = await Dispatcher.SendAsync<CreateSubTaskCommand, SubTaskDto>(
            new CreateSubTaskCommand(taskId, request.Title, request.AssignedUserId), ct);
        return StatusCode(StatusCodes.Status201Created, ApiResponse<SubTaskDto>.Ok(result));
    }

    [HttpPatch("{taskId:guid}/subtasks/{subTaskId:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSubTaskStatus(Guid taskId, Guid subTaskId, [FromBody] UpdateSubTaskStatusRequest request, CancellationToken ct)
    {
        if (!Enum.TryParse<SubTaskStatus>(request.Status, true, out var status))
            return BadRequest(ApiResponse<object>.Fail($"Invalid status: {request.Status}"));

        try
        {
            await Dispatcher.SendAsync(new UpdateSubTaskStatusCommand(subTaskId, status), ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpDelete("{taskId:guid}/subtasks/{subTaskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSubTask(Guid taskId, Guid subTaskId, CancellationToken ct)
    {
        try
        {
            await Dispatcher.SendAsync(new DeleteSubTaskCommand(subTaskId), ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message));
        }
    }
}

public record CreateTaskRequest(
    Guid ProjectId,
    string Title,
    string? Description,
    string? Priority,
    DateTime? DueDate,
    Guid? SprintId,
    Guid? CeremonyId,
    Guid? AssignedUserId,
    Guid? AssignedTeamId);

public record UpdateTaskRequest(
    string Title,
    string? Description,
    string? Priority,
    DateTime? DueDate);

public record UpdateStatusRequest(string Status);
public record AssignTaskRequest(Guid? AssignedUserId, Guid? AssignedTeamId);
public record CreateSubTaskRequest(string Title, Guid? AssignedUserId);
public record UpdateSubTaskStatusRequest(string Status);
