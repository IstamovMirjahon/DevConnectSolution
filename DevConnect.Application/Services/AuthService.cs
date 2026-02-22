using DevConnect.Application.Interfaces.Auth;
using DevConnect.Application.Models.Auth.Requests;
using DevConnect.Application.Models.Auth.Response;
using DevConnect.Domain.Entities;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;
using DevConnect.Domain.IRepositories;

namespace DevConnect.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider)
    : IAuthService
{
    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        if (request.Password != request.ConfirmPassword)
            return Result.Fail<AuthResponse>(
                new Error("PASSWORD_MISMATCH", "Passwords do not match"));

        if (await userRepository.ExistsByEmailAsync(request.Email, ct))
            return Result.Fail<AuthResponse>(
                new Error("EMAIL_EXISTS", "Email already registered"));

        var hash = passwordHasher.Hash(request.Password);

        var user = new User(
            request.FullName,
            request.Email,
            hash,
            request.Role,
            request.Profession
        );

        await userRepository.AddAsync(user, ct);

        var token = jwtProvider.Generate(user);

        return Result.Success(new AuthResponse(
            token,
            user.Email,
            user.Role.ToString()
        ));
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await userRepository
            .GetByEmailAsync(request.Email.ToLower(), ct);

        if (user is null)
            return Result.Fail<AuthResponse>(
                new Error("INVALID_CREDENTIALS", "Email or password wrong"));

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result.Fail<AuthResponse>(
                new Error("INVALID_CREDENTIALS", "Email or password wrong"));

        if (user.State != State.Active)
            return Result.Fail<AuthResponse>(
                new Error("USER_INACTIVE", "User is not active"));

        var token = jwtProvider.Generate(user);

        return Result.Success(new AuthResponse(
            token,
            user.Email,
            user.Role.ToString()
        ));
    }
}
