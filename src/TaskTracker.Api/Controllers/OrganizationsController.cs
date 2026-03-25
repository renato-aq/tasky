using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Models;
using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Organizations.Commands.CreateOrganization;
using TaskTracker.Application.Features.Organizations.DTOs;
using TaskTracker.Application.Features.Organizations.Queries.GetMyOrganizations;
using TaskTracker.Application.Features.Organizations.Queries.GetOrganizationById;

namespace TaskTracker.Api.Controllers;

[Route("api/organizations")]
public class OrganizationsController : BaseApiController
{
    public OrganizationsController(IDispatcher dispatcher) : base(dispatcher) { }

    [HttpGet("mine")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrganizationDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMine(CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetMyOrganizationsQuery, IEnumerable<OrganizationDto>>(
            new GetMyOrganizationsQuery(CurrentUserId), ct);
        return Ok(ApiResponse<IEnumerable<OrganizationDto>>.Ok(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<OrganizationDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationRequest request, CancellationToken ct)
    {
        var command = new CreateOrganizationCommand(request.Name, CurrentUserId);
        var result = await Dispatcher.SendAsync<CreateOrganizationCommand, OrganizationDto>(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<OrganizationDto>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<OrganizationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await Dispatcher.QueryAsync<GetOrganizationByIdQuery, OrganizationDto?>(
            new GetOrganizationByIdQuery(id), ct);

        if (result is null)
            return NotFound(ApiResponse<object>.Fail("Organization not found."));

        return Ok(ApiResponse<OrganizationDto>.Ok(result));
    }
}

public record CreateOrganizationRequest(string Name);
