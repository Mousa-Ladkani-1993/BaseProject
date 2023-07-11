
using System.ComponentModel;
using Microsoft.AspNetCore.Builder;

namespace BaseProjectApp.API.Middlewares
{
    public static class PermissionsMiddlewareExtensions
    {
        public static IApplicationBuilder UsePermissions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PermissionsMiddleware>();
        }
    }

}
    