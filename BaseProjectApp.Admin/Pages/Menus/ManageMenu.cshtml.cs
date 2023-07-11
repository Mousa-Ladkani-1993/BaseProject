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

namespace BaseProjectApp.Admin.Pages.Menus
{
    public class ManageMenuModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private UserManager<IdentityUser> _userManager;
        private readonly IUnitofWork _repositories = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Perm _permObj;
        private readonly IMemoryCache _cache;
        public bool Edit = false;

        public ManageMenuModel(IMemoryCache cache, IConfiguration configuration, UserManager<IdentityUser> userManager, IUnitofWork repositories, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _userManager = userManager;
            _repositories = repositories;
            _webHostEnvironment = webHostEnvironment;
            _cache = cache;
        }


        [BindProperty]
        public int? BindedMobileCustomMenuId { get; set; }



        [BindProperty]
        public string? Label { get; set; }

        [BindProperty]
        public string? Name { get; set; }

        [BindProperty]
        public string? NameAr { get; set; }


        public string? Summary { get; set; }

        [BindProperty]
        public string? SummaryAr { get; set; }

        [BindProperty]
        public string? Details { get; set; }

        [BindProperty]
        public string? DetailsAr { get; set; }

        [BindProperty]
        public bool Active { get; set; }

        [BindProperty]
        public int? Priority { get; set; }

        [BindProperty]
        public int? ParentId { get; set; }

        [BindProperty]
        public string? Link { get; set; }

        [BindProperty]
        public bool ShowInDrawer { get; set; }





        [BindProperty]
        public string? ImageSrc { get; set; }
        [BindProperty]
        public string? ImageId { get; set; }

        [BindProperty]
        public string? Image { get; set; }

        [BindProperty]
        public string? ImageUrl { get; set; }


        [BindProperty]
        public MobileCustomMenu MobileCustomMenu { get; set; }



        public async Task<IActionResult> OnGetAsync(int? id)
        {
            _permObj = new Perm(RolesNames.MENUS, _repositories, this.User.FindFirst(x => x.Value != null).Value);
            var currentUser = this.User;
            ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
            ViewData["DevelopmentLocalhost"] = _configuration["DevelopmentLocalhost"];
            ViewData["UserId"] = _userManager.GetUserId(currentUser);
            ViewData["ContentRootPath"] = _webHostEnvironment.ContentRootPath;
            ViewData["MobileCustomMenuId"] = (id == null ? 0 : id);

            id = id ?? 0;
            ViewData["Parents"] = new SelectList((await _repositories.MobileCustomMenus.SelectAll(s => new ListItem { Id = s.Id, Value = s.Label }, expression: s => id == 0 || s.Id != id)), "Id", "Value");

            var MobileCustomMenus = await _repositories.MobileCustomMenus.GetAll();
            ViewData["MobileCustomMenus"] = new SelectList(MobileCustomMenus, "Id", "Label");
            _cache.Set("MobileCustomMenus", MobileCustomMenus);



            BindedMobileCustomMenuId = id == null || id == 0 ? 0 : (int)id;
            Edit = BindedMobileCustomMenuId > 0;

            _cache.Set("BindedMobileCustomMenuId", BindedMobileCustomMenuId);


            if (id != null)
            {
                if (id > 0)
                {
                    await LoadDataAsync((int)id);
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSaveAndContinueAsync()
        {

            ModelState.Remove("MobileCustomMenu.Id");

            if (MobileCustomMenu.Id > 0)
            {
                ModelState.Remove("Icon");
            }

            if (!ModelState.IsValid)
            {
                _permObj = new Perm(RolesNames.MENUS, _repositories, this.User.FindFirst(x => x.Value != null).Value);
                var currentUser = this.User;
                ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
                ViewData["DevelopmentLocalhost"] = _configuration["DevelopmentLocalhost"];
                ViewData["UserId"] = _userManager.GetUserId(currentUser);
                ViewData["ContentRootPath"] = _webHostEnvironment.ContentRootPath;
                ViewData["MobileCustomMenuId"] = MobileCustomMenu.Id;

                var MobileCustomMenusData = _cache.Get<IEnumerable<MobileCustomMenu>>("MobileCustomMenus");
                ViewData["MobileCustomMenus"] = new SelectList(MobileCustomMenusData, "Id", "Label");


                ViewData["Parents"] = new SelectList((await _repositories.MobileCustomMenus.SelectAll(s => new ListItem { Id = s.Id, Value = s.Label }, expression: s => MobileCustomMenu.Id == 0 || s.Id != MobileCustomMenu.Id)), "Id", "Value");

                BindedMobileCustomMenuId = _cache.Get<int>("BindedMobileCustomMenuId");
                Edit = BindedMobileCustomMenuId > 0;

                return Page();
            }

            Guid UserId = Guid.Empty;
            int Id = await SaveMobileCustomMenuAsync();

            ViewData["Parents"] = new SelectList((await _repositories.MobileCustomMenus.SelectAll(s => new ListItem { Id = s.Id, Value = s.Label }, expression: s => MobileCustomMenu.Id == 0 || s.Id != MobileCustomMenu.Id)), "Id", "Value");

            var MobileCustomMenus = _cache.Get<IEnumerable<MobileCustomMenu>>("MobileCustomMenus");
            ViewData["MobileCustomMenus"] = new SelectList(MobileCustomMenus, "Id", "Label");
            BindedMobileCustomMenuId = _cache.Get<int>("BindedMobileCustomMenuId");

            return Redirect("~/Menus/ManageMenu?id=" + Id);
        }

        public async Task<IActionResult> OnPostSaveAndExitAsync()
        {

            ModelState.Remove("MobileCustomMenu.Id");

            if (MobileCustomMenu.Id > 0)
            {
                ModelState.Remove("Icon");

            }


            if (!ModelState.IsValid)
            {
                _permObj = new Perm(RolesNames.MENUS, _repositories, this.User.FindFirst(x => x.Value != null).Value);
                var currentUser = this.User;
                ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
                ViewData["DevelopmentLocalhost"] = _configuration["DevelopmentLocalhost"];
                ViewData["UserId"] = _userManager.GetUserId(currentUser);
                ViewData["ContentRootPath"] = _webHostEnvironment.ContentRootPath;
                ViewData["MobileCustomMenuId"] = MobileCustomMenu.Id;
                var MobileCustomMenus = _cache.Get<IEnumerable<MobileCustomMenu>>("MobileCustomMenus");
                ViewData["MobileCustomMenus"] = new SelectList(MobileCustomMenus, "Id", "Label");
                ViewData["Parents"] = new SelectList((await _repositories.MobileCustomMenus.SelectAll(s => new ListItem { Id = s.Id, Value = s.Label }, expression: s => MobileCustomMenu.Id == 0 || s.Id != MobileCustomMenu.Id)), "Id", "Value");

                BindedMobileCustomMenuId = _cache.Get<int>("BindedMobileCustomMenuId");
                Edit = BindedMobileCustomMenuId > 0;

                return Page();
            }

            Guid UserId = Guid.Empty;
            int Id = await SaveMobileCustomMenuAsync();

            var MobileCustomMenusData = _cache.Get<IEnumerable<MobileCustomMenu>>("MobileCustomMenus");
            ViewData["MobileCustomMenus"] = new SelectList(MobileCustomMenusData, "Id", "Label");
            ViewData["Parents"] = new SelectList((await _repositories.MobileCustomMenus.SelectAll(s => new ListItem { Id = s.Id, Value = s.Label }, expression: s => MobileCustomMenu.Id == 0 || s.Id != MobileCustomMenu.Id)), "Id", "Value");

            BindedMobileCustomMenuId = _cache.Get<int>("BindedMobileCustomMenuId");
            return Redirect("~/Menus/ManageMenus");
        }

        public async Task<IActionResult> OnPostSaveAndAddNewAsync()
        {


            ModelState.Remove("MobileCustomMenu.Id");
            if (!ModelState.IsValid)
            {
                _permObj = new Perm(RolesNames.MENUS, _repositories, this.User.FindFirst(x => x.Value != null).Value);
                var currentUser = this.User;
                ViewData["APIURL"] = _configuration["AppSettings:APIURL"];
                ViewData["DevelopmentLocalhost"] = _configuration["DevelopmentLocalhost"];
                ViewData["UserId"] = _userManager.GetUserId(currentUser);
                ViewData["ContentRootPath"] = _webHostEnvironment.ContentRootPath;
                ViewData["MobileCustomMenuId"] = MobileCustomMenu.Id;
                var MobileCustomMenus = _cache.Get<IEnumerable<MobileCustomMenu>>("MobileCustomMenus");
                ViewData["MobileCustomMenus"] = new SelectList(MobileCustomMenus, "Id", "Label");
                ViewData["Parents"] = new SelectList((await _repositories.MobileCustomMenus.SelectAll(s => new ListItem { Id = s.Id, Value = s.Label }, expression: s => MobileCustomMenu.Id == 0 || s.Id != MobileCustomMenu.Id)), "Id", "Value");


                BindedMobileCustomMenuId = _cache.Get<int>("BindedMobileCustomMenuId");
                Edit = BindedMobileCustomMenuId > 0;

                return Page();
            }

            Guid UserId = Guid.Empty;
            int Id = await SaveMobileCustomMenuAsync();


            var MobileCustomMenusData = _cache.Get<IEnumerable<MobileCustomMenu>>("MobileCustomMenus");
            ViewData["MobileCustomMenus"] = new SelectList(MobileCustomMenusData, "Id", "Label");
            ViewData["Parents"] = new SelectList((await _repositories.MobileCustomMenus.SelectAll(s => new ListItem { Id = s.Id, Value = s.Label }, expression: s => MobileCustomMenu.Id == 0 || s.Id != MobileCustomMenu.Id)), "Id", "Value");

            BindedMobileCustomMenuId = _cache.Get<int>("BindedMobileCustomMenuId");

            return Redirect("~/Menus/ManageMenu?id=0");
        }

        public async Task LoadDataAsync(int id)
        {

            MobileCustomMenu = _repositories.MobileCustomMenus.GetById(id);

            if (MobileCustomMenu != null)
            {

                Label = MobileCustomMenu.Label;
                Name = MobileCustomMenu.Name;
                NameAr = MobileCustomMenu.NameAr;
                Summary = MobileCustomMenu.Summary;
                SummaryAr = MobileCustomMenu.SummaryAr;
                Details = MobileCustomMenu.Details;
                DetailsAr = MobileCustomMenu.DetailsAr;
                Active = MobileCustomMenu.Active == true;
                ShowInDrawer = MobileCustomMenu.ShowInDrawer == true;
                Priority = MobileCustomMenu.Priority;
                ParentId = MobileCustomMenu.ParentId == null ? -1 : MobileCustomMenu.ParentId;
                Link = MobileCustomMenu.Link;
                ImageSrc = MobileCustomMenu.IconUrl;
            }


        }

        public async Task<int> SaveMobileCustomMenuAsync()
        {
            string stringGuid = _userManager.GetUserId(this.User);
            Guid UserId = Guid.Parse(stringGuid);

            MobileCustomMenu myMobileCustomMenu = new MobileCustomMenu();

            if (MobileCustomMenu.Id != 0)
                myMobileCustomMenu = _repositories.MobileCustomMenus.GetById(MobileCustomMenu.Id);

            myMobileCustomMenu.ShowInDrawer = ShowInDrawer == true;
            myMobileCustomMenu.Active = Active == true;
            myMobileCustomMenu.Label = Label;
            myMobileCustomMenu.Name = Name;
            myMobileCustomMenu.NameAr = NameAr;
            myMobileCustomMenu.Summary = Summary;
            myMobileCustomMenu.SummaryAr = SummaryAr;
            myMobileCustomMenu.Details = Details;
            myMobileCustomMenu.DetailsAr = DetailsAr;
            myMobileCustomMenu.Priority = Priority;
            myMobileCustomMenu.Link = Link;
            myMobileCustomMenu.ParentId = ParentId == null ? -1 : ParentId;

            if (!string.IsNullOrWhiteSpace(ImageUrl))
            {
                myMobileCustomMenu.IconUrl = ImageUrl;
            }

            if (myMobileCustomMenu.Id == 0)
                _repositories.MobileCustomMenus.Insert(myMobileCustomMenu);
            else
                _repositories.MobileCustomMenus.Update(myMobileCustomMenu);

            await _repositories.Save(UserId);


            if (myMobileCustomMenu.Id > 0)
            {
                return myMobileCustomMenu.Id;
            }

            return -1;
        }

    }
}

