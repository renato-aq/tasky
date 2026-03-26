using Dapper;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;
using TaskTracker.Application.Features.Sprints.DTOs;

namespace TaskTracker.Infrastructure.Persistence.Repositories;

public class SprintReadRepository : ISprintReadRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SprintReadRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<SprintDto>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT id, project_id AS ProjectId, name, goal, duration_days AS DurationDays,
                   start_date AS StartDate, end_date AS EndDate, status, created_at AS CreatedAt
            FROM sprints
            WHERE project_id = @ProjectId
            ORDER BY created_at
            """;

        return await connection.QueryAsync<SprintDto>(sql, new { ProjectId = projectId });
    }

    public async Task<SprintDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT id, project_id AS ProjectId, name, goal, duration_days AS DurationDays,
                   start_date AS StartDate, end_date AS EndDate, status, created_at AS CreatedAt
            FROM sprints
            WHERE id = @Id
            """;

        return await connection.QuerySingleOrDefaultAsync<SprintDto>(sql, new { Id = id });
    }

    public async Task<IEnumerable<SprintCeremonyDto>> GetCeremoniesAsync(Guid sprintId, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT id, sprint_id AS SprintId, type, notes, occurred_at AS OccurredAt
            FROM sprint_ceremonies
            WHERE sprint_id = @SprintId
            ORDER BY occurred_at
            """;

        return await connection.QueryAsync<SprintCeremonyDto>(sql, new { SprintId = sprintId });
    }

    public async Task<bool> HasActiveSprintAsync(Guid projectId, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT EXISTS(
                SELECT 1 FROM sprints
                WHERE project_id = @ProjectId AND status = 'active'
            )
            """;

        return await connection.ExecuteScalarAsync<bool>(sql, new { ProjectId = projectId });
    }
}
