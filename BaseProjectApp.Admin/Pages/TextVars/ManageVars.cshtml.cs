using BaseProjectApp.Admin.Middlewares;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using BaseProjectApp.Library.Templates;
using BaseProjectApp.Library.Templates.Enums;
using BaseProjectApp.Library.Templates.Responses;
using BaseProjectApp.Library.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using BaseProjectApp.Library.Templates.DTOs;

namespace EDP.Admin.Pages.TextVars
{
    public class ManageVarsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private UserManager<IdentityUser> _userManager;
        private readonly IUnitofWork _repositories = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Perm _permObj; 

        public ManageVarsModel(  IConfiguration configuration, UserManager<IdentityUser> userManager, IUnitofWork repositories, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _userManager = userManager;
            _repositories = repositories;
            _webHostEnvironment = webHostEnvironment; 
        }


        public async Task OnGetAsync()
        {
            _permObj = new Perm(RolesNames.TEXT_VARIABLES, _repositories, this.User.FindFirst(x => x.Value != null).Value);

            var currentUser = this.User;
            ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
            ViewData["UserId"] = _userManager.GetUserId(currentUser);

        }
    }
}
