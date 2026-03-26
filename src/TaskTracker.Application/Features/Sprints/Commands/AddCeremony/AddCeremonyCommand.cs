using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Sprints.DTOs;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Sprints.Commands.AddCeremony;

public record AddCeremonyCommand(Guid SprintId, CeremonyType Type, string? Notes, DateTime OccurredAt) : ICommand<SprintCeremonyDto>;
