using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Models;
using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Teams.Commands.AddTeamMember;
using TaskTracker.Application.Features.Teams.Commands.CreateTeam;
using TaskTracker.Application.Features.Teams.Commands.RemoveTeamMember;
using TaskTracker.Application.Features.Teams.Commands.UpdateTeam;
using TaskTracker.Application.Features.Teams.DTOs;
using TaskTracker.Application.Features.Teams.Queries.GetTeamById;
using TaskTracker.Application.Features.Teams.Queries.GetMyTeams;
using TaskTracker.Application.Features.Teams.Queries.GetTeamsByOrganization;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Api.Controllers;

[Route("api/teams")]
public class TeamsController : BaseApiController
{
    public TeamsController(IDispatcher dispatcher) : base(dispatcher) { }

    [HttpGet("mine")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TeamDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMine(CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetMyTeamsQuery, IEnumerable<TeamDto>>(
            new GetMyTeamsQuery(CurrentUserId), ct);
        return Ok(ApiResponse<IEnumerable<TeamDto>>.Ok(result));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TeamDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByOrganization([FromQuery] Guid organizationId, CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetTeamsByOrganizationQuery, IEnumerable<TeamDto>>(
            new GetTeamsByOrganizationQuery(organizationId), ct);
        return Ok(ApiResponse<IEnumerable<TeamDto>>.Ok(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TeamDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateTeamRequest request, CancellationToken ct)
    {
        var command = new CreateTeamCommand(request.OrganizationId, request.Name);
        var result = await Dispatcher.SendAsync<CreateTeamCommand, TeamDto>(command, ct);
        return CreatedAtAction(nameof(GetById), new { teamId = result.Id }, ApiResponse<TeamDto>.Ok(result));
    }

    [HttpGet("{teamId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TeamDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid teamId, CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetTeamByIdQuery, TeamDetailDto?>(
            new GetTeamByIdQuery(teamId), ct);

        if (result is null)
            return NotFound(ApiResponse<object>.Fail("Team not found."));

        return Ok(ApiResponse<TeamDetailDto>.Ok(result));
    }

    [HttpPut("{teamId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TeamDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid teamId, [FromBody] UpdateTeamRequest request, CancellationToken ct)
    {
        var result = await Dispatcher.SendAsync<UpdateTeamCommand, TeamDto>(
            new UpdateTeamCommand(teamId, request.Name), ct);
        return Ok(ApiResponse<TeamDto>.Ok(result));
    }

    [HttpPost("{teamId:guid}/members")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddMember(Guid teamId, [FromBody] AddMemberRequest request, CancellationToken ct)
    {
        await Dispatcher.SendAsync(new AddTeamMemberCommand(teamId, request.UserId, request.Role), ct);
        return NoContent();
    }

    [HttpDelete("{teamId:guid}/members/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveMember(Guid teamId, Guid userId, CancellationToken ct)
    {
        await Dispatcher.SendAsync(new RemoveTeamMemberCommand(teamId, userId), ct);
        return NoContent();
    }
}

public record CreateTeamRequest(Guid OrganizationId, string Name);
public record UpdateTeamRequest(string Name);
public record AddMemberRequest(Guid UserId, TeamRole Role);
