using BaseProjectApp.Admin.Middlewares;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace BaseProjectApp.Admin.Pages.MediaFiles
{
    [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, PageType.ALL, "MediaFiles/ManageMediaFiles")]
    public class ManageMediaFilesModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private UserManager<IdentityUser> _userManager;
        private readonly IUnitofWork _repositories = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Perm _permObj;

        public ManageMediaFilesModel(IConfiguration configuration, UserManager<IdentityUser> userManager, IUnitofWork repositories, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _userManager = userManager;
            _repositories = repositories;
            _webHostEnvironment = webHostEnvironment;
        }

        public void OnGet()
        {
            _permObj = new Perm(RolesNames.MEDIA_FILES, _repositories, this.User.FindFirst(x => x.Value != null).Value);
            var currentUser = this.User;
            ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
            ViewData["UserId"] = _userManager.GetUserId(currentUser);
        }
    }
}
