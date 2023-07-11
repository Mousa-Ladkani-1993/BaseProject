using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using BaseProjectApp.Library.Templates.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProjectApp.API.Middlewares
{

    class PermissionsMiddleware
    {
        private readonly RequestDelegate _next;
        public PermissionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context, IUnitofWork repository)
        {
            
            
            var userId = GetUserId(context);
            var userPermissions = await repository.UserPermissions.GetAll(up => up.UserId == userId, null, new string[] {"Role"});


            var path = context.Request.Path.HasValue ? 
            context.Request.Path.Value.Split("/").Reverse().Take(2).Reverse().Aggregate((s1, s2) => $"{s1}/{s2}") : 
            "";

            if(!canAccess(userPermissions.ToList(), path))
            {
                Console.WriteLine("<<<<< Access Denied >>>>>");
                context.Response.Clear();
                context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                await context.Response.WriteAsync($"you do not have permission to make request to {path}");
                return ;
            }


            await _next(context);
            
        }


        private string GetUserId(HttpContext context)
        {
            var userId = context.User.FindFirst(x => x.Type.Contains("UserId"))?.Value;
            return userId;
        }

        private bool canAccess(List<UserPermission> userPermissions, string _path)
        {
            var (path, role, action) = EndpointPermissionsSet.GetByPath(_path);
            // Console.WriteLine($"{path} {role.Str()} {action.ToString()}");

            if(path == null)
            {
                return false;
            }

            var res = userPermissions.Any(
                up => up.Role.Name.Trim().ToLower() == role.Str().Trim().ToLower() &&
                      (
                          (action == ApiActions.VIEW && up.CanView == true) ||
                          (action == ApiActions.ADD && up.CanAdd == true) ||
                          (action == ApiActions.EDIT && up.CanEdit == true) ||
                          (action == ApiActions.DELETE && up.CanDelete == true)
                      )
            );

            return res;
        }
    }

    public class PermissionAttribute: System.Attribute
    {
        public PermissionAttribute(RolesNames r, ApiActions a, string path)
        {
            EndpointPermissionsSet.Add(path, r, a);
        }
        
    }

}