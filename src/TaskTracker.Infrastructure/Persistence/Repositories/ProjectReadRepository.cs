using Dapper;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;
using TaskTracker.Application.Features.Projects.DTOs;

namespace TaskTracker.Infrastructure.Persistence.Repositories;

public class ProjectReadRepository : IProjectReadRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ProjectReadRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ProjectDto>> GetByTeamIdAsync(Guid teamId, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT id, team_id AS TeamId, name, description, created_at AS CreatedAt
            FROM projects
            WHERE team_id = @TeamId AND deleted_at IS NULL
            ORDER BY created_at
            """;

        return await connection.QueryAsync<ProjectDto>(sql, new { TeamId = teamId });
    }

    public async Task<IEnumerable<ProjectDto>> GetByUserAsync(Guid userId, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT p.id, p.team_id AS TeamId, p.name, p.description, p.created_at AS CreatedAt
            FROM projects p
            INNER JOIN team_members tm ON tm.team_id = p.team_id
            WHERE tm.user_id = @UserId
              AND p.deleted_at IS NULL
            ORDER BY p.created_at
            """;

        return await connection.QueryAsync<ProjectDto>(sql, new { UserId = userId });
    }

    public async Task<ProjectDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT id, team_id AS TeamId, name, description, created_at AS CreatedAt
            FROM projects
            WHERE id = @Id AND deleted_at IS NULL
            """;

        return await connection.QuerySingleOrDefaultAsync<ProjectDto>(sql, new { Id = id });
    }
}
