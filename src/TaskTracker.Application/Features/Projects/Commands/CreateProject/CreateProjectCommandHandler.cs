using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Projects.DTOs;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProjectDto> HandleAsync(CreateProjectCommand command, CancellationToken ct = default)
    {
        var project = Project.Create(command.TeamId, command.Name, command.Description);

        await _projectRepository.AddAsync(project, ct);
        await _unitOfWork.CommitAsync(ct);

        return new ProjectDto(project.Id, project.TeamId, project.Name, project.Description, project.CreatedAt);
    }
}
