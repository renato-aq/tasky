using System.Data;

namespace TaskTracker.Application.Abstractions.Services;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
