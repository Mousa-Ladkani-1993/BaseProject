using AutoMapper;
using BaseProjectApp.API.Authorization;
using BaseProjectApp.API.Middlewares;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BaseProjectApp.API.Controllers
{
    [Authorize]
    [Route("Api/SystemParameters")]
    [ApiController]
    public class SystemParametersController : ControllerBase
    {
        private readonly IUnitofWork repositories = null;
        private readonly IMapper _mapper;
        private readonly ILogger<SystemParametersController> _logger;
        private Guid UserId;

        public SystemParametersController(IUnitofWork repositories, ILogger<SystemParametersController> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
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
        [Route("Products")]
        [Permission(RolesNames.SYSTEM_PARAMETERS, ApiActions.VIEW, "SystemParameters/GetProducts")]
        [Authorize(Auth_Permissions.SystemParameters.CanViewSystemParameters)]
        public IActionResult GetProducts()
        {
            var result = repositories.SystemParameters.GetAll();

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [Route("All")]
        [HttpGet]
        [Permission(RolesNames.SYSTEM_PARAMETERS, ApiActions.VIEW, "SystemParameters/GetProducts")]
        [Authorize(Auth_Permissions.SystemParameters.CanViewSystemParameters)]
        public async Task<IActionResult> GetSystemParameteres()
        {
            try
            {
                var systemParameters = await repositories.SystemParameters.GetAll(item => item.Editable == true);

                if (systemParameters == null)
                    return NotFound();

                return Ok(systemParameters);
            }

            catch (Exception ex)
            {

                return Ok();
            }
        }


        [HttpGet]
        [Permission(RolesNames.SYSTEM_PARAMETERS, ApiActions.VIEW, "SystemParameters/GetSystemParameter")]
        [Authorize(Auth_Permissions.SystemParameters.CanViewSystemParameters)]
        public IActionResult GetSystemParameter(int SystemParameterId)
        {
            SystemParameter systemParameter = repositories.SystemParameters.GetById(SystemParameterId);

            if (systemParameter == null)
                return NotFound();

            return Ok(systemParameter);
        }


        [HttpPut]
        [Authorize(Auth_Permissions.SystemParameters.CanAddSystemParameters)]
        public async Task<IActionResult> SaveSystemParameter([FromBody] SystemParameter obj)
        {
            if (obj.Id == 0)
                repositories.SystemParameters.Insert(obj);
            else
            { 
                var MyObj = repositories.SystemParameters.GetById(obj.Id);

                MyObj.BoolValue = obj.BoolValue;
                MyObj.DecimalValue = obj.DecimalValue;
                MyObj.TextValue = obj.TextValue;
                MyObj.DateValue = obj.DateValue;

                repositories.SystemParameters.Update(MyObj); 
            }

            var saveResult = await repositories.Save(UserId);

            if (saveResult.Item1 != true)
                return NotFound();

            return NoContent();
        }

    }
}
