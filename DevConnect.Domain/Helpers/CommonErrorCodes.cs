namespace DevConnect.Domain.Helpers;

public class CommonErrorCodes
{
    public const string Unauthorized = "UNAUTHORIZED";
    public const string BadInput = "BAD_INPUT";
    public const string AccessDenied = "ACCESS_DENIED";
    public const string ModelNotFound = "MODEL_NOT_FOUND";
    public const string ServiceError = "SERVICE_ERROR";
    public const string INDIVIDUAL_PROFILES_ONLY = "INDIVIDUAL_PROFILES_ONLY";
    public const string Forbidden = "ACTION_FORBIDDEN";
    public const string Concurrency = "THIS_ACTION_IS_CONCURRENTLY_DOING";
    public const string ProviderNotSupported = "THIS_ACTION_IS_CONCURRENTLY_DOING";
    public const string DashCameraProviderNotFound = "DASH_CAMERA_PROVIDER_NOT_FOUND_FOR_THIS_VEHICLE";

    /// <summary>
    /// The external API field error
    /// </summary>
    public static string ExternalApiFieldError = "EXTERNAL_API_FIELD_ERROR";

    /// <summary>
    /// The external API error
    /// </summary>
    public static string ExternalApiError = "EXTERNAL_API_ERROR";

    /// <summary>
    /// The internal
    /// </summary>
    public static string Internal = "INTERNAL_SERVER_ERROR";

    /// <summary>
    /// Indicates a required field is missing or null.
    /// </summary>
    public static string Required = "REQUIRED";

    /// <summary>
    /// Indicates the submitted field value is unrecognized or doesn't match any existing system entity. Ensure the input corresponds to a valid, registered entity in the system.
    /// </summary>
    public const string ValueDoesNotExist = "VALUE_DOES_NOT_EXIST";

    /// <summary>
    /// Indicates the provided value does not match the expected one.
    /// </summary>
    public static string ValueMustBeEqual = "VALUE_MUST_BE_EQUAL";

    /// <summary>
    /// Indicates that the provided value is of an unexpected or unsupported data type.
    /// </summary>
    public static string InvalidType = "INVALID_TYPE";

    /// <summary>
    /// Indicates that the input provided does not match the expected format or value type as outlined in the API specifications. It could be due to incorrect type, incorrect pattern, or out-of-range values.
    /// </summary>
    public const string InvalidValue = "INVALID_VALUE";

    /// <summary>
    /// Indicates that the provided date value in the request is invalid or not in the correct format.
    /// </summary>
    public static string InvalidDate = "INVALID_DATE";

    /// <summary>
    /// Indicates that the provided email address is not valid due to incorrect format or invalid characters.
    /// </summary>
    public static string InvalidEmail = "INVALID_EMAIL";

    /// <summary>
    /// Indicates that the provided phone number is not valid. This could be due to incorrect format, invalid characters, or incorrect length.
    /// </summary>
    public static string InvalidPhone = "INVALID_PHONE";

    /// <summary>
    /// Indicates that the input value is below the minimum limit. This could apply to numeric values, string lengths, array lengths, file or image sizes (including width and height), or dates.
    /// </summary>
    public static string TooSmall = "TOO_SMALL";

    /// <summary>
    /// Indicates that the provided value exceeds the maximum limit. This could apply to numbers, string lengths, array lengths, file sizes, image dimensions, and dates.
    /// </summary>
    public static string TooBig = "TOO_BIG";

    /// <summary>
    /// Indicates that the provided value is not among the allowed enumeration options.
    /// </summary>
    public static string InvalidEnumValue = "INVALID_ENUM_VALUE";

    /// <summary>
    /// Indicates that one or more input keys provided in the API request are not recognized or supported.
    /// </summary>
    public static string UnrecognizedKeys = "UNRECOGNIZED_KEYS";

    /// <summary>
    /// Indicates that a required string input must include a specific value or pattern.
    /// </summary>
    public static string StringMustInclude = "STRING_MUST_INCLUDE";

    /// <summary>
    /// Indicates that the provided URL in the request is invalid or malformed.
    /// </summary>
    public static string InvalidUrl = "INVALID_URL";

    /// <summary>
    /// Indicates that the provided emoji in the request is invalid or not recognized.
    /// </summary>
    public static string InvalidEmoji = "INVALID_EMOJI";

    /// <summary>
    /// Indicates an invalid Universally Unique Identifier (UUID) in the request.
    /// </summary>
    public static string InvalidUuid = "INVALID_UUID";

    /// <summary>
    /// Indicates that the provided input does not match the required regular expression pattern.
    /// </summary>
    public static string InvalidRegex = "INVALID_REGEX";

    /// <summary>
    /// Indicates that the provided CUID (Client Unique Identifier) in the request is invalid or not recognized.
    /// </summary>
    public static string InvalidCuid = "INVALID_CUID";

    /// <summary>
    /// Indicates that the provided CUID (Client Unique Identifier) in the request is invalid or not recognized.
    /// </summary>
    public static string InvalidCuid2 = "INVALID_CUID_2";

    /// <summary>
    /// Indicates that the provided ULID (Universally Unique Lexicographically Sortable Identifier) in the request is invalid or not recognized.
    /// </summary>
    public static string InvalidUlid = "INVALID_ULID";

    /// <summary>
    /// Indicates that the provided datetime value in the request is invalid or not in the expected format.
    /// </summary>
    public static string InvalidDatetime = "INVALID_DATETIME";

    /// <summary>
    /// Indicates that the provided IP address in the request is invalid or not in the correct format.
    /// </summary>
    public static string InvalidIp = "INVALID_IP";

    /// <summary>
    /// Indicates that the provided date value in the request is not in the required format.
    /// </summary>
    public static string InvalidDateFormat = "INVALID_DATE_FORMAT";

    /// <summary>
    /// Indicates that the field must be accepted, typically used for 'Terms of Service' or similar acceptance fields. Acceptable value is true.
    /// </summary>
    public static string MustBeAccepted = "MUST_BE_ACCEPTED";

    /// <summary>
    /// Indicates that the provided value already exists.
    /// </summary>
    public static string DuplicateValue = "DUPLICATE_VALUE";

    /// <summary>
    /// Indicates that an internal server error occurred.
    /// </summary>
    public static string InternalServerError = "INTERNAL_SERVER_ERROR";

    /// <summary>
    /// Indicates a general validation error.
    /// </summary>
    public static string ValidationError = "VALIDATION_ERROR";

    /// <summary>
    /// The validation error
    /// </summary>
    public static string Expired = "EXPIRED";

    /// <summary>
    /// No support for this user
    /// </summary>
    public static string NoSupportUser = "NO_SUPPORT_FOR_THIS_USER";

    /// <summary>
    /// The value already exists
    /// </summary>
    public static string ValueAlreadyExists = "VALUE_ALREADY_EXISTS";

    /// <summary>
    /// The operation could not be operated
    /// </summary>
    public static string OperationNotCompleted = "OPERATION_COULD_NOT_BE_COMPLETED";
}