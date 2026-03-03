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
    IUnitOfWork unitOfWork,
    IMemoryCache _memoryCache) : IAuthService
{
    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(request.Email))
            return Result.Fail<AuthResponse>(
                new UserError(ErrorCodes.InvalidEmailFormat, ErrorMessages.InvalidEmailFormat));

        if (await userRepository.ExistsByEmailAsync(request.Email, ct))
            return Result.Fail<AuthResponse>(
                new UserError(ErrorCodes.EmailExists, ErrorMessages.EmailExists));

        var passwordHash = passwordHasher.Hash(request.Password);

        var pendingUser = new PendingRegisterModel
        {
            FullName = request.FullName,
            Email = request.Email.ToLower(),
            PasswordHash = passwordHash,
            Role = request.Role
        };

        _memoryCache.Set(
            $"register_{request.Email}",
            pendingUser,
            TimeSpan.FromMinutes(10));

        await _emailService.SendVerificationCodeAsync(request.Email);

        return Result.Success<AuthResponse>(null!);
    }
    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await userRepository
            .GetByEmailAsync(request.Email.ToLower(), ct);

        if (user is null)
            return Result.Fail<LoginResponse>(
                new UserError(ErrorCodes.InvalidCredentials, ErrorMessages.InvalidCredentials));

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result.Fail<LoginResponse>(
                new UserError(ErrorCodes.InvalidCredentials, ErrorMessages.InvalidCredentials));

        if (user.State != State.Active)
            return Result.Fail<LoginResponse>(
                new UserError(ErrorCodes.UserInactive, ErrorMessages.UserInactive));

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

        await unitOfWork.SaveChangesAsync(ct);

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
            return Result<string>.Fail(new UserError(ErrorCodes.InvalidRefreshToken, ErrorMessages.InvalidRefreshToken));

        if (token.RefreshTokenExpiration < DateTime.UtcNow)
            return Result<string>.Fail(new UserError(ErrorCodes.RefreshTokenExpired, ErrorMessages.RefreshTokenExpired));

        var user = await userRepository
            .GetByIdAsync(token.UserId, ct);

        if (user == null)
            return Result<string>.Fail(new NotFoundError(ErrorCodes.UserNotFound, ErrorMessages.UserNotFound));

        var newAccessToken = jwtProvider.Generate(user);

        token.AccessToken = newAccessToken;
        token.AccessTokenExpiration = DateTime.UtcNow.AddMinutes(120);

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success(newAccessToken);
    }
    public async Task<Result> LogoutAsync(string refreshToken, CancellationToken ct)
    {
        var token = await _userTokenRepository
            .GetByRefreshTokenAsync(refreshToken, ct);

        if (token == null)
            return Result.Fail(new UserError(ErrorCodes.InvalidRefreshToken, ErrorMessages.InvalidRefreshToken));

        token.RefreshToken = string.Empty;
        token.RefreshTokenExpiration = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
    public async Task<Result> RequestPasswordResetAsync(string email, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailAsync(email, ct);

        if (user == null)
            return Result.Fail(new NotFoundError(ErrorCodes.UserNotFound, ErrorMessages.UserNotFound));

        await _emailService.SendResetPasswordCodeAsync(email);
        return Result.Success();
    }
    public async Task<Result> ResetPasswordAsync(string email, string code, string newPassword, CancellationToken ct)
    {
        var isValid = _emailService.VerifyResetPasswordCode(email, code);

        if (!isValid)
            return Result.Fail(new UserError(ErrorCodes.InvalidCode, ErrorMessages.InvalidCode));

        var user = await userRepository.GetByEmailAsync(email, ct);

        if (user == null)
            return Result.Fail(new NotFoundError(ErrorCodes.UserNotFound, ErrorMessages.UserNotFound));

        user.PasswordHash = passwordHasher.Hash(newPassword);

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
    public async Task<Result> VerifyRegisterAsync(string email, string code, CancellationToken ct)
    {
        var isValid = _emailService.VerifyCode(email, code);

        if (!isValid)
            return Result.Fail(
                new UserError(ErrorCodes.InvalidCode, ErrorMessages.InvalidCode));

        if (!_memoryCache.TryGetValue(
            $"register_{email}",
            out PendingRegisterModel? pendingUser))
        {
            return Result.Fail(
                new UserError(ErrorCodes.RegisterExpired, ErrorMessages.RegisterExpired));
        }

        var user = new User(
            pendingUser.FullName,
            pendingUser.Email,
            pendingUser.PasswordHash,
            pendingUser.Role
        );

        await userRepository.AddAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);

        _memoryCache.Remove($"register_{email}");

        return Result.Success();
    }

    public async Task<Result> ResendVerificationCodeAsync(string email, CancellationToken ct)
    {
        if (!_memoryCache.TryGetValue(
            $"register_{email}",
            out PendingRegisterModel? _))
        {
            return Result.Fail(
                new UserError(ErrorCodes.RegisterExpired, ErrorMessages.RegisterExpired));
        }

        await _emailService.SendVerificationCodeAsync(email);

        return Result.Success();
    }
}