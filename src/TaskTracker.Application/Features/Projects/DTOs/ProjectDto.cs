namespace TaskTracker.Application.Features.Projects.DTOs;

public record ProjectDto(Guid Id, Guid TeamId, string Name, string? Description, DateTime CreatedAt);
