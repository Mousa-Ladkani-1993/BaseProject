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
    [Permission(RolesNames.TEXT_VARIABLES, ApiActions.VIEW, PageType.ONE, "TextVars/ManageTextVar")]
    [Permission(RolesNames.TEXT_VARIABLES, ApiActions.ADD, PageType.ONE, "TextVars/ManageTextVar")]
    [Permission(RolesNames.TEXT_VARIABLES, ApiActions.EDIT, PageType.ONE, "TextVars/ManageTextVar")]
    public class ManageVarModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private UserManager<IdentityUser> _userManager;
        private readonly IUnitofWork _repositories = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Perm _permObj;
        private readonly IMemoryCache _cache; 

        public ManageVarModel(IMemoryCache cache, IConfiguration configuration, UserManager<IdentityUser> userManager, IUnitofWork repositories, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _userManager = userManager;
            _repositories = repositories;
            _webHostEnvironment = webHostEnvironment;
            _cache = cache; 
        }
        [BindProperty]
        public TextVar textVar { get; set; }

        [BindProperty]
        public int? BindedVarId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Aarabic data is required.")]
        public string? DataAr { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "English data is required.")]
        public string? DataEn { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Aarabic link is required.")]
        public string? LinkAr { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "English link is required.")]
        public string? LinkEn { get; set; }
 

        [BindProperty]
        public string? Name { get; set; }
     
     

        [BindProperty]
        public string? ImageId { get; set; }

        [BindProperty]
        public string? Image { get; set; }

        [BindProperty]
        public string? ImageUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            _permObj = new Perm(RolesNames.TEXT_VARIABLES, _repositories, this.User.FindFirst(x => x.Value != null).Value);
            var currentUser = this.User;
            ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
            ViewData["DevelopmentLocalhost"] = _configuration["DevelopmentLocalhost"];
            ViewData["UserId"] = _userManager.GetUserId(currentUser);
            ViewData["ContentRootPath"] = _webHostEnvironment.ContentRootPath;
            ViewData["VarId"] = (id == null ? 0 : id);

            BindedVarId = id == null || id == 0 ? 0 : (int)id;

            _cache.Set("BindedVarId", BindedVarId);


            if (id != null)
            {
                if (id > 0)
                {
                    LoadData((int)id);
                }
            }

            return Page();
        }
         
        public async Task<IActionResult> OnPostSaveAndContinueAsync()
        {
            ModelState.Remove("textVar.Id"); 

            if (!ModelState.IsValid)
            {
                _permObj = new Perm(RolesNames.TEXT_VARIABLES, _repositories, this.User.FindFirst(x => x.Value != null).Value);
                var currentUser = this.User;
                ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
                ViewData["DevelopmentLocalhost"] = _configuration["DevelopmentLocalhost"];
                ViewData["UserId"] = _userManager.GetUserId(currentUser);
                ViewData["ContentRootPath"] = _webHostEnvironment.ContentRootPath;
                ViewData["VarId"] = textVar.Id;
                BindedVarId = _cache.Get<int>("BindedVarId");
                return Page();
            }

            Guid UserId = Guid.Empty;
            int? Id = await SaveVarAsync();


            BindedVarId = _cache.Get<int>("BindedVarId");

            return Redirect("~/TextVars/ManageVar?id=" + Id);
        }

        public async Task<IActionResult> OnPostSaveAndExitAsync()
        {

            ModelState.Remove("textVar.Id");  
            if (!ModelState.IsValid)
            {
                _permObj = new Perm(RolesNames.TEXT_VARIABLES, _repositories, this.User.FindFirst(x => x.Value != null).Value);
                var currentUser = this.User;
                ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
                ViewData["DevelopmentLocalhost"] = _configuration["DevelopmentLocalhost"];
                ViewData["UserId"] = _userManager.GetUserId(currentUser);
                ViewData["ContentRootPath"] = _webHostEnvironment.ContentRootPath;
                ViewData["VarId"] = textVar.Id;
                BindedVarId = _cache.Get<int>("BindedVarId");
                return Page();
            }

            Guid UserId = Guid.Empty;
            int? Id = await SaveVarAsync();
            BindedVarId = _cache.Get<int>("BindedVarId");

            return Redirect("~/TextVars/ManageVars");
        }

        public async Task<IActionResult> OnPostSaveAndAddNew()
        { 
            ModelState.Remove("textVar.Id");
            if (!ModelState.IsValid)
            {
                _permObj = new Perm(RolesNames.TEXT_VARIABLES, _repositories, this.User.FindFirst(x => x.Value != null).Value);
                var currentUser = this.User;
                ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
                ViewData["DevelopmentLocalhost"] = _configuration["DevelopmentLocalhost"];
                ViewData["UserId"] = _userManager.GetUserId(currentUser);
                ViewData["ContentRootPath"] = _webHostEnvironment.ContentRootPath;
                ViewData["VarId"] = textVar;
                BindedVarId = _cache.Get<int>("BindedVarId");
                return Page();
            }

            Guid UserId = Guid.Empty;
            int? Id = await SaveVarAsync();
            BindedVarId = _cache.Get<int>("BindedVarId");

            return Redirect("~/TextVars/ManageVar?id=0");
        }

        public void LoadData(int  id)
        {
            textVar = _repositories.TextVars.GetById(id);
            DataAr = textVar?.DataAr;
            DataEn = textVar?.DataEn;
            LinkAr = textVar?.LinkAr;
            LinkEn = textVar?.LinkEn; 
            Name = textVar?.Name; 
        }

        public async Task<int> SaveVarAsync()
        {
            string? stringGuid = _userManager.GetUserId(this.User);
            Guid UserId = Guid.Parse(stringGuid);

            TextVar myVar = new TextVar();

            if (textVar.Id != 0)
                myVar = _repositories.TextVars.GetById(textVar.Id);

            myVar.DataAr = DataAr;
            myVar.DataEn = DataEn;
            myVar.LinkAr = LinkAr;
            myVar.LinkEn = LinkEn;
            myVar.Name = Name;


            //Vars

            //return -1;
            if (myVar.Id == 0)
            { _repositories.TextVars.Insert(myVar); }
            else
            { _repositories.TextVars.Update(myVar); }
              

           var res =  await _repositories.Save(UserId);

  

            if (myVar.Id == 0)
                return -1;

            if (myVar.Id > 0)
                return myVar.Id;
            else
                return -1;
        }
  
    }
}
