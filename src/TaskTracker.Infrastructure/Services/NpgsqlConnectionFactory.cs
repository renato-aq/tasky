using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using TaskTracker.Application.Abstractions.Services;

namespace TaskTracker.Infrastructure.Services;

public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException("Connection string 'Postgres' not configured.");
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}
