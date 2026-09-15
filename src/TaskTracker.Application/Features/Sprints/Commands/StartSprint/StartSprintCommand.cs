using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Application.Features.Sprints.Commands.StartSprint;

public record StartSprintCommand(Guid SprintId) : ICommand<SprintDto>;
