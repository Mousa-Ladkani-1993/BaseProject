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

namespace BaseProjectApp.API.Controllers
{
    [Authorize]
    [Route("api/LookupValues")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly IUnitofWork repositories = null;
        private readonly IMapper _mapper;
        private readonly ILogger<LookupController> _logger;
        private Guid UserId;

        public LookupController(IUnitofWork repositories, ILogger<LookupController> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
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
        [Permission(RolesNames.LOOKUPS, ApiActions.VIEW, "LookupValues/Get")]
        [Authorize(Auth_Permissions.lookups.CanViewlookups)]
        public IActionResult Get(int id)
        {
            var result = repositories.LookupValues.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [ApiVersion("1.0")]
        [HttpGet]
        [Route("All")]
        [Permission(RolesNames.LOOKUPS, ApiActions.VIEW, "LookupValues/GetAll")]
        [Authorize(Auth_Permissions.lookups.CanViewlookups)]
        public async Task<IActionResult> GetAll()
        { 
            return Ok(await repositories.LookupValues.SelectAll(
            rec => new LookupInnerDTO
            {
                Id = rec.Id,
                LookupId = rec.LookupId,
                LookupName = rec.Lookup == null ? "" : rec.Lookup.Name,
                ValueEn = rec.ValueEn,
                ValueAr = rec.ValueAr

            }
          , includeExpressions: new string[] { "Lookup" }));

             
        }


        [ApiVersion("1.0")]
        [HttpGet] 
        [Route("~/api/Lookups")]
        [Permission(RolesNames.LOOKUPS, ApiActions.VIEW, "LookupValues/GetByLookupId")]
        [Authorize(Auth_Permissions.lookups.CanViewlookups)]
        public async Task<IActionResult> GetByLookupId(int id)
            //(int LookupId)
        {
            var result = await repositories.LookupValues.GetAll(lv => lv.LookupId == id);

            if (result == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<LookupValueReponse>>(result));
        }


        [ApiVersion("1.0")]
        [HttpPost]
        [Permission(RolesNames.LOOKUPS, ApiActions.ADD, "LookupValues/Add")]
        [Authorize(Auth_Permissions.lookups.CanAddlookups)]
        public async Task<IActionResult> Add([FromBody] LookupValue Obj)
        {
            if (Obj.Id != 0)
                return BadRequest();

            var LookupValueObj = _mapper.Map<LookupValue>(Obj);

            repositories.LookupValues.Insert(LookupValueObj);

            var saveResult = await repositories.Save(UserId);

            if (saveResult.Item1 == true)
                return StatusCode((int)HttpStatusCode.NoContent);


            return StatusCode((int)HttpStatusCode.InternalServerError, saveResult.Item2);
        }


        [ApiVersion("1.0")]
        [HttpPut]
        [Permission(RolesNames.LOOKUPS, ApiActions.ADD, "LookupValues/Update")]
        [Authorize(Auth_Permissions.lookups.CanAddlookups)]
        public async Task<IActionResult> Update([FromBody] LookupValue Obj)
        {
            if (Obj?.Id == null || Obj.Id <= 0)
                return StatusCode((int)HttpStatusCode.BadRequest, "record must have id..");

            var LookupValueObj = repositories.LookupValues.GetById(Obj.Id);

            LookupValueObj.ValueEn = Obj.ValueEn;
            LookupValueObj.ValueAr = Obj.ValueAr;
            LookupValueObj.LookupId = Obj.LookupId;
            LookupValueObj.Deleted = false;

            repositories.LookupValues.Update(LookupValueObj);

            var saveResult = await repositories.Save(UserId);

            if (saveResult.Item1 == true)
                return StatusCode((int)HttpStatusCode.NoContent);


            return StatusCode((int)HttpStatusCode.InternalServerError, saveResult.Item2);
        }


        [ApiVersion("1.0")]
        [HttpDelete] 
        [Permission(RolesNames.LOOKUPS, ApiActions.DELETE, "LookupValues/Delete")]
        [Authorize(Auth_Permissions.lookups.CanDeletelookups)]
        public async Task<IActionResult> Delete(int id)
        {
            repositories.LookupValues.Delete(id); 
            var saveResult = await repositories.Save(UserId);

            if (saveResult.Item1)
                return StatusCode((int)HttpStatusCode.NoContent);

            return StatusCode((int)HttpStatusCode.InternalServerError, saveResult.Item2);

        }









        [ApiVersion("1.0")]
        [APIKey]
        [AllowAnonymous]
        [HttpGet]
        //[Route("~/Api/Web/Lookup/GetLookupValues")]
        [Route("~/api/v1.0/Web/LookupValues")]
        public async Task<IActionResult> GetLookupValues(int? LookupId = 0, string? lang = "en")
        {

            if (LookupId == null || LookupId <= 0)
                return Ok(APIResponse<string>.Fail(lang == "en" ? "please enter valid id" : "الرجاء ادخال معرف صالح", ""));

            var Data = await repositories.LookupValues.SelectAll(
                expression: s =>
                s.LookupId == LookupId && s.Visible > 0 &&
                ((lang == "en" && s.ValueEn != null) || (lang == "ar" && s.ValueAr != null)),
                   select: x => new LookUpValueDTO
                   {
                       Id = x.Id,
                       Value = (lang == "en" ? x.ValueEn : x.ValueAr)
                   },
                orderBy: order => order.OrderBy(s => s.OrderNb)
                );

            if (Data == null || Data.Count() == 0)
                return Ok(APIResponse<string>.NotFound(lang == "en"));

            return Ok(APIResponse<List<LookUpValueDTO>>.Success(Data.ToList()));
             
        }


        [ApiVersion("1.0")]
        [APIKey]
        [AllowAnonymous]
        [HttpPost]
        //[Route("~/Api/Web/Lookup/GetLookupsValues")]
        [Route("~/api/v1.0/Web/LookupValues/Filter")]
        public async Task<IActionResult> GetLookupValues([FromBody] List<int?> IDs, string? lang = "en")
        {
            if (IDs == null || IDs.Count == 0)
                return Ok(APIResponse<string>.Fail(lang == "en" ? "please enter valid ids" : "الرجاء ادخال معرفات صالحة", ""));

            var Data = await repositories.LookupValues.SelectAll(
                 expression: s =>
                 IDs.Contains(s.LookupId) && s.Visible > 0 &&
                 ((lang == "en" && s.ValueEn != null) || (lang == "ar" && s.ValueAr != null)),
                 select: x => new LookupValueDTO
                 {
                     Id = x.Id,
                     Value = (lang == "en" ? x.ValueEn : x.ValueAr),
                     LookupId = x.LookupId,
                     LookupName = x.Lookup != null ? x.Lookup.Name : ""
                 },
                 orderBy: order => order.OrderBy(s => s.OrderNb),
                 includeExpressions: new[] { "Lookup" }
                 );

            if (Data == null || Data.Count == 0)
                return Ok(APIResponse<string>.NotFound(lang == "en"));

            var GroupedData = (Data.GroupBy(s => s.LookupName))?.ToList();
            Dictionary<string, List<LookUpValueDTO>> Result = new Dictionary<string, List<LookUpValueDTO>>();

            if (GroupedData == null || GroupedData.Count == 0)
                return Ok(APIResponse<string>.NotFound(lang == "en"));

            List<LookUpValueDTO> values;
            string LookUp = "Unknown";


            foreach (var item in GroupedData)
            {
                LookUp = item.Key == null ? LookUp : item.Key;
                values = new List<LookUpValueDTO>();

                foreach (var innerItem in item.ToList())
                {
                    values.Add(new LookUpValueDTO
                    {
                        Id = innerItem.Id,
                        Value = innerItem.Value
                    });
                }

                Result.Add(LookUp, values);
            }
            return Ok(APIResponse<Dictionary<string, List<LookUpValueDTO>>>.Success(Result));


        }
    }
}
