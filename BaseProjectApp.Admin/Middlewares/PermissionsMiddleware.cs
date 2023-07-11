using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using BaseProjectApp.Library.Templates.Enums;
using BaseProjectApp.Admin.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProjectApp.Admin.Middlewares
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

            var _params = ParseQs(context.Request.QueryString.Value);

            var ca = canAccess(userPermissions.ToList(), path, _params, context.Request.Method);

            Console.WriteLine("----------------------------------");
            Console.WriteLine(context.Request.Path.Value + context.Request.QueryString);
            Console.WriteLine($"Can Access = {ca}");
            Console.WriteLine("----------------------------------");


            // Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            // Console.WriteLine(context.Request.QueryString.ToUriComponent());
            // Console.WriteLine(context.Request.Path.Value);
            // Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            // Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            // EndpointPermissionsSet.endpointPermissionsSet.ForEach(x => Console.WriteLine($"{x.path} {x.role.Str()} {x.action.ToString()}"));
            // Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");

            // Console.WriteLine("#############################");
            // userPermissions.ToList().ForEach(x => Console.WriteLine($"{x.Role.Name} {x.CanView} {x.CanAdd} {x.CanEdit} {x.CanDelete}"));
            // Console.WriteLine("#############################");




            if(!canAccess(userPermissions.ToList(), path, _params, context.Request.Method))
            {
                Console.WriteLine("<<<<< Access Denied >>>>>");
                context.Response.Clear();
                context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                await context.Response.WriteAsync($"you do not have permission to make request to {path}");
                return ;
            }


            await _next(context);
            
        }

        private NameValueCollection ParseQs(string s)
        {
            
            var _params =  HttpUtility.ParseQueryString(s);
            return _params;
        }

        private string GetUserId(HttpContext context)
        {
            var userId = context.User.FindFirst(x => x.Value != null).Value;
            return userId;
        }

        private bool canAccess(List<UserPermission> userPermissions, string path, NameValueCollection _params, string httpMethod)
        {
            var urlPermissions = EndpointPermissionsSet.GetAllByPath(path);

            if(urlPermissions.Count == 0)
            {
                return false;
            }

            var hasId = _params["Id"] != null || _params["id"] != null || _params["ID"] != null || _params["iD"] != null;
            string id = null;

            if(hasId)
            {
                id = _params["Id"] != null ? _params["Id"] : _params["id"];
            }

            // Console.WriteLine("LLLLLLLLLLLLLLLLLLLLLLLLLLLL");
            // urlPermissions.ForEach(_ => Console.WriteLine($"{_.role.Str()}"));
            // Console.WriteLine(urlPermissions.Select(x => x.role.Str()).Contains("suppliers"));

            var res = userPermissions.Any(
                up => urlPermissions.Select(x => x.role.Str().Trim().ToLower()).Contains(up.Role.Name.Trim().ToLower()) &&
                      (
                        (httpMethod == "GET" && up.CanEdit == true && urlPermissions.Select(_ => _.ptype).Contains(PageType.ONE) &&
                            hasId && id != "0" && urlPermissions.Select(_ => _.action).Contains(ApiActions.EDIT)) 
                        
                        ||

                        (httpMethod == "GET" && up.CanView == true && urlPermissions.Select(_ => _.ptype).Contains(PageType.ONE) &&
                            hasId && id != "0" && urlPermissions.Select(_ => _.action).Contains(ApiActions.VIEW)) 
                            
                        ||

                        (httpMethod == "GET" && up.CanAdd == true && urlPermissions.Select(_ => _.ptype).Contains(PageType.ONE) &&
                            (!hasId || id == "0") && urlPermissions.Select(_ => _.action).Contains(ApiActions.ADD)) 
                            
                        ||

                        (httpMethod == "GET" && up.CanView == true && urlPermissions.Select(_ => _.ptype).Contains(PageType.ALL) && 
                            urlPermissions.Select(_ => _.action).Contains(ApiActions.VIEW)) 
                            
                        ||

                        (httpMethod == "POST" && (up.CanAdd == true || up.CanEdit == true) && 
                            (urlPermissions.Select(_ => _.action).Contains(ApiActions.ADD) || urlPermissions.Select(_ => _.action).Contains(ApiActions.EDIT)) )
                            
                        ||

                        (httpMethod == "Delete" && up.CanDelete == true && 
                            urlPermissions.Select(_ => _.action).Contains(ApiActions.DELETE))
                        
                    )
            );

            return res;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PermissionAttribute: System.Attribute
    {
        public PermissionAttribute(RolesNames r, ApiActions a, PageType p ,string path)
        {
            
            // Console.WriteLine(">>> " + path);
            EndpointPermissionsSet.Add(path, r, a, p);
        }
        
    }

}