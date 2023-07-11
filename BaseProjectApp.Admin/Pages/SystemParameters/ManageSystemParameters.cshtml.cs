using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using BaseProjectApp.Library.Templates.Enums;

namespace BaseProjectApp.Admin.Pages.SystemParameters
{
    public class ManageSystemParametersModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private UserManager<IdentityUser> _userManager;


        public int TypeText = (int)SystemParameterType_Enum.Text;
        public int TypeNumber = (int)SystemParameterType_Enum.Number;
        public int TypeDate = (int)SystemParameterType_Enum.Date;
        public int TypeBoolean = (int)SystemParameterType_Enum.Boolean;

        public ManageSystemParametersModel(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public void OnGet()
        {
            var currentUser = this.User;
            ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
            ViewData["UserId"] = _userManager.GetUserId(currentUser);
        }
    }
}
