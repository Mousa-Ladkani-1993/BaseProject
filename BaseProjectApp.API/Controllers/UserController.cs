using BaseProjectApp.API.Middlewares;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BaseProjectApp.Library.Templates.Responses;
using BaseProjectApp.API.Authorization;
using BaseProjectApp.Library.Utility;

namespace BaseProjectApp.API.Controllers
{
    [Authorize]
    [Route("Api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitofWork repositories = null;
        private readonly UserManager<IdentityUser> _userManager;
        private Guid UserId;
        private AspNetUser _User;
        private readonly IMapper _mapper;

        public UserController(IUnitofWork repositories, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            this.repositories = repositories;
            _mapper = mapper;
            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = httpContextAccessor.HttpContext.User.FindFirst(x => x.Type.Contains("UserId")).Value;
                UserId = Guid.Parse(userId);
                _User = repositories.AspNetUsers.GetAllById(u => u.Id == userId).FirstOrDefault();
            }
        }

        [ApiVersion("1.0")]
        [HttpGet]
        [Route("Get")]
        [Permission(RolesNames.USERS, ApiActions.VIEW, "User/Get")]
        [Authorize(Auth_Permissions.Users.CanViewUsers)]
        public IActionResult Get(string id)
        {
            var roles = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();

            if (roles == null)
                return NotFound();

            return Ok(roles);
        }

          

        [ApiVersion("1.0")]
        [HttpGet]
        [Route("All")]
        [Permission(RolesNames.USERS, ApiActions.VIEW, "User/GetAll")]
        [Authorize(Auth_Permissions.Users.CanViewUsers)]
        public async Task<IActionResult> GetAll(int PageSize = 10, int PageNumber = 1)
        {
             
            var Data = await repositories.AspNetUsers.SelectAllPagging(
               rec => new UserResponse
               {
                   Id = rec.Id,
                   UserName = rec.UserName,
                   Email = rec.Email == null ? "" : rec.Email,
                   LockoutEnd = rec.LockoutEnd,
                   PortalUser = true, 
               }  
               , parameterPagination: new ParameterPagination { PageNumber = PageNumber, PageSize = PageSize });


            if (Data == null)
            { return NotFound(); }


            return Ok(Data);

        }

        [ApiVersion("1.0")]
        [HttpGet]
        [Route("AllTotal")]
        [Permission(RolesNames.Properties, ApiActions.VIEW, "Property/GetAll")]
        //[Authorize(Auth_Permissions.Properties.CanViewProperties)]
        public async Task<IActionResult> GetAllAllTotal()
        {
            var Data = repositories.AspNetUsers.Count(s => s.Id != null);

            if (Data == null)
            { return NotFound(); }

            return Ok(Data);

        }


        [ApiVersion("1.0")]
        [HttpGet]
        [Route("GetAllWithSearchByUsername")]
        [Permission(RolesNames.USERS, ApiActions.VIEW, "User/GetAllWithSearchByUsername")]
        [Authorize(Auth_Permissions.Users.CanViewUsers)]
        public async Task<IActionResult> GetAllWithSearchByUsername(string searchQuery)
        {

            searchQuery = searchQuery != null ? searchQuery.Trim().ToLower() : "";

            var users = (await repositories.AspNetUsers.GetAll())
                .Where(u => u.UserName.ToLower().Contains(searchQuery))
            .ToList();

            if (users == null)
                return NotFound();

            return Ok(users);
        }


        [ApiVersion("1.0")]
        [HttpPost]
        [Route("Save")]
        [Permission(RolesNames.USERS, ApiActions.ADD, "User/Save")]
        [Authorize(Auth_Permissions.Users.CanAddUsers)]
        public async Task<IActionResult> CreateUser(string name)
        {
            IdentityResult result = await _userManager.CreateAsync(new IdentityUser(name));

            if (result.Succeeded)
                return NoContent();
            else
                return NotFound();
        }

        [ApiVersion("1.0")]
        [HttpDelete]
        [Permission(RolesNames.USERS, ApiActions.DELETE, "User/Delete")]
        [Authorize(Auth_Permissions.Users.CanDeleteUsers)]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                bool DeleteRes = false;
                bool DeleteUserRolesRes = await repositories.IdentityRepo.DeleteUserRoles(id);
                 
                repositories.UserLogs.Delete(x => x.UserId == user.Id);
                repositories.UserPermissions.Delete(x => x.UserId == user.Id);

                DeleteRes = (await repositories.Save(UserId)).Item1;

                if (DeleteRes)
                {
                    try
                    {
                        IdentityResult result = await _userManager.DeleteAsync(user);

                        if (result.Succeeded)
                            return NoContent();
                    }
                    catch (Exception ex)
                    { }
                }

                return NotFound();
            }
            else
                return NotFound();
        }


        [ApiVersion("1.0")]
        [HttpGet]
        [Route("UserPermissions")]
        [Permission(RolesNames.USERS, ApiActions.VIEW, "User/GetUserPermissions")]
        [Authorize(Auth_Permissions.Users.CanViewUsers)]
        public async Task<IActionResult> GetUserPermissions(string UserId)
        {
            var userPermissions = await repositories.UserPermissions.GetAll(up => up.UserId == UserId, null, new string[] { "Role" });

            if (userPermissions == null)
            {
                return NotFound();
            }

            var res = userPermissions.Select(up => new { Id = up.Id, RoleName = up.Role?.Name, View = up.CanView, Add = up.CanAdd, Edit = up.CanEdit, Delete = up.CanDelete });
            return Ok(res);
        }


        [ApiVersion("1.0")]
        [Authorize(Auth_Permissions.Users.CanEditUsers)]
        [HttpPut]
        [Route("UnlockAccount")]
        public async Task<IActionResult> OnPostUnlockAccount(string Id)
        {

            var _user = await _userManager.FindByIdAsync(Id);

            _user.LockoutEnd = null;

            var res = await _userManager.UpdateAsync(_user);

            if (res.Succeeded)
            {
                return Ok(new JsonResult(new { success = true }));
            }
            else
            {
                return Ok(new JsonResult(new
                {
                    success = false,
                    errorMsg = res.Errors.Select(e => e.Description).Aggregate((a, b) => string.Join("\n", a, b))
                }));
            }


        } 


        [ApiVersion("1.0")]
        [Authorize(Auth_Permissions.Users.CanEditUsers)]
        [HttpPut]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string Id)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string Schars = ".!$%&*";
            Random rand = new Random();
            int num_chars = rand.Next(0, chars.Length);
            int num_Schars = rand.Next(0, Schars.Length);


            IdentityUser user = await _userManager.FindByIdAsync(Id);
            string newPassword = RandomPassword.Generate(6);
            newPassword += chars[num_chars];
            newPassword += Schars[num_Schars];

            string PasswordHash = _userManager.PasswordHasher.HashPassword(user, newPassword);

            var _User = repositories.AspNetUsers.GetByIdString(Id);
            _User.PasswordHash = PasswordHash;
            repositories.AspNetUsers.Update(_User);
            var result = await repositories.Save();
             

            if (result.Item1 == true)
            {
                return Ok(newPassword);
            }

            return BadRequest();

        }


        [ApiVersion("1.0")]
        [Authorize(Auth_Permissions.Users.CanEditUsers)]
        [HttpPut]
        [Route("LockAccount")]
        public async Task<IActionResult> OnPostLockAccount(string Id)
        {
            var _user = await _userManager.FindByIdAsync(Id);

            _user.LockoutEnabled = true;
            _user.LockoutEnd = DateTime.Now.AddYears(1000);

            var res = await _userManager.UpdateAsync(_user);

            await _userManager.UpdateSecurityStampAsync(_user);

            if (res.Succeeded)
            {
                return Ok(new JsonResult(new { success = true }));
            }
            else
            {
                return Ok(new JsonResult(new
                {
                    success = false,
                    errorMsg = res.Errors.Select(e => e.Description).Aggregate((a, b) => string.Join("\n", a, b))
                }));
            }
        }
    }
}
