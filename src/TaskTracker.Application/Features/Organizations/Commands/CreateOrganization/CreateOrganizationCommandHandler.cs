using System.Text.RegularExpressions;
using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Organizations.DTOs;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.Organizations.Commands.CreateOrganization;

public class CreateOrganizationCommandHandler : ICommandHandler<CreateOrganizationCommand, OrganizationDto>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrganizationCommandHandler(
        IOrganizationRepository organizationRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrganizationDto> HandleAsync(CreateOrganizationCommand command, CancellationToken ct = default)
    {
        var slug = await GenerateUniqueSlugAsync(command.Name, ct);

        var organization = Organization.Create(command.Name, slug);

        await _organizationRepository.AddAsync(organization, ct);
        await _userRepository.SetOrganizationAsync(command.UserId, organization.Id, ct);
        await _unitOfWork.CommitAsync(ct);

        return new OrganizationDto(organization.Id, organization.Name, organization.Slug, organization.CreatedAt);
    }

    private async Task<string> GenerateUniqueSlugAsync(string name, CancellationToken ct)
    {
        var baseSlug = Regex.Replace(name.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');

        if (!await _organizationRepository.SlugExistsAsync(baseSlug, ct))
            return baseSlug;

        var counter = 2;
        string candidate;
        do
        {
            candidate = $"{baseSlug}-{counter++}";
        } while (await _organizationRepository.SlugExistsAsync(candidate, ct));

        return candidate;
    }
}
