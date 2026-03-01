namespace DevConnect.Domain.Helpers;

public static class ErrorMessages
{
    public const string PasswordMismatch = "Passwords do not match.";
    public const string EmailExists = "Email is already registered.";
    public const string InvalidCredentials = "Email or password is incorrect.";
    public const string UserInactive = "User is not active.";
    public const string InvalidRefreshToken = "Invalid refresh token.";
    public const string InvalidCode = "Invalid or expired code.";
    public const string RegisterExpired = "Registration has expired.";
    public const string UserNotFound = "User not found.";
    public const string RecruiterExists = "Recruiter profile already exists for this user.";
    public const string RecruiterNotFound = "Recruiter profile not found.";
    public const string IncorrectCurrentPassword = "Incorrect current password.";
    public const string InvalidEmailFormat = "Invalid email format.";
    public const string RefreshTokenExpired = "Refresh token expired.";
}
