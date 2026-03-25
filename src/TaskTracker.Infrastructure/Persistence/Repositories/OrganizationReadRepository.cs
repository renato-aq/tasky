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
}
