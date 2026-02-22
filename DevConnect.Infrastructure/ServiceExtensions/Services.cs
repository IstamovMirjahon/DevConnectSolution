using DevConnect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevConnect.Infrastructure.ServiceExtensions;

public static class Services
{
    public static IServiceCollection AddDevConnectInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        return services;
    }
    public static IServiceCollection AddDevConnectPersistence(this IServiceCollection services, IConfiguration config)
    {
        var cs = config.GetConnectionString("DefaultConnetion");
        services.AddDbContext<DefaultContext>(opt => opt.UseNpgsql(cs, npg => npg.UseNetTopologySuite()));

        return services;
    }
}