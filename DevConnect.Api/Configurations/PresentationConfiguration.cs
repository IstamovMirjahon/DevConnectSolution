using DevConnect.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.Api.Configurations;

public static class PresentationConfiguration
{
    public static void ConfigureOwnerOperationsPresentation(this IServiceCollection services)
    {
        // CORS konfiguratsiyasi
        services.AddCors(options => options.AddPolicy("AllowCors", builder =>
        {
            builder
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(_ => true)
                .AllowAnyHeader();
        }));

        // Controllers va model validation
        services.AddControllers(config =>
        {
            // Authentication policy kerak bo'lsa shu yerga qo'shish mumkin
        }).ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var fieldErrors = context.ModelState
                    .Where(v => v.Value.Errors.Count > 0)
                    .Select(e => new FieldError(
                        [e.Key],
                        CommonErrorCodes.InvalidValue,
                        e.Value.Errors.First().ErrorMessage
                    ))
                    .ToList();

                return new ObjectResult(new ValidationError(fieldErrors)) { StatusCode = 422 };
            };
        });

        // API Explorer
        services.AddEndpointsApiExplorer();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddProblemDetails();
    }
}