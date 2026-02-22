using Microsoft.AspNetCore.Http;

namespace DevConnect.Infrastructure.Helpers;

public class HttpContextHelper
{
    private static IHttpContextAccessor accessor = null!;
    public static IHttpContextAccessor Accessor { get => accessor; set => accessor = value; }
    public static HttpContext Current => Accessor.HttpContext;
}
