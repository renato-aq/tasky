namespace TaskTracker.Application.Features.Organizations.DTOs;

public record OrganizationMemberDto(Guid UserId, string UserName, string UserEmail, string Role);
