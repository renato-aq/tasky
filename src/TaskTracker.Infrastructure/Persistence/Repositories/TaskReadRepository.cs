using Dapper;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;
using TaskTracker.Application.Features.Tasks.DTOs;

namespace TaskTracker.Infrastructure.Persistence.Repositories;

public class TaskReadRepository : ITaskReadRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TaskReadRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    private const string SelectColumns = """
        t.id, t.project_id AS ProjectId, t.sprint_id AS SprintId,
        t.created_in_ceremony_id AS CreatedInCeremonyId,
        t.title, t.description, t.status, t.priority, t.due_date AS DueDate,
        t.created_by AS CreatedBy,
        t.assigned_user_id AS AssignedUserId,
        u.name AS AssignedUserName,
        t.assigned_team_id AS AssignedTeamId,
        t.created_at AS CreatedAt, t.updated_at AS UpdatedAt
        """;

    public async Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = $"""
            SELECT {SelectColumns}
            FROM tasks t
            LEFT JOIN users u ON u.id = t.assigned_user_id
            WHERE t.id = @Id
            """;

        return await connection.QuerySingleOrDefaultAsync<TaskDto>(sql, new { Id = id });
    }

    public async Task<IEnumerable<TaskDto>> GetByProjectAsync(Guid projectId, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = $"""
            SELECT {SelectColumns}
            FROM tasks t
            LEFT JOIN users u ON u.id = t.assigned_user_id
            WHERE t.project_id = @ProjectId
            ORDER BY t.created_at DESC
            """;

        return await connection.QueryAsync<TaskDto>(sql, new { ProjectId = projectId });
    }

    public async Task<IEnumerable<TaskDto>> GetBacklogByProjectAsync(Guid projectId, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = $"""
            SELECT {SelectColumns}
            FROM tasks t
            LEFT JOIN users u ON u.id = t.assigned_user_id
            WHERE t.project_id = @ProjectId AND t.sprint_id IS NULL
            ORDER BY t.priority DESC, t.created_at DESC
            """;

        return await connection.QueryAsync<TaskDto>(sql, new { ProjectId = projectId });
    }

    public async Task<IEnumerable<TaskDto>> GetByTeamPoolAsync(Guid teamId, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = $"""
            SELECT {SelectColumns}
            FROM tasks t
            LEFT JOIN users u ON u.id = t.assigned_user_id
            WHERE t.assigned_team_id = @TeamId AND t.assigned_user_id IS NULL
            ORDER BY t.created_at DESC
            """;

        return await connection.QueryAsync<TaskDto>(sql, new { TeamId = teamId });
    }

    public async Task<IEnumerable<Guid>> GetAssignedUsersBySprintAsync(Guid sprintId, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT DISTINCT assigned_user_id
            FROM tasks
            WHERE sprint_id = @SprintId AND assigned_user_id IS NOT NULL
            """;

        return await connection.QueryAsync<Guid>(sql, new { SprintId = sprintId });
    }
}
