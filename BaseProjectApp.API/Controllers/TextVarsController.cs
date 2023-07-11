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
using BaseProjectApp.Library.Templates;
using BaseProjectApp.Library.Templates.DTOs;
using System.Net;
using BaseProjectApp.Library.Utility;

namespace BaseProjectApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextVarsController : Controller
    {
        private readonly IUnitofWork repositories = null;
        private readonly IMapper _mapper;
        private readonly ILogger<TextVarsController> _logger;
        private Guid UserId;

        public TextVarsController(IUnitofWork repositories, ILogger<TextVarsController> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
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



        [HttpGet]
        [Route("Get")]
        [Permission(RolesNames.TEXT_VARIABLES, ApiActions.VIEW, "TextVars/Get")]
        [Authorize(Auth_Permissions.Text_Variables.CanViewTextVariables)]
        public IActionResult Get(int id)
        {
            var result = repositories.TextVars.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpGet]
        [Route("GetAll")]
        [Authorize(Auth_Permissions.Text_Variables.CanViewTextVariables)]
        [Permission(RolesNames.TEXT_VARIABLES, ApiActions.VIEW, "TextVars/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await repositories.TextVars.GetAll();

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [APIKey]
        [AllowAnonymous]
        [HttpGet]
        [Route("~/Api/Web/TextVars/{Key}")]
        public async Task<IActionResult> WebGetPageDetailsAsync(string? Key = "", string lang = "en")
        {

            if (string.IsNullOrWhiteSpace(Key))
                return Ok(APIResponse<string>.Fail(lang == "en" ? "please enter a valid Key" : "الرجاء ادخال كلمة صالح صالحة",""));


            TextVarDto textvar = await repositories.TextVars.SelectFirst(x => new TextVarDto
            {
                Data = lang == "en" ? x.DataEn : lang == "ar" ? x.DataAr : "",
                Link = lang == "en" ? x.LinkEn : lang == "ar" ? x.LinkAr : "",
            }, s => s.TextKey != null && s.TextKey.ToLower() == Key.Trim().ToLower());

            if (textvar == null || textvar == null)
                APIResponse<TextVarDto>.NotFound(lang == "en");

            return Ok(APIResponse<TextVarDto>.Success(textvar));


            ;
        }
    }
}
