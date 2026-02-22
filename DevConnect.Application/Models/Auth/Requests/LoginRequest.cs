namespace DevConnect.Application.Models.Auth.Requests;

public record LoginRequest(
    string Email,
    string Password
);