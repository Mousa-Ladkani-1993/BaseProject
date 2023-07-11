using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseProjectApp.Library.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
 

namespace BaseProjectApp.API.Authorization
{ 

    internal class PermissionsAuthorizationHandler : AuthorizationHandler<PermissionsRequirement>
    {
        //UserManager<ApplicationUser> _userManager;
        private readonly UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager; 

        public PermissionsAuthorizationHandler(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirement requirement)
        {
            string[] APIPermessions = requirement.Permission.Split(',');

            try
            {
              if (context.User == null)
              {
                return;
              } 

                string[] Permessions;
                bool Authorized = false;

                foreach (var Claim in context.User.Claims)
                {
                    if (Claim.Type == CustomJWTClaimTypes.Permessions)
                    {
                        Permessions = Claim.Value.Split(',');
                        Authorized = Permessions.Intersect(APIPermessions).Any();
                        //need lower  
                    }  
                }

                if (Authorized)
                    context.Succeed(requirement); 

                return;
            }

            catch (Exception ex)
            {
                return; 
            }

            
        }
    }

}
