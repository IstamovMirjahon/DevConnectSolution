namespace DevConnect.Domain.Helpers;

public static class ErrorCodes
{
    public const string PasswordMismatch = "PASSWORD_MISMATCH";
    public const string EmailExists = "EMAIL_EXISTS";
    public const string InvalidCredentials = "INVALID_CREDENTIALS";
    public const string UserInactive = "USER_INACTIVE";
    public const string InvalidRefreshToken = "INVALID_REFRESH_TOKEN";
    public const string InvalidCode = "INVALID_CODE";
    public const string RegisterExpired = "REGISTER_EXPIRED";
    public const string UserNotFound = "USER_NOT_FOUND";
    public const string RecruiterExists = "RECRUITER_EXISTS";
    public const string RecruiterNotFound = "RECRUITER_NOT_FOUND";
    public const string InvalidEmailFormat = "INVALID_EMAIL_FORMAT";
    public const string RefreshTokenExpired = "REFRESH_TOKEN_EXPIRED";
    public const string InvalidUserType = "INVALID_USER_TYPE";
    public const string UserTypeUpdateForbidden = "USER_TYPE_UPDATE_FORBIDDEN";
}
