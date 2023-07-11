using System;
using System.Threading.Tasks;
using BaseProjectApp.Admin.Middlewares;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace BaseProjectApp.Admin.Pages.LookupValues
{
    public class ManageCompanyLookupValuesModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private UserManager<IdentityUser> _userManager;
        private readonly IUnitofWork _repositories = null;
        public Perm _permObj;

        public ManageCompanyLookupValuesModel(IConfiguration configuration, UserManager<IdentityUser> userManager, IUnitofWork repositories)
        {
            _configuration = configuration;
            _userManager = userManager;
            _repositories = repositories;
        }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                _permObj = new Perm(RolesNames.LOOKUPS, _repositories, this.User.FindFirst(x => x.Value != null).Value);
                var currentUser = this.User;
                ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
                ViewData["UserId"] = _userManager.GetUserId(currentUser);
                ViewData["CompanyLookups"] = new SelectList(await _repositories.CompanyLookups.GetAll(), "Id", "Name");
            }
            catch (Exception ex)
            {
            }

            return Page();
        }
    }
}
