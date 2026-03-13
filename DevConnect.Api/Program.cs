using DevConnect.Api.Configurations;
using DevConnect.Application.ResponseSerializer;
using DevConnect.Api.Configurations.Jwt;
using DevConnect.Application.DI;
using DevConnect.Application.Interfaces;
using DevConnect.Api.Services;
using DevConnect.Infrastructure.Options;
using DevConnect.Infrastructure.ServiceExtensions;
using DevConnect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureJwtService();
builder.Services.AddAuthorization();
builder.Services.ConfigureOwnerOperationsPresentation();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureHttpContextAccessor();

builder.Services.AddDevConnectInfrastructure(builder.Configuration);
builder.Services.AddDevConnectPersistence(builder.Configuration);
builder.Services.AddApplicationServices();

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<DevConnectResponseSerializer>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddHttpClient();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DefaultContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}
// Swagger productionda ham ishlasin
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevConnect API V1");
    c.RoutePrefix = "swagger";
});

// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("AllowCors");

// Authentication bo'lsa authorizationdan oldin turishi kerak
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Root ochilganda swaggerga yo'naltirsin
app.MapGet("/", () => Results.Redirect("/swagger")).AllowAnonymous();

app.Run();
