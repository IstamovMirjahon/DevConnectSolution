using DevConnect.Infrastructure.Authentication.Jwt;
using Microsoft.Extensions.Options;

namespace DevConnect.Api.Configurations.Jwt;

public class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "Jwt";
    public void Configure(JwtOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}