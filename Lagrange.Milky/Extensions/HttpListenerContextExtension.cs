using System.Net;

namespace Lagrange.Milky.Extensions;

public static class HttpListenerContextExtension
{
    public static void ReturnStatus(this HttpListenerContext context, HttpStatusCode status, string? allowCorsOrigins = null)
    {
        context.Response.StatusCode = (int)status;
        if (allowCorsOrigins != null) context.Response.Headers["Access-Control-Allow-Origin"] = allowCorsOrigins;
        context.Response.Close();
    }
}