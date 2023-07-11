using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace BaseProjectApp.Admin.Pages.Users
{
    public class ManageUsersModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ManageUsersModel(IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            var currentUser = this.User;
            ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
            ViewData["UserId"] = _userManager.GetUserId(currentUser);

            return Page();
        }

        public async Task<JsonResult> OnPostLockAccount(string Id)
        {
            var _user = await _userManager.FindByIdAsync(Id);

            _user.LockoutEnabled = true;
            _user.LockoutEnd = DateTime.Now.AddYears(1000);

            var res = await _userManager.UpdateAsync(_user);

            await _userManager.UpdateSecurityStampAsync(_user);
            
            if(res.Succeeded)
            {
                return new JsonResult(new {success=true});
            }
            else
            {
                return new JsonResult(new {
                    success=false, 
                    errorMsg=res.Errors.Select(e => e.Description).Aggregate((a, b) => string.Join("\n", a, b))
                });
            }
        }

        public async Task<JsonResult> OnPostUnlockAccount(string Id)
        {

            var _user = await _userManager.FindByIdAsync(Id);

            _user.LockoutEnd = null;

            var res = await _userManager.UpdateAsync(_user);
            
            if(res.Succeeded)
            {
                return new JsonResult(new {success=true});
            }
            else
            {
                return new JsonResult(new {
                    success=false, 
                    errorMsg=res.Errors.Select(e => e.Description).Aggregate((a, b) => string.Join("\n", a, b))
                });
            }

            
        }


    }
}
