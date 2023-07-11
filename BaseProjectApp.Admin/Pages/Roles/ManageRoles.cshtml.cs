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

namespace BaseProjectApp.Admin.Pages.Roles
{
    public class ManageRolesModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private UserManager<IdentityUser> _userManager;
        private readonly IUnitofWork _repositories = null;

        public List<ListItem> RoleTypesVals { get; set; }

        public ManageRolesModel(IUnitofWork repositories, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _repositories = repositories;
            _configuration = configuration;
            _userManager = userManager;
        }
         
        public string SectionsObj { get; set; }
        public string ClassesObj { get; set; }
        

        public async Task<IActionResult> OnGet()
        {
            var currentUser = this.User;
            ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
            ViewData["UserId"] = _userManager.GetUserId(currentUser);


          var Data = await _repositories.AspNetRoles.SelectAll(s => new { s.SectionName, s.CssClassName });

        
            var Sections = Data.Select(s=>s.SectionName).Distinct();
            SectionsObj = Sections != null ? JsonConvert.SerializeObject(Sections) : JsonConvert.SerializeObject(new List<string>());

            var Classes = Data.Select(s => s.CssClassName).Distinct();
            ClassesObj = Classes != null ? JsonConvert.SerializeObject(Classes) : JsonConvert.SerializeObject(new List<string>());



            return Page();
        }
    }
}
