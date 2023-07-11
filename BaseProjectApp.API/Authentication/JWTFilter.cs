using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BaseProjectApp.API.Authentication
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class JWTFilter : Attribute, IAsyncActionFilter
    {
        private const string Key = "APIKey";
        private const string ValidAudience = "JWT:ValidAudience";
        private const string ValidIssuer = "JWT:ValidIssuer";
        private const string Secret = "JWT:Secret";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
             
            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            if (!string.IsNullOrWhiteSpace(context.HttpContext.Request.Headers["Authorization"].ToString()))
            {
                if (!TokenManager.ValidateToken(context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                     , appSettings.GetValue<string>(Secret), appSettings.GetValue<string>(ValidIssuer), appSettings.GetValue<string>(ValidAudience)))
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 401,
                        Content = "Token is invalid"
                    };

                    return;
                }
            }
            else {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Token is required"
                };

                return;
            }

            await next();
        }
    }
}

