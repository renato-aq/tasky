using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Models;
using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Sprints.Commands.AddCeremony;
using TaskTracker.Application.Features.Sprints.Commands.CompleteSprint;
using TaskTracker.Application.Features.Sprints.Commands.CreateSprint;
using TaskTracker.Application.Features.Sprints.Commands.StartSprint;
using TaskTracker.Application.Features.Sprints.DTOs;
using TaskTracker.Application.Features.Sprints.Queries.GetSprintById;
using TaskTracker.Application.Features.Sprints.Queries.GetSprintCeremonies;
using TaskTracker.Application.Features.Sprints.Queries.GetSprintsByProject;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Api.Controllers;

[Authorize]
[Route("api/sprints")]
public class SprintsController : BaseApiController
{
    public SprintsController(IDispatcher dispatcher) : base(dispatcher) { }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<SprintDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByProject([FromQuery] Guid projectId, CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetSprintsByProjectQuery, IEnumerable<SprintDto>>(
            new GetSprintsByProjectQuery(projectId), ct);
        return Ok(ApiResponse<IEnumerable<SprintDto>>.Ok(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SprintDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateSprintRequest request, CancellationToken ct)
    {
        var command = new CreateSprintCommand(request.ProjectId, request.Name, request.Goal, request.DurationDays);
        var result = await Dispatcher.SendAsync<CreateSprintCommand, SprintDto>(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<SprintDto>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SprintDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetSprintByIdQuery, SprintDto?>(
            new GetSprintByIdQuery(id), ct);

        if (result is null)
            return NotFound(ApiResponse<object>.Fail("Sprint not found."));

        return Ok(ApiResponse<SprintDto>.Ok(result));
    }

    [HttpPost("{id:guid}/start")]
    [ProducesResponseType(typeof(ApiResponse<SprintDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Start(Guid id, CancellationToken ct)
    {
        try
        {
            var result = await Dispatcher.SendAsync<StartSprintCommand, SprintDto>(
                new StartSprintCommand(id), ct);
            return Ok(ApiResponse<SprintDto>.Ok(result));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(typeof(ApiResponse<SprintDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Complete(Guid id, CancellationToken ct)
    {
        try
        {
            var result = await Dispatcher.SendAsync<CompleteSprintCommand, SprintDto>(
                new CompleteSprintCommand(id), ct);
            return Ok(ApiResponse<SprintDto>.Ok(result));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpGet("{id:guid}/ceremonies")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<SprintCeremonyDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCeremonies(Guid id, CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetSprintCeremoniesQuery, IEnumerable<SprintCeremonyDto>>(
            new GetSprintCeremoniesQuery(id), ct);
        return Ok(ApiResponse<IEnumerable<SprintCeremonyDto>>.Ok(result));
    }

    [HttpPost("{id:guid}/ceremonies")]
    [ProducesResponseType(typeof(ApiResponse<SprintCeremonyDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddCeremony(Guid id, [FromBody] AddCeremonyRequest request, CancellationToken ct)
    {
        try
        {
            if (!Enum.TryParse<CeremonyType>(request.Type, true, out var ceremonyType))
                return BadRequest(ApiResponse<object>.Fail($"Invalid ceremony type: {request.Type}"));

            var command = new AddCeremonyCommand(id, ceremonyType, request.Notes, request.OccurredAt);
            var result = await Dispatcher.SendAsync<AddCeremonyCommand, SprintCeremonyDto>(command, ct);
            return StatusCode(StatusCodes.Status201Created, ApiResponse<SprintCeremonyDto>.Ok(result));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }
}

public record CreateSprintRequest(Guid ProjectId, string Name, string? Goal, int DurationDays = 14);
public record AddCeremonyRequest(string Type, string? Notes, DateTime OccurredAt);
