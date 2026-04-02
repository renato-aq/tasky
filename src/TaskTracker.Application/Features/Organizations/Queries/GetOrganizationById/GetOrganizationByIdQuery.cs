using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Organizations.DTOs;

namespace TaskTracker.Application.Features.Organizations.Queries.GetOrganizationById;

public record GetOrganizationByIdQuery(Guid Id) : IQuery<OrganizationDto?>;
