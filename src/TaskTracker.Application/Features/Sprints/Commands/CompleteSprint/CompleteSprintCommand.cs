using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Commands.CompleteSprint;

public record CompleteSprintCommand(Guid SprintId) : ICommand<SprintDto>;
