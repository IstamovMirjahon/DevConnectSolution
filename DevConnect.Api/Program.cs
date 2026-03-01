using DevConnect.Api.Configurations;
using DevConnect.Application.ResponseSerializer;
using DevConnect.Api.Configurations.Jwt;
using DevConnect.Application.DI;
using DevConnect.Infrastructure.Options;
using DevConnect.Infrastructure.ServiceExtensions;

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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowCors");

app.UseAuthorization();


app.MapControllers();

app.Run();
