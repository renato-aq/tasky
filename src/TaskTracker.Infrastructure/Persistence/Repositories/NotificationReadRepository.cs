using System.Text.Json;
using Dapper;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;
using TaskTracker.Application.Features.Notifications.DTOs;

namespace TaskTracker.Infrastructure.Persistence.Repositories;

public class NotificationReadRepository : INotificationReadRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public NotificationReadRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<NotificationDto>> GetByUserAsync(Guid userId, CancellationToken ct = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT id, user_id AS UserId, type, payload::text AS Payload, read_at AS ReadAt, created_at AS CreatedAt
            FROM notifications
            WHERE user_id = @UserId
            ORDER BY created_at DESC
            LIMIT 50
            """;

        var rows = await connection.QueryAsync<NotificationRow>(sql, new { UserId = userId });

        return rows.Select(r => new NotificationDto(
            r.Id,
            r.UserId,
            r.Type,
            string.IsNullOrEmpty(r.Payload) ? null : JsonSerializer.Deserialize<JsonElement>(r.Payload),
            r.ReadAt,
            r.CreatedAt));
    }

    private class NotificationRow
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Payload { get; set; } = "{}";
        public DateTime? ReadAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
