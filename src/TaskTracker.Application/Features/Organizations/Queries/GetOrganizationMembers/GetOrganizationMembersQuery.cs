using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Organizations.DTOs;

namespace TaskTracker.Application.Features.Organizations.Queries.GetOrganizationMembers;

public record GetOrganizationMembersQuery(Guid OrganizationId) : IQuery<IEnumerable<OrganizationMemberDto>>;
