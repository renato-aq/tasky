using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Projects.DTOs;

namespace TaskTracker.Application.Features.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandler : IQueryHandler<GetProjectByIdQuery, ProjectDto?>
{
    private readonly IProjectReadRepository _projectReadRepository;

    public GetProjectByIdQueryHandler(IProjectReadRepository projectReadRepository)
    {
        _projectReadRepository = projectReadRepository;
    }

    public Task<ProjectDto?> HandleAsync(GetProjectByIdQuery query, CancellationToken ct = default)
        => _projectReadRepository.GetByIdAsync(query.ProjectId, ct);
}
