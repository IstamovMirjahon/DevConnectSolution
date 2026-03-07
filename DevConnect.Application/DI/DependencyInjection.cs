using DevConnect.Application.Interfaces;
using DevConnect.Application.Interfaces.Auth;
using DevConnect.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DevConnect.Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRecruiterService, RecruiterService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILogCheckerCompanyService, LogCheckerCompanyService>();
        services.AddScoped<ILogCheckerDeveloperService, LogCheckerDeveloperService>();
        return services;
    }
}