using Dapper;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;
using TaskTracker.Application.Features.Organizations.DTOs;

namespace TaskTracker.Infrastructure.Persistence.Repositories;

public class OrganizationReadRepository : IOrganizationReadRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public OrganizationReadRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<OrganizationDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT id, name, slug, created_at AS CreatedAt
            FROM organizations
            WHERE id = @Id
            """;

        return await connection.QuerySingleOrDefaultAsync<OrganizationDto>(sql, new { Id = id });
    }

    public async Task<IEnumerable<OrganizationDto>> GetByUserAsync(Guid userId, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT o.id, o.name, o.slug, o.created_at AS CreatedAt
            FROM organizations o
            INNER JOIN users u ON u.organization_id = o.id
            WHERE u.id = @UserId
            """;

        return await connection.QueryAsync<OrganizationDto>(sql, new { UserId = userId });
    }
}
