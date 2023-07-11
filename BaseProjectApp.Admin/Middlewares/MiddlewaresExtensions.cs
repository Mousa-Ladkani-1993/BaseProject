
using System.ComponentModel;
using BaseProjectApp.Admin.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace BaseProjectApp.Admin.Middlewares
{
    public static class PermissionsMiddlewareExtensions
    {
        public static IApplicationBuilder UsePermissions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PermissionsMiddleware>();
        }
    }

}
    