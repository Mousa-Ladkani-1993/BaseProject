
using AutoMapper;
using BaseProjectApp.API.Authentication;
using BaseProjectApp.API.Middlewares;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.DbSpsResult;
using BaseProjectApp.Library.Templates.DTOs;
using BaseProjectApp.Library.Templates.Responses;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using BaseProjectApp.Library.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseProjectApp.API.Middlewares.ExceptionHandler;
using BaseProjectApp.API.Authorization;
using BaseProjectApp.Library.Templates;

namespace BaseProjectApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GlobalController : ControllerBase
    {

        private readonly IUnitofWork repositories = null;
        private readonly IMapper _mapper;
        private readonly ILogger<GlobalController> _logger;
        private Guid UserId;

        public GlobalController(IUnitofWork repositories, ILogger<GlobalController> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
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
        [APIKey]
        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/v1.0/Web/[controller]/Filter")]
        public async Task<IActionResult> Search(string Keyword = "", string lang = "en")
        { 
            return Ok(APIResponse<List<GlobalSearchDTO>>.Success(await repositories.globalRepo.Search(Keyword, lang))); 
        }
    }
}
