using DevConnect.Domain.Enums;

namespace DevConnect.Application.Models.Auth.Requests;

public record RegisterRequest(
    string FullName,
    string Email,
    string Password,
    string ConfirmPassword,
    Role Role,
    Profession Profession
);