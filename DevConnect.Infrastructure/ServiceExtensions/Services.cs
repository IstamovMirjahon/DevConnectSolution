using DevConnect.Application.Interfaces;
using DevConnect.Application.Interfaces.Auth;
using DevConnect.Domain.IRepositories;
using DevConnect.Infrastructure.Context;
using DevConnect.Infrastructure.Repositories;
using DevConnect.Infrastructure.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevConnect.Infrastructure.ServiceExtensions;

public static class Services
{
    public static IServiceCollection AddDevConnectInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IRecruiterRepository, RecruiterRepository>();
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
    public static IServiceCollection AddDevConnectPersistence(this IServiceCollection services, IConfiguration config)
    {
        var cs = config.GetConnectionString("DefaultConnetion");
        services.AddDbContext<DefaultContext>(opt => opt.UseNpgsql(cs));

        return services;
    }
}