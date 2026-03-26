using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Models;
using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Projects.Commands.CreateProject;
using TaskTracker.Application.Features.Projects.Commands.DeleteProject;
using TaskTracker.Application.Features.Projects.Commands.UpdateProject;
using TaskTracker.Application.Features.Projects.DTOs;
using TaskTracker.Application.Features.Projects.Queries.GetProjectById;
using TaskTracker.Application.Features.Projects.Queries.GetProjectsByTeam;

namespace TaskTracker.Api.Controllers;

[Authorize]
[Route("api/projects")]
public class ProjectsController : BaseApiController
{
    public ProjectsController(IDispatcher dispatcher) : base(dispatcher) { }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProjectDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTeam([FromQuery] Guid teamId, CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetProjectsByTeamQuery, IEnumerable<ProjectDto>>(
            new GetProjectsByTeamQuery(teamId), ct);
        return Ok(ApiResponse<IEnumerable<ProjectDto>>.Ok(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateProjectRequest request, CancellationToken ct)
    {
        var command = new CreateProjectCommand(request.TeamId, request.Name, request.Description);
        var result = await Dispatcher.SendAsync<CreateProjectCommand, ProjectDto>(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<ProjectDto>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetProjectByIdQuery, ProjectDto?>(
            new GetProjectByIdQuery(id), ct);

        if (result is null)
            return NotFound(ApiResponse<object>.Fail("Project not found."));

        return Ok(ApiResponse<ProjectDto>.Ok(result));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request, CancellationToken ct)
    {
        var result = await Dispatcher.SendAsync<UpdateProjectCommand, ProjectDto>(
            new UpdateProjectCommand(id, request.Name, request.Description), ct);
        return Ok(ApiResponse<ProjectDto>.Ok(result));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await Dispatcher.SendAsync(new DeleteProjectCommand(id), ct);
        return NoContent();
    }
}

public record CreateProjectRequest(Guid TeamId, string Name, string? Description);
public record UpdateProjectRequest(string Name, string? Description);
