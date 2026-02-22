namespace DevConnect.Application.Models.Auth.Response;

public record AuthResponse(
    string AccessToken,
    string Email,
    string Role
);