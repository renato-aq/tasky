using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Commands.CreateSprint;

public record CreateSprintCommand(Guid ProjectId, string Name, string? Goal, int DurationDays) : ICommand<SprintDto>;
