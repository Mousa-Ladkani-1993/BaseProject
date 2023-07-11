using Microsoft.Extensions.Configuration;
using BaseProjectApp.API.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using BaseProjectApp.API.Middlewares;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using System.Text;
using System.Collections.Generic;
using BaseProjectApp.API.Authorization;
using BaseProjectApp.Library.Templates.Enums;
using Microsoft.AspNetCore.Authorization;

namespace BaseProjectApp.API.Controllers
{
    [Route("Api/Authenticate")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitofWork repositories = null;

        public AuthenticateController(IUnitofWork repositories, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.repositories = repositories;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ValidateUser")]
        [Permission(RolesNames.USERS, ApiActions.VIEW, "Authenticate/ValidateUser")]
        public async Task<ActionResult> ValidateUser(string UserEmail, string Password)
        {
            try
            {
                IdentityUser user = await _userManager.FindByEmailAsync(UserEmail);
                bool isExisted = await _userManager.CheckPasswordAsync(user, Password);

                return Ok(isExisted);
            }
            catch (Exception e)
            {
                return Ok(false);
            }
        }

        [HttpGet]
        [Route("GenerateNewToken")]  
        public async Task<ActionResult> GenerateNewTokenAsync(string UserId, string UserEmail) 
        { 
            var Permissions = await repositories.UserPermissions.GetAll(s => s.UserId == UserId , includeExpressions: new[] { "Role" });

            List<string> PermissionsList = new List<string>();

            foreach (var Permission in Permissions)
            {
                if (Permission.CanView == true)
                    PermissionsList.Add(PermissionsNum.View + (Permission.Role?.Code));

                if (Permission.CanAdd == true)
                    PermissionsList.Add(PermissionsNum.Add + (Permission.Role?.Code));

                if (Permission.CanDelete == true)
                    PermissionsList.Add(PermissionsNum.Delete + (Permission.Role?.Code));

                if (Permission.CanEdit == true)
                    PermissionsList.Add(PermissionsNum.Edit + (Permission.Role?.Code));  

            }

            string Secret = _configuration["JWT:Secret"];
            string Issuer = _configuration["JWT:ValidIssuer"];
            string Audience = _configuration["JWT:ValidAudience"];

            bool Admin = true; 

            string token = TokenManager.GenerateToken(UserId, UserEmail, Admin, Secret, Issuer, Audience,string.Join(",",PermissionsList));


            return Ok(token);
        }

    }
}
