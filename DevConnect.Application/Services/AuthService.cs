using DevConnect.Application.Interfaces;
using DevConnect.Application.Interfaces.Auth;
using DevConnect.Application.Models.Auth.Requests;
using DevConnect.Application.Models.Auth.Response;
using DevConnect.Domain.Entities;
using DevConnect.Domain.Enums;
using DevConnect.Domain.Helpers;
using DevConnect.Domain.IRepositories;
using Microsoft.Extensions.Caching.Memory;

namespace DevConnect.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider,
    IEmailService _emailService,
    IUserTokenRepository _userTokenRepository,
    IMemoryCache _memoryCache) : IAuthService
{
    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        if (request.Password != request.ConfirmPassword)
            return Result.Fail<AuthResponse>(
                new Error("PASSWORD_MISMATCH", "Passwords do not match"));

        if (await userRepository.ExistsByEmailAsync(request.Email, ct))
            return Result.Fail<AuthResponse>(
                new Error("EMAIL_EXISTS", "Email already registered"));

        var passwordHash = passwordHasher.Hash(request.Password);

        var pendingUser = new PendingRegisterModel
        {
            FullName = request.FullName,
            Email = request.Email.ToLower(),
            PasswordHash = passwordHash,
            Role = request.Role,
            Profession = request.Profession
        };

        _memoryCache.Set(
            $"register_{request.Email}",
            pendingUser,
            TimeSpan.FromMinutes(2));

        await _emailService.SendVerificationCodeAsync(request.Email);

        return Result.Success<AuthResponse>(null!);
    }
    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await userRepository
            .GetByEmailAsync(request.Email.ToLower(), ct);

        if (user is null)
            return Result.Fail<LoginResponse>(
                new Error("INVALID_CREDENTIALS", "Email or password wrong"));

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result.Fail<LoginResponse>(
                new Error("INVALID_CREDENTIALS", "Email or password wrong"));

        if (user.State != State.Active)
            return Result.Fail<LoginResponse>(
                new Error("USER_INACTIVE", "User is not active"));

        var accessToken = jwtProvider.Generate(user);
        var refreshToken = jwtProvider.GenerateRefreshToken();

        var userToken = await _userTokenRepository
            .GetByUserIdAsync(user.Id, ct);

        if (userToken == null)
        {
            userToken = new UserToken
            {
                UserId = user.Id
            };

            await _userTokenRepository.AddAsync(userToken, ct);
        }

        userToken.AccessToken = accessToken;
        userToken.RefreshToken = refreshToken;
        userToken.AccessTokenExpiration = DateTime.UtcNow.AddMinutes(120);
        userToken.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

        await _userTokenRepository.SaveChangesAsync(ct);

        var response = new LoginResponse
        {
            Id = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            UserType = user.Type
        };
        return Result.Success(response);
    }
    public async Task<Result<string>> RefreshAsync(string refreshToken, CancellationToken ct)
    {
        var token = await _userTokenRepository
            .GetByRefreshTokenAsync(refreshToken, ct);

        if (token == null)
            throw new UnauthorizedAccessException("Invalid refresh token");

        if (token.RefreshTokenExpiration < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token expired");

        var user = await userRepository
            .GetByIdAsync(token.UserId, ct);

        if (user == null)
            throw new UnauthorizedAccessException("User not found");

        var newAccessToken = jwtProvider.Generate(user);

        token.AccessToken = newAccessToken;
        token.AccessTokenExpiration = DateTime.UtcNow.AddMinutes(120);

        await _userTokenRepository.SaveChangesAsync(ct);

        return Result.Success(newAccessToken);
    }
    public async Task<Result> LogoutAsync(string refreshToken, CancellationToken ct)
    {
        var token = await _userTokenRepository
            .GetByRefreshTokenAsync(refreshToken, ct);

        if (token == null)
            return Result.Fail(new Error("INVALID_REFRESH_TOKEN", "Invalid refresh token"));

        token.RefreshToken = string.Empty;
        token.RefreshTokenExpiration = DateTime.UtcNow;

        await _userTokenRepository.SaveChangesAsync(ct);
        return Result.Success();
    }
    public async Task RequestPasswordResetAsync(string email, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailAsync(email, ct);

        if (user == null)
            throw new Exception("User not found");

        await _emailService.SendResetPasswordCodeAsync(email);
    }
    public async Task ResetPasswordAsync(string email, string code, string newPassword, CancellationToken ct)
    {
        var isValid = _emailService.VerifyResetPasswordCode(email, code);

        if (!isValid)
            throw new Exception("Invalid or expired code");

        var user = await userRepository.GetByEmailAsync(email, ct);

        if (user == null)
            throw new Exception("User not found");

        user.PasswordHash = passwordHasher.Hash(newPassword);

        await userRepository.SaveChangesAsync(ct);
    }
    public async Task<Result> VerifyRegisterAsync(string email, string code, CancellationToken ct)
    {
        var isValid = _emailService.VerifyCode(email, code);

        if (!isValid)
            return Result.Fail(
                new Error("INVALID_CODE", "Invalid or expired code"));

        if (!_memoryCache.TryGetValue(
            $"register_{email}",
            out PendingRegisterModel? pendingUser))
        {
            return Result.Fail(
                new Error("REGISTER_EXPIRED", "Registration expired"));
        }

        var user = new User(
            pendingUser.FullName,
            pendingUser.Email,
            pendingUser.PasswordHash,
            pendingUser.Role,
            pendingUser.Profession
        );

        await userRepository.AddAsync(user, ct);
        await userRepository.SaveChangesAsync(ct);

        _memoryCache.Remove($"register_{email}");

        return Result.Success();
    }
}