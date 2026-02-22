using DevConnect.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DevConnect.Api.Configurations;

public static class HttpContextConfiguration
{
    public static void ConfigureHttpContextAccessor(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
    }
    public static void UseHttpContextHelper(this IApplicationBuilder app)
    {
        if (app.ApplicationServices.GetService<IHttpContextAccessor>() is not null)
            HttpContextHelper.Accessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
    }
}