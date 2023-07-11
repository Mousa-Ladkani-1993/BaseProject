using BaseProjectApp.Library.Repositories.UnitOfwork;
using BaseProjectApp.Library.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BaseProjectApp.Admin.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUnitofWork _repositories;

        public LoginModel(IUnitofWork repositories, SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _repositories = repositories;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Index");
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            ViewData["AppName"] = !string.IsNullOrWhiteSpace(_configuration["AppName"]) ? "/" + _configuration["AppName"] : "";

            ViewData["Message"] = "";

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            ViewData["AppName"] = !string.IsNullOrWhiteSpace(_configuration["AppName"]) ? "/" + _configuration["AppName"] : "";

            ViewData["Message"] = "";

            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            try
            {

                if (ModelState.IsValid)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true


                    var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        var userInfo = await _userManager.FindByEmailAsync(Input.Email);

                        // Create Cookie and Generate new Token
                        CookieOptions option = new CookieOptions();
                        //option.Expires = DateTime.Now.AddYears(1);
                        option.Expires = DateTime.Now.AddDays(10);
                        // option.Secure = true;
                        string tokenValue = GenerateNewTokenAsync(userInfo.Id, Input.Email);

                        Response.Cookies.Append("BaseProjectApp_Admin_Token", tokenValue, option);


                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ViewData["Message"] = "Invalid login attempt.";
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        await _signInManager.SignOutAsync();
                        return Page();
                    }
                }

            }
            catch (Exception ex) { }
            // If we got this far, something failed, redisplay form
            return Page();
        }

        private bool ValidateUserAsync(string UserEmail, string Password)
        {
            string Result = "";
            try
            {
                var ApiUrl = _configuration["AppSettings:APIURL"];
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };


                using (HttpClient httpClient = new HttpClient(clientHandler))
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = httpClient.GetAsync(ApiUrl + "Api/Authenticate/ValidateUser?UserEmail=" + UserEmail + "&Password=" + Password).Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Result = response.Content.ReadAsStringAsync().Result;
                    }

                    if (Result == "true")
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string GenerateNewTokenAsync(string UserId, string UserEmail)
        {
            string Token = "";
            try
            {
                var ApiUrl = _configuration["AppSettings:APIURL"];

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };


                using (HttpClient httpClient = new HttpClient(clientHandler))
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = httpClient.GetAsync(ApiUrl + "Api/Authenticate/GenerateNewToken?UserId=" + UserId + "&UserEmail=" + UserEmail).Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Token = response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception e)
            {
                return "";
            }

            return Token;
        }

        public async Task<JsonResult> OnPostResetPassword(string email)
        {
            email = email.Trim();
            var emails = (await _repositories.AspNetUsers.GetAll()).Select(u => u.Email).ToList();
            if (!emails.Contains(email))
            {
                return new JsonResult(new { status = false, msg = "email not found" });
            }

            string secret = _configuration["AppSettings:ResetSecret"];
            string resetCode = ResetPasswordUtility.GenerateResetPasswordCode();
            string resetJwt = ResetPasswordUtility.GenerateResetJwtToken(email, resetCode, secret);

            string resetUrl = $"https://localhost:7228/Identity/Account/ResetPassword?ResetToken={resetJwt}&UserEmail={email}";

            string emailSubject = "BaseProjectApp Reset Password";
            string emailBody = $@"
                <p>follow this link to reset you password it is valid for 30 minutes only</p>
                <br>
                <a href=""{resetUrl}"">{resetUrl}</a>
            ";

            var user = (await _repositories.AspNetUsers.GetAll()).FirstOrDefault(u => u.Email == email);
            user.ResetPasswordCode = resetCode;
            var saveRes = await _repositories.Save(Guid.Empty);

            if (saveRes.Item1 == false)
            {
                Console.WriteLine(saveRes.Item2);
                return new JsonResult(new { status = false, msg = "failed to send email, please try again later" });
            }

            try
            {
                EmailUtility.SendEmail(email, emailSubject, emailBody);
                return new JsonResult(new { status = true, msg = "please check your email." });
            }
            catch (System.Exception)
            {
                return new JsonResult(new { status = false, msg = "failed to send email, please try again later" });
            }

        }
    }
}
