using AutoMapper;
using BaseProjectApp.API.Authentication;
using BaseProjectApp.API.Middlewares;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Templates.DTOs;
using BaseProjectApp.Library.Templates.Responses;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BaseProjectApp.Library.Utility;
using Microsoft.AspNetCore.Identity;
using BaseProjectApp.API.Authorization;

namespace BaseProjectApp.API.Controllers
{
    [Authorize]
    [Route("Api/Roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        RoleManager<IdentityRole> _roleManager;
        private readonly IUnitofWork repositories = null;
        private Guid UserId;
        public RoleController(IUnitofWork repositories, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            this.repositories = repositories;
            _roleManager = roleManager;

            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = httpContextAccessor.HttpContext.User.FindFirst(x => x.Type.Contains("UserId")).Value;
                UserId = Guid.Parse(userId);
            }
        }

        [HttpGet]
        [Permission(RolesNames.ROLES, ApiActions.VIEW, "Role/Get")]
        [Authorize(Auth_Permissions.Roles.CanViewRoles)]
        public IActionResult Get(string id)
        {
            var roles = _roleManager.Roles.Where(x => x.Id == id).FirstOrDefault();

            if (roles == null)
                return NotFound();

            return Ok(roles);
        }

        [HttpGet]
        [Route("All")]
        [Permission(RolesNames.ROLES, ApiActions.VIEW, "Role/GetAll")]
        [Authorize(Auth_Permissions.Roles.CanViewRoles)]
        public async Task<IActionResult> GetAll()
        {
            //var roles = _roleManager.Roles.ToList(); 
            //if (roles == null)
            //return NotFound(); 
            //return Ok(roles);
             
             return Ok(await repositories.AspNetRoles.SelectAll(
             rec => new RoleRes
             {
                 Id = rec.Id,
                 Name = rec.Name
             }));

        }

        [HttpPut]
        [Permission(RolesNames.ROLES, ApiActions.ADD, "Role/Save")]
        [Authorize(Auth_Permissions.Roles.CanAddRoles)]
        public async Task<IActionResult> CreateRole([FromBodyAttribute] RoleResponse Obj)
        {
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(Obj.name));


            if (result.Succeeded)
            {

                if (!string.IsNullOrWhiteSpace(Obj.Code) || !string.IsNullOrWhiteSpace(Obj.Section) || !string.IsNullOrWhiteSpace(Obj.IconClass))
                {
                    var TargetRole = await repositories.AspNetRoles.GetFirst(s => s.Name != null && s.Name.ToLower() == Obj.name.ToLower());

                    if (TargetRole != null)
                    {
                        TargetRole.Code = Obj.Code;
                        TargetRole.SectionName = Obj.Section;
                        TargetRole.CssClassName = Obj.IconClass;

                        repositories.AspNetRoles.Update(TargetRole);
                        await repositories.Save(UserId);
                    }

                    return NoContent();
                }

                return NoContent();
            }

            else
                return NotFound();
        }

        [HttpDelete]
        [Permission(RolesNames.ROLES, ApiActions.DELETE, "Role/Delete")]
        [Authorize(Auth_Permissions.Roles.CanDeleteRoles)]
        public async Task<IActionResult> Delete(string id)
        {
            bool? RelatedRemoved = true;

            IdentityRole role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var Claims = await _roleManager.GetClaimsAsync(role);

                foreach (var Claim in Claims)
                {
                    RelatedRemoved = (await _roleManager.RemoveClaimAsync(role, Claim))?.Succeeded;
                }

                if (RelatedRemoved == true)
                {
                    repositories.UserPermissions.Delete(s => s.RoleId == role.Id);
                    RelatedRemoved = (await repositories.Save(UserId))?.Item1;
                }

                if (RelatedRemoved == false || RelatedRemoved == null)
                    return NotFound();

                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return NoContent();
                else
                    return NotFound();
            }
            else
                return NotFound();
        }
    }
}
