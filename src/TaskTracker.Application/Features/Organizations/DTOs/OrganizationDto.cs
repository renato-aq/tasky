namespace TaskTracker.Application.Features.Organizations.DTOs;

public record OrganizationDto(Guid Id, string Name, string Slug, DateTime CreatedAt);
