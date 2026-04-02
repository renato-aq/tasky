using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Projects.DTOs;

namespace TaskTracker.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand, ProjectDto>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProjectCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProjectDto> HandleAsync(UpdateProjectCommand command, CancellationToken ct = default)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct)
            ?? throw new InvalidOperationException($"Project {command.ProjectId} not found.");

        project.Update(command.Name, command.Description);
        await _unitOfWork.CommitAsync(ct);

        return new ProjectDto(project.Id, project.TeamId, project.Name, project.Description, project.CreatedAt);
    }
}
