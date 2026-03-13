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
    public const string InvalidUserType = "Invalid user type value.";
    public const string UserTypeUpdateForbidden = "You do not have permission to update user type.";
    public const string UserAlreadyHasType = "User already has this type.";
    public const string LogCheckerUsersNotFound = "No users found for the specified criteria.";
    public const string InvalidCvFile           = "Only PDF files are supported.";
    public const string AiAnalysisFailed        = "Failed to analyze CV with AI. Please try again.";
}
