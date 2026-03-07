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
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("AllowCors");

app.UseAuthorization();


app.MapControllers();

app.Run();
