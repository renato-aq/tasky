using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Organizations.DTOs;

namespace TaskTracker.Application.Features.Organizations.Queries.GetMyOrganizations;

public record GetMyOrganizationsQuery(Guid UserId) : IQuery<IEnumerable<OrganizationDto>>;
