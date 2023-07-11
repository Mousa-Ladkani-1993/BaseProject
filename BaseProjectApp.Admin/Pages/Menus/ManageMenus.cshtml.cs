using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using BaseProjectApp.Library.Templates;
using BaseProjectApp.Library.Templates.DTOs;
using BaseProjectApp.Library.Templates.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using BaseProjectApp.Admin.Middlewares;

namespace BaseProjectApp.Admin.Pages.Menus
{
    public class ManageMenusModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private UserManager<IdentityUser> _userManager;
        private readonly IUnitofWork _repositories = null;
        public Perm _permObj;

        public int canEdit = 0;
        public int canAdd = 0;
        public int canView = 0;
        public int canDelete = 0;

        public List<ListItem> RoleTypesVals { get; set; }

        public ManageMenusModel(IUnitofWork repositories, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _repositories = repositories;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            _permObj = new Perm(RolesNames.Packages, _repositories, this.User.FindFirst(x => x.Value != null).Value);


            if (_permObj != null)
            {
                canAdd = _permObj.canAdd == true ? 1 : 0;
                canEdit = _permObj.canEdit == true ? 1 : 0;
                canView = _permObj.canView == true ? 1 : 0;
                canDelete = _permObj.canDelete == true ? 1 : 0;
            }

            var currentUser = this.User;
            ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
            ViewData["UserId"] = _userManager.GetUserId(currentUser);


            return Page();
        }
    }
}

