using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BaseProjectApp.Admin.Areas.Identity.Model;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using BaseProjectApp.Library.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Exception = System.Exception;

namespace BaseProjectApp.Admin.Pages.Users
{
    public class ManageUserModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitofWork _repositories = null;
        private readonly IMemoryCache _cache;


        public ManageUserModel(IMemoryCache cache, IConfiguration configuration, UserManager<IdentityUser> userManager, RoleManager<ApplicationRole> roleManager, IUnitofWork repositories)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _repositories = repositories;
            _cache = cache;

        }

        [BindProperty]
        public IdentityUser IdentityUser { get; set; }


        [BindProperty]
        public string? FullName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }

        [BindProperty]
        //[Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [BindProperty]
        public string? NewResetPassword { get; set; }

        [BindProperty]
        public bool Active { get; set; }

        [BindProperty]
        public string rolesList { get; set; }

        [BindProperty]
        public string Mode { get; set; }

        [BindProperty]
        public string? txtMediaId { get; set; }

        [BindProperty]
        public string? UserProfileImage { get; set; }

        [BindProperty]
        public string? UserProfileImageUrl { get; set; }

        [BindProperty]
        public string? RolesPermissions { get; set; }

        public static string password;

        public async Task<IActionResult> OnGet(string id, string mode)
        {
            var currentUser = this.User;
            string APIUrl = _configuration["AppSettings:APIURL"];
            ViewData["APIURL"] = APIUrl;
            var UserId = _userManager.GetUserId(currentUser);
            ViewData["UserId"] = UserId;
            ViewData["Mode"] = mode;
            ViewData["UrlUserId"] = id;
            Mode = mode;
            ViewData["PasswordCheck"] = "";


            _cache.Set("mode", mode);
            _cache.Set("UrlUserId", id);
            _cache.Set("UserId", UserId);
            _cache.Set("APIUrl", APIUrl);


            if (id != null)
                await LoadData(id);

            return Page();
        }

        public async Task<JsonResult> OnGetLoadRolesList()
        {
            List<ApplicationRole> roles = _roleManager.Roles.ToList();
            List<ListModel> rolesList = new List<ListModel>();

            var _User = (await _repositories.AspNetUsers.GetAll(u => u.Id == _userManager.GetUserId(this.User))).FirstOrDefault();


            for (int i = 0; i <= roles.Count - 1; i++)
            {
                ListModel newRole = new ListModel();
                newRole.id = roles[i].Id;
                newRole.title = roles[i].Name;

                rolesList.Add(newRole);
            }

            return new JsonResult(rolesList);
        }

        public async Task<IActionResult> OnPostSaveAndContinue()
        {
            ModelState.Remove("IdentityUser.Id");
            if (Mode == "Edit")
            {
                ModelState.Remove("NewPassword");
                ModelState.Remove("Email");
                ModelState.Remove("UserName");
            }

            if (!ModelState.IsValid)
            {
                string mode = _cache.Get<string>("mode");
                string UrlUserId = _cache.Get<string>("UrlUserId");
                string userId = _cache.Get<string>("UserId");
                string APIUrl = _cache.Get<string>("APIUrl");

                ViewData["UrlUserId"] = UrlUserId;
                ViewData["mode"] = mode;
                ViewData["UserId"] = userId;
                ViewData["APIUrl"] = APIUrl;
                Mode = mode;
                ViewData["PasswordCheck"] = "";


                Console.WriteLine(Email);
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList().ForEach(Console.WriteLine);
                return Page();
            }
            try
            {
                SaveResult res = await SaveUser();

                if (res.Success)
                    return Redirect("~/Users/ManageUser?id=" + res.Id + "&mode=Edit");

                else
                    return Page();
            }

            catch (Exception ex)
            {
                return Redirect("~/Users/ManageUser?id=0&mode=New");
            }

        }

        public async Task<IActionResult> OnPostResetPassword()
        {
            string Id = await ResetPassword();

            return Redirect("~/Users/ManageUser?id=" + Id + "&mode=Reset");
        }

        //public async void LoadData(string id)
        public async Task LoadData(string id)
        {
            IdentityUser = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();

            //UserName = IdentityUser.Email;
            //FullName = IdentityUser.Email;
            Email = IdentityUser?.Email;
            Active = IdentityUser == null ? false : !IdentityUser.LockoutEnabled;
            NewResetPassword = password;

            //Load Roles

            var UserObj = await _repositories.AspNetUsers.GetFirst(s => s.Id == id, includeExpressions: new[] { "Roles" });
            var roles = UserObj?.Roles;
            FullName = UserObj.FullName;
            //var User = await _userManager.FindByIdAsync(id); 
            //var Roles = await _userManager.GetUsersInRoleAsync(User);

            //List<AspNetUserRole> roles
            //_repositories.AspNetUserRoles.GetAllById(item => item.UserId == id);

            if (roles != null)
            {
                string allRoles = "";
                //foreach (AspNetUserRole role in roles)
                foreach (var role in roles)
                {
                    allRoles += role.Id + " ";
                }
                rolesList = allRoles;
            }
        }

        public async Task<SaveResult> SaveUser()
        {
            string stringGuid = _userManager.GetUserId(this.User);
            Guid UserId = Guid.Parse(stringGuid);

            IdentityUser myUser = new IdentityUser();

            if (IdentityUser.Id != null)
                myUser = await _userManager.FindByIdAsync(IdentityUser.Id);

            if (IdentityUser.Id == null)
            {
                myUser.UserName = Email;
                myUser.Email = Email;
            }

            myUser.PhoneNumber = IdentityUser.PhoneNumber;
            myUser.EmailConfirmed = true;


            IdentityResult result;
            if (IdentityUser.Id != null)
                result = await _userManager.UpdateAsync(myUser);
            else
            {

                var Validators = _userManager.PasswordValidators;
                bool ValidPassword = true;
                string Errors = "";

                foreach (var item in Validators)
                {
                    var res = await item.ValidateAsync(_userManager, myUser, NewPassword);

                    if (!res.Succeeded)
                    {
                        ValidPassword = false;
                        Errors = string.Join("\n", res.Errors.Select(s => s.Description));
                    }

                }

                if (!ValidPassword)
                {
                    string mode = _cache.Get<string>("mode");
                    string UrlUserId = _cache.Get<string>("UrlUserId");
                    string userId = _cache.Get<string>("UserId");
                    string APIUrl = _cache.Get<string>("APIUrl");

                    ViewData["UrlUserId"] = UrlUserId;
                    ViewData["mode"] = mode;
                    ViewData["UserId"] = userId;
                    ViewData["APIUrl"] = APIUrl;
                    Mode = mode;
                    ViewData["PasswordCheck"] = Errors;


                    return new SaveResult
                    {
                        Success = false,
                        Id = "",
                        Message = "",
                    };

                }



                myUser.PasswordHash = _userManager.PasswordHasher.HashPassword(myUser, NewPassword);


                result = ValidPassword ? await _userManager.CreateAsync(myUser) : null;
            }


            if (result != null && result.Succeeded)
            {
                //var _User = (await _repositories.AspNetUsers.GetAll(u => u.Id == _userManager.GetUserId(this.User))).FirstOrDefault();


                if (rolesList != null)
                {

                    var User = (await _repositories.AspNetUsers.GetFirst(s => s.Id == myUser.Id, includeExpressions: new[] { "Roles" }));
                    //var UserRoles  = (await _repositories.AspNetUserRoles.GetAll(s => s.UserId == myUser.Id))?.ToList();

                    User.FullName = FullName;
                    User.Roles = null;
                    //var Res1 = await _repositories.Save(UserId);

                    //if (UserRoles != null)
                    //    _repositories.AspNetUserRoles.DeleteBulk(UserRoles);


                    string[] roles = rolesList.ToString().Split(',');
                    if (roles.Length > 0)
                    {
                        var AllRoles = await _repositories.AspNetRoles.GetAll();
                        //_roleManager.Roles?.ToList();

                        User.Roles = new List<AspNetRole>();

                        for (int i = 0; i <= roles.Length - 1; i++)
                        {

                            if (roles[i].Trim() != "" && roles[i].Trim() != "Select All")
                            {
                                var temp = AllRoles?.FirstOrDefault(x => x.Name == roles[i].Trim());

                                //ApplicationRole
                                if (temp == null)
                                    continue;

                                //AspNetUserRole newRole = new AspNetUserRole();
                                //newRole.UserId = myUser.Id;
                                //newRole.RoleId = temp.Id;

                                User.Roles.Add(temp);

                                //bool isExisted = _repositories.AspNetUserRoles.Exist(item => item.RoleId == temp.Id && item.UserId == myUser.Id);

                                //if (!isExisted) 
                                //_repositories.AspNetUserRoles.Insert(newRole);

                            }
                        }

                        //_repositories.AspNetUsers.UpdateExistString(User, User.Id);

                        _repositories.AspNetUsers.Update(User);
                        await _repositories.Save(UserId);
                    }
                }
                else
                {
                    var User =  _repositories.AspNetUsers.GetByIdString(myUser.Id); 
                    User.FullName = FullName;

                    _repositories.AspNetUsers.Update(User);
                    await _repositories.Save(UserId);
                }
                if (RolesPermissions != null && RolesPermissions != "null")
                {
                    var permissions = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(RolesPermissions);

                    _repositories.UserPermissions.Delete(up => up.UserId == myUser.Id);
                    await _repositories.Save(UserId);

                    foreach (var roleName in permissions.Keys)
                    {
                        var role = _roleManager.Roles.Where(x => x.Name == roleName.Trim()).FirstOrDefault();

                        if (role == null)
                            continue;

                        _repositories.UserPermissions.Insert(
                            new UserPermission()
                            {
                                UserId = myUser.Id,
                                RoleId = role.Id,
                                CanView = true,
                                CanAdd = permissions[roleName].add,
                                CanEdit = permissions[roleName].edit,
                                CanDelete = permissions[roleName].delete
                            }
                        );

                    }

                    await _repositories.Save(UserId);
                }


                string icon = UserProfileImage;
                string dataUrl = UserProfileImageUrl;
                if (icon != null && dataUrl != null)
                {
                    if (txtMediaId != null)
                        SaveImage(myUser.Id.ToString(), txtMediaId, icon, dataUrl);
                    else
                        SaveImage(myUser.Id.ToString(), "", icon, dataUrl);
                }

                return new SaveResult { Success = true, Id = myUser.Id.ToString() };

            }
            else
            {
                string mode = _cache.Get<string>("mode");
                string UrlUserId = _cache.Get<string>("UrlUserId");
                string userId = _cache.Get<string>("UserId");
                string APIUrl = _cache.Get<string>("APIUrl");

                ViewData["UrlUserId"] = UrlUserId;
                ViewData["mode"] = mode;
                ViewData["UserId"] = userId;
                ViewData["APIUrl"] = APIUrl;
                Mode = mode;
                ViewData["PasswordCheck"] = "";


                return new SaveResult { Success = false };
            }

        }

        public async Task<string> ResetPassword()
        {
            IdentityUser user = await _userManager.FindByIdAsync(IdentityUser.Id);

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string newPassword = RandomPassword.Generate(6);
            //var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, newPassword);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                password = newPassword;
                return IdentityUser.Id;
            }
            else
                return "-1";
        }

        private void SaveImage(string userProfileId, string mediaId, string icon, string dataUrl)
        {
            MediaFile myFile = new MediaFile();
            int id = 0;
            int.TryParse(mediaId, out id);
            if (id > 0)
                myFile = _repositories.MediaFiles.GetById(id);

            string dateString = DateTime.Now.Ticks.ToString();
            string[] extension = icon.Split('.');
            var DocPath = "/Media/Default/" + extension[0] + "_" + dateString + "." + extension[1];
            string path = Path.Combine(DocPath);

            string filename = extension[0] + "_" + dateString + "." + extension[1];
            string filepath = Path.Combine(_configuration["MediaSettings:MEDIAFILESURL"], "Media", "Default") + $@"\{filename}";

            string cleandata = Regex.Replace(dataUrl, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);
            byte[] data = System.Convert.FromBase64String(cleandata);
            MemoryStream ms = new MemoryStream(data);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            img.Save(filepath, System.Drawing.Imaging.ImageFormat.Png);

            myFile.UserProfileId = userProfileId;
            myFile.FilePath = path;
            myFile.FileName = filename;
            myFile.CaptionEn = "User Profile Image " + userProfileId;
            myFile.CaptionAr = "";
            myFile.YouTubePath = "";
            myFile.DisplayOrder = 1;
            myFile.TypeId = 1;
            myFile.CreationDate = DateTime.Now;
            myFile.MainImage = false;

            string stringGuid = _userManager.GetUserId(this.User);
            Guid UserId = Guid.Parse(stringGuid);

            if (id == 0)
                _repositories.MediaFiles.Insert(myFile);
            else
                _repositories.MediaFiles.Update(myFile);

            _repositories.Save(UserId);
        }


        public class SaveResult
        {
            public string Id { get; set; }
            public bool Success { get; set; }
            public string Message { get; set; }
        }

        public class ListModel
        {
            public string id { get; set; }
            public string title { get; set; }
        }
    }
}
