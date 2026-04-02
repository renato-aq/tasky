using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Features.Projects.DTOs;

namespace TaskTracker.Application.Features.Projects.Queries.GetMyProjects;

public class GetMyProjectsQueryHandler : IQueryHandler<GetMyProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectReadRepository _readRepository;

    public GetMyProjectsQueryHandler(IProjectReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<IEnumerable<ProjectDto>> HandleAsync(GetMyProjectsQuery query, CancellationToken ct = default)
        => _readRepository.GetByUserAsync(query.UserId, ct);
}
