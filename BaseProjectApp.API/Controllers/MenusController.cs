using AutoMapper;
using BaseProjectApp.API.Authorization;
using BaseProjectApp.API.Middlewares;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Templates.Responses;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseProjectApp.API.Authentication;
using BaseProjectApp.Library.Templates.Enums;
using BaseProjectApp.Library.Templates.DTOs;
using BaseProjectApp.Library.Templates;
using BaseProjectApp.Library.Utility;

namespace BaseProjectApp.API.Controllers
{

    [Authorize]
    [Route("Api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IUnitofWork repositories = null;
        private readonly IMapper _mapper;
        private readonly ILogger<MenusController> _logger;
        private Guid UserId;

        public MenusController(IUnitofWork repositories, ILogger<MenusController> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.repositories = repositories;
            _logger = logger;
            _mapper = mapper;

            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = httpContextAccessor.HttpContext.User.FindFirst(x => x.Type.Contains("UserId")).Value;
                UserId = Guid.Parse(userId);
            }
        }



        [ApiVersion("1.0")]
        [HttpGet]
        [Route("All")]
        [Permission(RolesNames.MENUS, ApiActions.VIEW, "Menu/GetAll")]
        [Authorize(Auth_Permissions.Menus.CanViewMenus)]
        public async Task<IActionResult> GetAll()
        {
            var result = await repositories.MobileCustomMenus.GetAll(orderBy: order => order.OrderBy(s => s.Priority));

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [ApiVersion("1.0")]
        [HttpPut]
        [Route("Up")]
        [Authorize(Auth_Permissions.Menus.CanEditMenus)]
        [Permission(RolesNames.MENUS, ApiActions.VIEW, "Menu/MenuUp")]
        public async Task<IActionResult> MenuUp(int Id = 0)
        {
            var MenuObj = repositories.MobileCustomMenus.GetById(Id);

            if (MenuObj == null)
                return NotFound();

            if (MenuObj.Priority == null || MenuObj.Priority <= 1)
                return NoContent();

            MenuObj.Priority = MenuObj.Priority - 1;

            repositories.MobileCustomMenus.Update(MenuObj);

            var res = await repositories.Save(UserId);

            if (res.Item1)
                return NoContent();

            return NotFound();
        }


        [ApiVersion("1.0")]
        [HttpPut]
        [Route("Down")]
        [Authorize(Auth_Permissions.Menus.CanEditMenus)]
        [Permission(RolesNames.MENUS, ApiActions.VIEW, "Menu/MenuDown")]
        public async Task<IActionResult> MenuDown(int Id = 0)
        {
            var MenuObj = repositories.MobileCustomMenus.GetById(Id);

            if (MenuObj == null)
                return NotFound();

            if (MenuObj.Priority == null)
                return NoContent();

            MenuObj.Priority = MenuObj.Priority + 1;

            repositories.MobileCustomMenus.Update(MenuObj);
            var res = await repositories.Save(UserId);

            if (res.Item1)
                return NoContent();

            return NotFound();
        }


        [ApiVersion("1.0")]
        [HttpPut]
        [Route("ActiveMenus")]
        [Authorize(Auth_Permissions.Menus.CanEditMenus)]
        [Permission(RolesNames.MENUS, ApiActions.VIEW, "Menu/MenuDown")]
        public async Task<IActionResult> Active_DeActive(bool Active, int Id = 0)
        {
            var MenuObj = repositories.MobileCustomMenus.GetById(Id);

            if (MenuObj == null)
                return NotFound();

            if (MenuObj.Priority == null)
                return NoContent();

            MenuObj.Active = Active;

            repositories.MobileCustomMenus.Update(MenuObj); 

            var res = await repositories.Save(UserId);

            if (res.Item1)
                return NoContent();

            return NotFound();
        }


        [ApiVersion("1.0")]
        [APIKey]
        [AllowAnonymous]
        [HttpGet]
        [Route("~/Api/Web/Menus/All")]
        public async Task<IActionResult> WebGetAll(string Lang = "en")
        {
            string? AdminUrl = Configuration.GetConfigURL();

            var menus = await repositories.MobileCustomMenus.SelectAll(s => new MobileCustomMenuDTO
            {
                Id = s.Id,
                Name = Lang == "en" ? s.Name : s.NameAr,
                Details = Lang == "en" ? s.Details : s.DetailsAr,
                Summary = Lang == "en" ? s.Summary : s.SummaryAr,
                Priority = s.Priority,
                ParentId = s.ParentId,
                ShowInDrawer = s.ShowInDrawer == true,
                IconUrl = Configuration.GenerateURL(s.IconUrl, AdminUrl),
                Label = s.Label,
                Link = s.Link,
            }
            , expression: s => s.Active == true
            , orderBy: order => order.OrderBy(s => s.Priority)
            );

            if (menus == null || !menus.Any())
            { return NotFound(); }

            var TreeDataBase = _mapper.Map<List<MobileCustomMenuNode>>(menus);
            var TreeData = TreeBuilder.BuildMenusTree(TreeDataBase);

            return Ok(APIResponse<ICollection<MobileCustomMenuNode>>.Success(TreeData));

        }

    }
}
