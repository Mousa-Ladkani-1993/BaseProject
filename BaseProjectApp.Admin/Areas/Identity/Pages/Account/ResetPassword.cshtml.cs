using BaseProjectApp.Library.Repositories.UnitOfwork;
using BaseProjectApp.Library.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaseProjectApp.Admin.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<ResetPasswordModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUnitofWork _repositories;

        public ResetPasswordModel(IUnitofWork repositories, SignInManager<IdentityUser> signInManager, ILogger<ResetPasswordModel> logger, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _repositories = repositories;
        }

        public IActionResult OnGet(string ResetToken, string UserEmail)
        {
            if (ResetToken == null || UserEmail == null)
            {
                return Redirect("/");
            }

            return Page();
        }


        public async Task<JsonResult> OnPostResetPassword(string Password, string ResetToken, string UserEmail)
        {

            (bool isValid, string resetCode, string userEmail) = ResetPasswordUtility.ValidateToken(ResetToken, _configuration["AppSettings:ResetSecret"]);

            if (!isValid)
            {
                return new JsonResult(new { status = false, redirect = "/Identity/Account/Login", msg = "Invalid token" });
            }
            var user = (await _repositories.AspNetUsers.GetAll(u => u.Email == userEmail)).FirstOrDefault();

            if (user == null || user.ResetPasswordCode != resetCode)
            {
                return new JsonResult(new { status = false, redirect = "/Identity/Account/Login", msg = "user not found" });
            }

            string passwordHash = _userManager.PasswordHasher.HashPassword(_userManager.Users.First(u => u.Id == user.Id), Password);

            user.PasswordHash = passwordHash;
            user.ResetPasswordCode = null;
            var saveRes = await _repositories.Save(Guid.Empty);
            if (saveRes.Item1 == false)
            {
                return new JsonResult(new { status = false, redirect = "/Identity/Account/Login", msg = "please try again later" });
            }

            return new JsonResult(new { status = true, redirect = "/Identity/Account/Login", msg = "password updated successfully" });
        }



    }
}
