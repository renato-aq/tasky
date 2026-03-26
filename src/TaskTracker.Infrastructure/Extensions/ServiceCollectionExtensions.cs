using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Application.Abstractions.CQRS;
using TaskTracker.Application.Abstractions.Repositories;
using TaskTracker.Application.Abstractions.Services;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Messaging;
using TaskTracker.Infrastructure.Persistence;
using TaskTracker.Infrastructure.Persistence.Context;
using TaskTracker.Infrastructure.Persistence.Repositories;
using TaskTracker.Infrastructure.Services;

namespace TaskTracker.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres"))
                   .UseSnakeCaseNamingConvention());

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddSingleton<IDbConnectionFactory, NpgsqlConnectionFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IOrganizationReadRepository, OrganizationReadRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<ITeamReadRepository, TeamReadRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IDispatcher, Dispatcher>();

        services.AddHandlersFromAssembly(typeof(Application.Features.Auth.Commands.Register.RegisterCommandHandler).Assembly);

        return services;
    }

    private static IServiceCollection AddHandlersFromAssembly(
        this IServiceCollection services,
        Assembly assembly)
    {
        var handlerInterfaces = new[]
        {
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>),
            typeof(IQueryHandler<,>)
        };

        var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                handlerInterfaces.Contains(i.GetGenericTypeDefinition())));

        foreach (var handlerType in handlerTypes)
        {
            foreach (var iface in handlerType.GetInterfaces()
                .Where(i => i.IsGenericType &&
                    handlerInterfaces.Contains(i.GetGenericTypeDefinition())))
            {
                services.AddScoped(iface, handlerType);
            }
        }

        return services;
    }
}
