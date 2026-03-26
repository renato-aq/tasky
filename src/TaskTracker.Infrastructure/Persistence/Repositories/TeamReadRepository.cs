using Dapper;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;
using TaskTracker.Application.Features.Teams.DTOs;

namespace TaskTracker.Infrastructure.Persistence.Repositories;

public class TeamReadRepository : ITeamReadRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TeamReadRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<TeamDto>> GetByOrganizationAsync(Guid organizationId, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT id, organization_id AS OrganizationId, name, created_at AS CreatedAt
            FROM teams
            WHERE organization_id = @OrganizationId
            ORDER BY created_at
            """;

        return await connection.QueryAsync<TeamDto>(sql, new { OrganizationId = organizationId });
    }

    public async Task<TeamDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string teamSql = """
            SELECT id, organization_id AS OrganizationId, name, created_at AS CreatedAt
            FROM teams
            WHERE id = @Id
            """;

        const string membersSql = """
            SELECT tm.user_id AS UserId, u.name AS UserName, u.email AS Email, tm.role AS Role
            FROM team_members tm
            INNER JOIN users u ON u.id = tm.user_id
            WHERE tm.team_id = @Id
            """;

        var team = await connection.QuerySingleOrDefaultAsync<TeamDto>(teamSql, new { Id = id });
        if (team is null) return null;

        var members = await connection.QueryAsync<TeamMemberDto>(membersSql, new { Id = id });

        return new TeamDetailDto(team.Id, team.OrganizationId, team.Name, team.CreatedAt, members);
    }
}
