using DevConnect.Domain.Entities;

namespace DevConnect.Application.Interfaces.Auth;

public interface IJwtProvider
{
    string Generate(User user);
}