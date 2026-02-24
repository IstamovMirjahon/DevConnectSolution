namespace DevConnect.Application.Models.Auth.Response;

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    string Email,
    string Role
);