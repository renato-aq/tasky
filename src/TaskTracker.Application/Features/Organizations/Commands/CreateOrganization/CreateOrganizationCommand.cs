using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Features.Organizations.DTOs;

namespace TaskTracker.Application.Features.Organizations.Commands.CreateOrganization;

public record CreateOrganizationCommand(string Name, Guid UserId) : ICommand<OrganizationDto>;
