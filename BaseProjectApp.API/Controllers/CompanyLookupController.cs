using AutoMapper;
using BaseProjectApp.API.Authentication;
using BaseProjectApp.API.Authorization;
using BaseProjectApp.API.Middlewares;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Templates.DTOs;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseProjectApp.Library.Templates;
using BaseProjectApp.Library.Templates.Responses;
using System.Net;

namespace BaseProjectApp.API.Controllers
{
    [Authorize]
    [Route("api/CompanyLookupValues")]
    [ApiController]
    public class CompanyLookupController : ControllerBase
    {
        private readonly IUnitofWork repositories = null;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyLookupController> _logger;
        private Guid UserId;

        public CompanyLookupController(IUnitofWork repositories, ILogger<CompanyLookupController> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
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
        [Authorize(Auth_Permissions.Company_Lookups.CanViewCompanyLookups)]
        [Permission(RolesNames.COMPANY_LOOKUPS, ApiActions.VIEW, "CompanyLookupValues/Get")]
        public IActionResult Get(int id)
        {
            CompanyLookupValue result = repositories.CompanyLookupValues.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [ApiVersion("1.0")]
        [HttpGet]
        [Route("All")]
        [Authorize(Auth_Permissions.Company_Lookups.CanViewCompanyLookups)]
        [Permission(RolesNames.COMPANY_LOOKUPS, ApiActions.VIEW, "CompanyLookupValues/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Data = await repositories.CompanyLookupValues.SelectAll(
                  rec => new CompanyLookupInnerDTO
                  {
                      Id = rec.Id,
                      CompanyLookupId = rec.CompanyLookupId,
                      CompanyLookupName = rec.CompanyLookup == null ? "" : rec.CompanyLookup.Name,
                      ValueEn = rec.ValueEn,
                      ValueAr = rec.ValueAr
                  }
                , includeExpressions: new string[] { "CompanyLookup" });

                return Ok(Data);
            }
            catch (Exception ex){ return Ok(); }


        }


        [ApiVersion("1.0")]
        [HttpPost]
        [Authorize(Auth_Permissions.Company_Lookups.CanAddCompanyLookups)]
        [Permission(RolesNames.COMPANY_LOOKUPS, ApiActions.ADD, "CompanyLookupValues/Add")]
        public async Task<IActionResult> Add([FromBody] CompanyLookupInnerDTO Obj)
        {
            if (Obj.Id != 0)
                return BadRequest();

            var CompanyLookupValueObj = _mapper.Map<CompanyLookupValue>(Obj);

            repositories.CompanyLookupValues.Insert(CompanyLookupValueObj);

            var saveResult = await repositories.Save(UserId);

            if (saveResult.Item1 == true)
                return StatusCode((int)HttpStatusCode.NoContent);


            return StatusCode((int)HttpStatusCode.InternalServerError, saveResult.Item2);

        }


        [ApiVersion("1.0")]
        [HttpPut]
        [Authorize(Auth_Permissions.Company_Lookups.CanAddCompanyLookups)]
        [Permission(RolesNames.COMPANY_LOOKUPS, ApiActions.ADD, "CompanyLookupValues/Update")]
        public async Task<IActionResult> Update([FromBody] CompanyLookupInnerDTO Obj)
        {
            if (Obj?.Id == null || Obj.Id <= 0)
                return StatusCode((int)HttpStatusCode.BadRequest, "record must have id..");

            var CompanyLookupValueObj = repositories.CompanyLookupValues.GetById(Obj.Id);

            CompanyLookupValueObj.ValueEn = Obj.ValueEn;
            CompanyLookupValueObj.ValueAr = Obj.ValueAr;
            CompanyLookupValueObj.CompanyLookupId = Obj.CompanyLookupId;
            CompanyLookupValueObj.Deleted = false;

            repositories.CompanyLookupValues.Update(CompanyLookupValueObj);

            var saveResult = await repositories.Save(UserId);

            if (saveResult.Item1 == true)
                return StatusCode((int)HttpStatusCode.NoContent);


            return StatusCode((int)HttpStatusCode.InternalServerError, saveResult.Item2);
        }


        [ApiVersion("1.0")]
        [HttpDelete]
        [Authorize(Auth_Permissions.Company_Lookups.CanDeleteCompanyLookups)]
        [Permission(RolesNames.COMPANY_LOOKUPS, ApiActions.DELETE, "CompanyLookupValues/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            repositories.CompanyLookupValues.Delete(id);

            var saveResult = await repositories.Save(UserId);

            if (saveResult.Item1)
                return StatusCode((int)HttpStatusCode.NoContent);

            return StatusCode((int)HttpStatusCode.InternalServerError, saveResult.Item2);
        }


        [ApiVersion("1.0")]
        [APIKey]
        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/v1.0/Web/CompanyLookupValues/InquiryTypes")]
        public IActionResult WebGetInquiryTypes(string lang = "en")
        {

            List<CompanyLookupValue> types = repositories.CompanyLookupValues.GetAll(x => x.CompanyLookupId == 2).GetAwaiter().GetResult().ToList();
            if (types == null)
                return NoContent();
            var finalResults = new List<CompanyLookupValueDto>();
            foreach (var item in types)
            {
                var toAdd = new CompanyLookupValueDto()
                {
                    Id = item.Id,
                    Name = lang == "en" ? item.ValueEn : lang == "ar" ? item.ValueAr : ""
                };
                finalResults.Add(toAdd);
            }
            finalResults = finalResults.OrderBy(y => y.Name).ToList();
            return Ok(finalResults);
        }


        [ApiVersion("1.0")]
        [APIKey]
        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/v1.0/Web/CompanyLookupValues")]
        public async Task<IActionResult> GetLookupValues(int? CompanyLookupId = 0, string? lang = "en")
        {
            if (CompanyLookupId == null || CompanyLookupId <= 0)
                return Ok(APIResponse<string>.Fail(lang == "en" ? "please enter valid id" : "الرجاء ادخال معرف صالح",""));

            var Data = await repositories.CompanyLookupValues.SelectAll(
                expression: s =>
                s.CompanyLookupId == CompanyLookupId && s.Visible > 0 &&
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
        [Route("~/api/v1.0/Web/CompanyLookupValues/Filter")]
        public async Task<IActionResult> GetLookupValues([FromBody] List<int?> IDs, string? lang = "en")
        {
            if (IDs == null || IDs.Count == 0)
                return Ok(APIResponse<string>.Fail(lang == "en" ? "please enter valid ids" : "الرجاء ادخال معرفات صالحة", ""));

            var Data = await repositories.CompanyLookupValues.SelectAll(
                 expression: s =>
                 IDs.Contains(s.CompanyLookupId) && s.Visible > 0 &&
                 ((lang == "en" && s.ValueEn != null) || (lang == "ar" && s.ValueAr != null)),
                 select: x => new CompanyLookupValueDTO
                 {
                     Id = x.Id,
                     Value = (lang == "en" ? x.ValueEn : x.ValueAr),
                     CompanyLookupId = x.CompanyLookupId,
                     CompanyLookupName = x.CompanyLookup != null ? x.CompanyLookup.Name : ""
                 },
                 orderBy: order => order.OrderBy(s => s.OrderNb),
                 includeExpressions: new[] { "CompanyLookup" }
                 );

            if (Data == null || Data.Count == 0)
                return Ok(APIResponse<string>.NotFound(lang == "en"));

            var GroupedData = (Data.GroupBy(s => s.CompanyLookupName))?.ToList();
            Dictionary<string, List<LookUpValueDTO>> Result = new Dictionary<string, List<LookUpValueDTO>>();

            if (GroupedData == null || GroupedData.Count == 0)
                return Ok(APIResponse<string>.NotFound(lang == "en"));

            List<LookUpValueDTO> values;
            string CompanyLookUp = "Unknown";


            foreach (var item in GroupedData)
            {
                CompanyLookUp = item.Key == null ? CompanyLookUp : item.Key;
                values = new List<LookUpValueDTO>();

                foreach (var innerItem in item.ToList())
                {
                    values.Add(new LookUpValueDTO
                    {
                        Id = innerItem.Id,
                        Value = innerItem.Value
                    });
                }

                Result.Add(CompanyLookUp, values);
            }
            return Ok(APIResponse<Dictionary<string, List<LookUpValueDTO>>>.Success(Result));


        }
    }
}
