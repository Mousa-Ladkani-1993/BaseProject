using AutoMapper;
using BaseProjectApp.API.Authentication;
using BaseProjectApp.API.Middlewares;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Templates.DTOs;
using BaseProjectApp.Library.Templates.Responses;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BaseProjectApp.Library.Utility;
using BaseProjectApp.API.Authorization;
using BaseProjectApp.Library.Templates;
using BaseProjectApp.Library.Templates.Enums;
using BaseProjectApp.Library.DbSpsResult;

namespace BaseProjectApp.API.Controllers
{
    [Authorize]
    [Route("Api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IUnitofWork repositories = null;
        private readonly IMapper _mapper;
        private readonly ILogger<LocationsController> _logger;
        private Guid UserId;

        public LocationsController(IUnitofWork repositories, ILogger<LocationsController> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
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
        [Route("Countries")]
        public IActionResult BackGetLocationsCountries(string? SearchTerm = "" , int? ParentId = 0)
        {

            var data = repositories.globalRepo.GetLocations("en", SearchTerm, 1 , ParentId);

            if (data != null && data.Any())
            {
                return Ok(data);
            }

            return NotFound();
        }


        [HttpGet]
        [Route("Cities")]
        public IActionResult BackCities(string? SearchTerm, int? ParentId = 0)
        {
            var data = repositories.globalRepo.GetLocations("en", SearchTerm, 2, ParentId);

            if (data != null && data.Any())
            {
                return Ok(data);
            }

            return NotFound();
        }


        [HttpGet]
        [Route("Areas")]
        public IActionResult BackGetLocationsAreas(string? SearchTerm, int? ParentId = 0)
        {
            var data = repositories.globalRepo.GetLocations("en", SearchTerm, 3, ParentId);

            if (data != null && data.Any())
            {
                return Ok(data);
            }

            return NotFound();
        }

         
        [HttpGet]
        [Route("RelatedLocations")]
        public async Task<IActionResult> RelatedLocations(int Id)
        {

            if (Id <= 0)
                return BadRequest();

            var LocationsList = await repositories.Locations.SelectAll(s => new ListItemN
            {
                Id = s.Id,
                Value = s.NameEn
            }, s => s.ParentId == Id);

            if (LocationsList != null && LocationsList.Any())
            {

                return Ok(LocationsList);
            }

            return NotFound();
        }



        [APIKey_JWT]
        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/Web/Locations/All")]
        public async Task<IActionResult> WebGetLocations(string Lang = "en")
        {

            var LocationsList = await repositories.Locations.SelectAll(s => new LocationNodeDTO
            {

                Id = s.Id,
                Name = Lang == "en" ? s.NameEn : s.NameAr,
                ParentId = s.ParentId,
                TypeId = s.TypeId,
                ISOCode3 = s.Isocode3
            });

            var LocationsTree = TreeBuilder.BuildLocationsTree(LocationsList);


            if (LocationsList != null && LocationsList.Any())
            {
                var Countries = LocationsList.Where(s => s.TypeId == (int)LocationType_Enum.Country).Select(s => new _LocationDTO
                {
                    Id = s.Id,
                    ISOCode3 = s.ISOCode3,
                    Name = s.Name
                });

                var Cities = LocationsList.Where(s => s.TypeId == (int)LocationType_Enum.City).Select(s => new _LocationDTO
                {
                    Id = s.Id,
                    ISOCode3 = s.ISOCode3,
                    Name = s.Name
                });

                var Areas = LocationsList.Where(s => s.TypeId == (int)LocationType_Enum.Area).Select(s => new _LocationDTO
                {
                    Id = s.Id,
                    ISOCode3 = s.ISOCode3,
                    Name = s.Name
                });

                 
                return Ok(APIResponse<Object>.Success(
                    new
                    {
                        Locations = LocationsTree,
                        Countries = Countries,
                        Cities = Cities,
                        Areas = Areas
                    }
                    ));
            }

            return Ok(APIResponse<Object>.NotFound(Lang == "en"));
        }



        [APIKey_JWT]
        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/Web/Locations/All/Countries")]
        public IActionResult GetLocationsCountries(string? SearchTerm, string? Lang, int? ParentId = 0)
        { 

            var data = repositories.globalRepo.GetLocations(Lang, SearchTerm, 1 , ParentId);

            if (data != null && data.Any())
            {
                return Ok(APIResponse<List<LocationsResponse>>.Success(data));
            }

            return Ok(APIResponse<Object>.NotFound(Lang == "en"));
        }


        [APIKey_JWT]
        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/Web/Locations/All/Cities")]
        public IActionResult Cities(string? SearchTerm, string? Lang , int? ParentId = 0 )
        {
            var data = repositories.globalRepo.GetLocations(Lang, SearchTerm, 2 , ParentId);

            if (data != null && data.Any())
            {
                return Ok(APIResponse<List<LocationsResponse>>.Success(data));
            }

            return Ok(APIResponse<Object>.NotFound(Lang == "en"));
        }


        [APIKey_JWT]
        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/Web/Locations/All/Areas")]
        public IActionResult GetLocationsAreas(string? SearchTerm, string? Lang, int? ParentId = 0)
        {
            var data = repositories.globalRepo.GetLocations(Lang, SearchTerm, 3 , ParentId);

            if (data != null && data.Any())
            {
                return Ok(APIResponse<List<LocationsResponse>>.Success(data));
            } 

            return Ok(APIResponse<Object>.NotFound(Lang == "en"));
        }

    }
}
