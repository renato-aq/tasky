namespace TaskTracker.Application.Features.Teams.DTOs;

public record TeamDto(Guid Id, Guid OrganizationId, string Name, DateTime CreatedAt);

public record TeamMemberDto(Guid UserId, string UserName, string Email, string Role);

public record TeamDetailDto(Guid Id, Guid OrganizationId, string Name, DateTime CreatedAt, IEnumerable<TeamMemberDto> Members);
