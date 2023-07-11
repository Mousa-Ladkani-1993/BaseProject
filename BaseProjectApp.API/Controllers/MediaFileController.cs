using AutoMapper;
using BaseProjectApp.API.Authorization;
using BaseProjectApp.API.Middlewares;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Templates.DTOs;
using BaseProjectApp.Library.Templates.Responses;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using BaseProjectApp.Library.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BaseProjectApp.API.Controllers
{
    //[Authorize]
    [Route("Api/MediaFile")]
    [ApiController]
    public class MediaFileController : ControllerBase
    {
        private readonly IUnitofWork repositories = null;
        private readonly IMapper _mapper;
        private readonly ILogger<MediaFileController> _logger;
        private readonly IConfiguration _configuration;
        private Guid UserId;

        public MediaFileController(IUnitofWork repositories, ILogger<MediaFileController> logger, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.repositories = repositories;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;

            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = httpContextAccessor.HttpContext.User.FindFirst(x => x.Type.Contains("UserId")).Value;
                UserId = Guid.Parse(userId);
            }
        }

        [HttpGet]
        [Route("Get")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/Get")]
        public IActionResult GetMediaFile(int id)
        {
            var result = repositories.MediaFiles.GetById(id);

            if (result == null)
                return NotFound();
            result.FilePath = result.FilePath?.Insert(0, "..");

            return Ok(result);
        }

        [HttpGet]
        [Route("GetRecordFiles")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetRecordFiles")]
        public async Task<IActionResult> GetRecordMediaFiles(int recordId)
        {
            var result = await repositories.MediaFiles.GetAll(item => item.RecordId == recordId, order => order.OrderBy(s => s.DisplayOrder));

            if (result == null)
                return NotFound();

            //foreach (var item in result)
            //{
            //    item.FilePath = item.FilePath?.Insert(0, "..");
            //}

            return Ok(_mapper.Map<IEnumerable<MediaFileResponse>>(result));
        }


        [HttpGet]
        [Route("GetPropertyFiles")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetPropertyFiles")]
        public async Task<IActionResult> GetPropertyFiles(int PropertyId)
        {
            var result = await repositories.MediaFiles.GetAll(item => item.PropertyId == PropertyId, order => order.OrderBy(s => s.DisplayOrder));

            if (result == null)
                return NotFound();

            //foreach (var item in result)
            //{
            //    item.FilePath = item.FilePath?.Insert(0, "..");
            //}

            return Ok(_mapper.Map<IEnumerable<MediaFileResponse>>(result));
        }


        [HttpGet]
        [Route("GetProjectFiles")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetProjectFiles")]
        public async Task<IActionResult> GetProjectMediaFiles(int projectId)
        {
            var result = await repositories.MediaFiles.GetAll(item => item.ProjectId == projectId, order => order.OrderBy(s => s.DisplayOrder));

            if (result == null)
                return NotFound();

            //foreach (var item in result)
            //{
            //    item.FilePath = item.FilePath?.Insert(0, "..");
            //}

            return Ok(_mapper.Map<IEnumerable<MediaFileResponse>>(result));
        }

        [HttpGet]
        [Route("GetProjectTimelineFiles")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetProjectTimelineFiles")]
        public async Task<IActionResult> GetProjectTimelineMediaFiles(int projectTimelineId)
        {
            var result = await repositories.MediaFiles.GetAll(item => item.ProjectTimelineId == projectTimelineId, order => order.OrderBy(s => s.DisplayOrder));

            if (result == null)
                return NotFound();

            //foreach (var item in result)
            //{
            //    item.FilePath = item.FilePath?.Insert(0, "..");
            //}

            return Ok(_mapper.Map<IEnumerable<MediaFileResponse>>(result));
        }

        [HttpGet]
        [Route("FilterMediaFiles")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/FilterMediaFiles")]
        public async Task<IActionResult> FilterMediaFiles(string? Name, int? TypeId)
        {
            var result = await repositories.MediaFiles.GetAll(item => (Name == "" || Name == null || item.FileName.Contains(Name) ||
                                              item.CaptionEn.Contains(Name) || item.CaptionAr.Contains(Name)) &&
                                              (TypeId == null || item.TypeId == TypeId), order => order.OrderBy(s => s.DisplayOrder));
 
            //foreach (var item in result)
            //{
            //    item.FilePath = item.FilePath?.Insert(0, "..");
            //}

            if (result == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<MediaFileResponse>>(result));
        }

        [HttpPost]
        [Route("UploadSelectedCollection")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.ADD, "MediaFile/UploadSelectedCollection")]
        public async Task<IActionResult> UploadSelectedCollection(MediaDto collection)
        {
            if (collection.ListOfIds != null && collection.ListOfIds.Trim() != "" && collection.ListOfIds.Split('-').Count() > 0)
            {
                string[] IdsList = collection.ListOfIds.Split('-');
                var order = collection.DisplayOrder;
                foreach (var item in IdsList)
                {
                    if (int.TryParse(item, out int id))
                    {
                        var media = repositories.MediaFiles.GetById(id);
                        MediaFile newMediaFile = new MediaFile()
                        {
                            FilePath = media.FilePath,
                            FileName = media.FileName,
                            CreationDate = DateTime.Now,
                            CaptionAr = collection.CaptionAr,
                            CaptionEn = collection.CaptionEn,
                            YouTubePath = collection.YoutubePath,
                            Deleted = false,
                            DisplayOrder = order++,
                            MainImage = collection.MainImage,
                            TypeId = collection.MediaType
                        };
                        if (collection.Table == "RecordImage")
                            newMediaFile.RecordId = Convert.ToInt32(collection.RecordId);

                        repositories.MediaFiles.Insert(newMediaFile);
                        await repositories.Save(UserId);
                    }
                }
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GetInfoFiles")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetInfoFiles")]
        public async Task<IActionResult> GetInfoFiles(int recordId)
        {
            var result = await repositories.MediaFiles.GetAll(item => item.UserInfoId == recordId, order => order.OrderByDescending(s => s.CreationDate));

            if (result == null)
                return NotFound();

            //foreach (var item in result)
            //{
            //    item.FilePath = item.FilePath?.Insert(0, "..");
            //}

            return Ok(_mapper.Map<IEnumerable<MediaFileResponse>>(result));
        }
        [HttpGet]
        [Route("GetCareerFiles")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetCareerFiles")]
        public async Task<IActionResult> GetCareerFiles(int CareerId)
        {
            var result = await repositories.MediaFiles.GetAll(item => item.CareerId == CareerId, order => order.OrderByDescending(s => s.CreationDate));

            if (result == null)
                return NotFound();

            //foreach (var item in result)
            //{
            //    item.FilePath = item.FilePath?.Insert(0, "..");
            //} 

            return Ok(_mapper.Map<IEnumerable<MediaFileResponse>>(result));
        }

        [HttpGet]
        [Route("GetSliderImage")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetSliderImage")]
        public async Task<IActionResult> GetSliderImage(int sliderId)
        {
            var result = await repositories.MediaFiles.GetAll(item => item.SliderId == sliderId, order => order.OrderByDescending(s => s.CreationDate));

            if (result == null)
                return NotFound();

            foreach (var item in result)
            {
                item.FilePath = item.FilePath?.Insert(0, "..");
            }

            return Ok(result);
        }
        [HttpGet]
        [Route("GetApplicantFile")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetApplicantFile")]
        public async Task<IActionResult> GetApplicantFile(int applicantId)
        {
            var result = await repositories.MediaFiles.GetAll(item => item.ApplicantId == applicantId && item.TypeId == 3, order => order.OrderByDescending(s => s.CreationDate));

            if (result == null)
                return NotFound();

            foreach (var item in result)
            {
                item.FilePath = item.FilePath?.Insert(0, "..");
            }

            return Ok(result);
        }
        [HttpGet]
        [Route("GetCareerImage")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetCareerImage")]
        public async Task<IActionResult> GetCareerImage(int careerId)
        {
            var result = await repositories.MediaFiles.GetAll(item => item.CareerId == careerId, order => order.OrderByDescending(s => s.CreationDate));

            if (result == null)
                return NotFound();

            foreach (var item in result)
            {
                item.FilePath = item.FilePath?.Insert(0, "..");
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("GetCategoryImage")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetCategoryImage")]
        public IActionResult GetCategoryImage(int categoryId)
        {
            var result = repositories.MediaFiles.GetByIdWithPredicate(item => item.CategoryId == categoryId);

            if (result == null)
                return NotFound();

            result.FilePath = result.FilePath?.Insert(0, "..");

            return Ok(result);
        }

        [HttpGet]
        [Route("GetUserProfileImage")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetUserProfileImage")]
        public IActionResult GetUserProfileImage()
        {
            var result = repositories.MediaFiles.GetByIdWithPredicate(item => item.UserProfileId == UserId.ToString());

            if (result == null)
                return NotFound();

            result.FilePath = result.FilePath?.Insert(0, "..");

            return Ok(result);
        }

        [HttpGet]
        [Route("GetUserProfileImageById")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetUserProfileImageById")]
        public IActionResult GetUserProfileImageById(string userId)
        {
            var result = repositories.MediaFiles.GetByIdWithPredicate(item => item.UserProfileId == userId);

            if (result == null)
                return NotFound();

            result.FilePath = result.FilePath?.Insert(0, "..");

            return Ok(result);
        }

        [HttpGet]
        [Route("GetPartnerDonorImageById")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetPartnerDonorImageById")]
        public IActionResult GetPartnerDonorImageById(int partnerDonorId)
        {
            var result = repositories.MediaFiles.GetByIdWithPredicate(item => item.PartnerDonorId == partnerDonorId);

            if (result == null)
                return NotFound();

            result.FilePath = result.FilePath?.Insert(0, "..");

            return Ok(result);
        }


        [HttpGet]
        [Route("GetPublicationFiles")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetPublicationFiles")]
        public async Task<IActionResult> GetPublicationFiles(int Id)
        {
            var result = await repositories.MediaFiles.GetAll(item => item.PublicationId == Id, order => order.OrderBy(s => s.DisplayOrder));

            if (result == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<MediaFilePublicationResponse>>(result));
        }

        [HttpGet]
        [Route("GetCountryFiles")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetCountryFiles")]
        public async Task<IActionResult> GetCountryFiles(int countryId)
        {
            var result = await repositories.MediaFiles.GetAll(item => item.CountryId == countryId, order => order.OrderBy(s => s.DisplayOrder));

            if (result == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<MediaFileResponse>>(result));
        }


        [HttpGet]
        [Route("GetTopicFiles")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetTopicFiles")]
        public async Task<IActionResult> GetTopicFiles(int TopicId)
        {
            var result = await repositories.MediaFiles.GetAll(item => item.TopicId == TopicId, order => order.OrderBy(s => s.DisplayOrder));

            if (result == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<MediaFileResponse>>(result));
        }



        [HttpGet]
        [Route("GetProjectTimeLineMediaFile")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetProjectTimeLineMediaFile")]
        public IActionResult GetProjectTimeLineMediaFile(int ProjectTimelineId)
        {
            var result = repositories.MediaFiles.GetByIdWithPredicate(item => item.ProjectTimelineId == ProjectTimelineId);

            if (result == null)
                return NotFound();

            result.FilePath = result.FilePath?.Insert(0, "..");

            return Ok(result);
        }
        [HttpGet]
        [Route("GetApplicantFileById")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetApplicantFileById")]
        public IActionResult GetApplicantFileById(int ApplicantId)
        {
            var result = repositories.MediaFiles.GetByIdWithPredicate(item => item.ApplicantId == ApplicantId);

            if (result == null)
                return NotFound();

            result.FilePath = result.FilePath?.Insert(0, "..");

            return Ok(result);
        }
        [HttpGet]
        [Route("GetTeamMemberImageById")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetTeamMemberImageById")]
        public IActionResult GetTeamMemberImageById(int teamMemberId)
        {
            var result = repositories.MediaFiles.GetByIdWithPredicate(item => item.TeamMemberId == teamMemberId);

            if (result == null)
                return NotFound();

            result.FilePath = result.FilePath?.Insert(0, "..");

            return Ok(result);
        }


        [HttpGet]
        [Route("GetJobVacanciesFiles")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.VIEW, "MediaFile/GetJobVacanciesFiles")]
        public IActionResult GetJobVacanciesFiles(int jobVacancyId)
        {
            var result = repositories.MediaFiles.GetByIdWithPredicate(item => item.JobVacancyId == jobVacancyId);

            if (result == null)
                return NotFound();

            result.FilePath = result.FilePath?.Insert(0, "..");

            return Ok(result);
        }

        [HttpPost]
        [Route("Save")]
        [Authorize(Auth_Permissions.Media_files.CanAddMediaFiles)]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.ADD, "MediaFile/Save")]
        public async Task<IActionResult> Save(int Id, string CaptionEn, string CaptionAr, int DisplayOrder, string YoutubePath, bool mainImage)
        {
            MediaFile myFile = new MediaFile();

            if (Id > 0)
                myFile = repositories.MediaFiles.GetById(Id);

            myFile.CaptionAr = CaptionAr;
            myFile.CaptionEn = CaptionEn;
            myFile.YouTubePath = YoutubePath;
            myFile.DisplayOrder = DisplayOrder;
            myFile.MainImage = mainImage;

            if (Id > 0)
                repositories.MediaFiles.Update(myFile);
            else
                repositories.MediaFiles.Insert(myFile);

            var res = await repositories.Save(UserId);

            if (res.Item1 == true)
                return NoContent();
            else
                return BadRequest(res.Item2);
        }

        [HttpPost]
        [Route("Delete")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.DELETE, "MediaFile/Delete")]
        [Authorize(Auth_Permissions.Media_files.CanDeleteMediaFiles)]
        public async Task<IActionResult> Delete(int id)
        {
            MediaFile myFile = repositories.MediaFiles.GetById(id);
            myFile.Deleted = true;

            repositories.MediaFiles.Update(myFile);

            var res = await repositories.Save(UserId);
            if (res.Item1 == true)
                return NoContent();
            else
                return BadRequest(res.Item2);
        }

        [HttpPost]
        [Route("SaveFile")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.ADD, "MediaFile/SaveFile")]
        [Authorize(Auth_Permissions.Media_files.CanAddMediaFiles)]
        public async Task<IActionResult> SaveFile(int mediaId, int recordId, string? caption, string? youTube, int? displayOrder, bool mainImage, int mediaType, string? type, bool? isHorizontal, bool? isIcon,
            string? duplicateIds, IList<IFormFile> files, string? LanguageId = "")
        {
            foreach (IFormFile source in files)
            {
                string dateString = DateTime.Now.Ticks.ToString();
                var DocPath = "/Media/Default/" + source.FileName.Insert(source.FileName.IndexOf("."), "_" + dateString);
                string path = Path.Combine(DocPath);

                string filename = source.FileName.Insert(source.FileName.IndexOf("."), "_" + dateString);
                string filepath = Path.Combine(_configuration["MediaSettings:MEDIAFILESURL"], "Media", "Default") + $@"\{filename}";

                using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
                {
                    source.CopyTo(fs);
                    fs.Flush();
                }

                MediaFile myFile = new MediaFile();
                if (mediaId > 0)
                    myFile = repositories.MediaFiles.GetById(mediaId);

                if (type == "JobVacancyImage")
                    myFile.JobVacancyId = Convert.ToInt32(recordId);
                if (type == "ProjectImage")
                    myFile.ProjectId = Convert.ToInt32(recordId);
                if (type == "ProjectTimelineImage")
                    myFile.ProjectTimelineId = Convert.ToInt32(recordId);
                if (type == "RecordImage")
                    myFile.RecordId = Convert.ToInt32(recordId);
                else if (type == "UserInfoFile")
                    myFile.UserInfoId = Convert.ToInt32(recordId);
                else if (type == "EventFile")
                    myFile.EventId = Convert.ToInt32(recordId);
                else if (type == "ProductFile")
                    myFile.ProductId = Convert.ToInt32(recordId);
                else if (type == "BrandImage")
                    myFile.BrandId = Convert.ToInt32(recordId);
                else if (type == "SizeChartImage")
                    myFile.SizeChartId = Convert.ToInt32(recordId);
                else if (type == "CommunityFiles")
                    myFile.CommunityId = Convert.ToInt32(recordId);
                else if (type == "CommunityPostFiles")
                    myFile.CommunityPostId = Convert.ToInt32(recordId);
                else if (type == "SupplierImage")
                    myFile.SupplierId = Convert.ToInt32(recordId);
                else if (type == "SupplierDocumentImage")
                    myFile.SupplierDocumentId = Convert.ToInt32(recordId);
                else if (type == "CareerImage")
                    myFile.CareerId = Convert.ToInt32(recordId);
                else if (type == "Publication")
                    myFile.PublicationId = Convert.ToInt32(recordId);
                else if (type == "CountryImage")
                    myFile.CountryId = Convert.ToInt32(recordId);
                else if (type == "TopicImage")
                    myFile.TopicId = Convert.ToInt32(recordId);
                else if (type == "PropertyFile")
                    myFile.PropertyId = Convert.ToInt32(recordId);



                myFile.FilePath = path;
                myFile.FileName = filename;
                myFile.CaptionEn = caption;
                myFile.CaptionAr = "";
                myFile.YouTubePath = youTube;
                myFile.DisplayOrder = displayOrder;
                myFile.TypeId = mediaType;
                myFile.CreationDate = DateTime.Now;
                myFile.MainImage = mainImage;
                myFile.IsHorizontal = isHorizontal;
                myFile.IsIcon = isIcon;
                myFile.Deleted = false;
                myFile.LanguageId = string.IsNullOrWhiteSpace(LanguageId) || LanguageId == "0" ? null : LanguageId;


                if (myFile.Id > 0)
                    repositories.MediaFiles.Update(myFile);
                else
                    repositories.MediaFiles.Insert(myFile);

                await repositories.Save(UserId);
            }

            if (duplicateIds == null) return NoContent();

            duplicateIds
            .Split(',')
            .Select(id => repositories.MediaFiles.GetById(Convert.ToInt32(id)))
            .Select(f =>
            {
                repositories.MediaFiles.Detach(f);

                f.RecordId = f.UserInfoId = f.EventId = f.ProductId = f.BrandId = f.SizeChartId = f.CommunityId
                = f.CommunityPostId = f.SupplierId = f.SupplierDocumentId = f.SupplierLogo = null;


                if (type == "RecordImage")
                    f.RecordId = Convert.ToInt32(recordId);
                else if (type == "UserInfoFile")
                    f.UserInfoId = Convert.ToInt32(recordId);
                else if (type == "EventFile")
                    f.EventId = Convert.ToInt32(recordId);
                else if (type == "ProductFile")
                    f.ProductId = Convert.ToInt32(recordId);
                else if (type == "BrandImage")
                    f.BrandId = Convert.ToInt32(recordId);
                else if (type == "SizeChartImage")
                    f.SizeChartId = Convert.ToInt32(recordId);
                else if (type == "CommunityFiles")
                    f.CommunityId = Convert.ToInt32(recordId);
                else if (type == "CommunityPostFiles")
                    f.CommunityPostId = Convert.ToInt32(recordId);
                else if (type == "SupplierImage")
                    f.SupplierId = Convert.ToInt32(recordId);
                else if (type == "SupplierDocumentImage")
                    f.SupplierDocumentId = Convert.ToInt32(recordId);
                else if (type == "CareerImage")
                    f.CareerId = Convert.ToInt32(recordId);
                else if (type == "ProjectTimelineImage")
                    f.ProjectTimelineId = Convert.ToInt32(recordId);
                else if (type == "Publication")
                    f.PublicationId = Convert.ToInt32(recordId);
                else if (type == "CountryImage")
                    f.CountryId = Convert.ToInt32(recordId);
                else if (type == "TopicImage")
                    f.TopicId = Convert.ToInt32(recordId);
                else if (type == "PropertyFile")
                    f.PropertyId = Convert.ToInt32(recordId);

                return f;
            })
            .ToList()
            .ForEach(f => repositories.MediaFiles.Insert(f));

            await repositories.Save(UserId);


            return NoContent();
        }



        [HttpDelete]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.EDIT, "MediaFiles/Remove")]
        [Authorize(Auth_Permissions.Media_files.CanDeleteMediaFiles)]
        public async Task<ActionResult> _RemoveFile(int id)
        {
            try
            {
                var MediaFile = repositories.MediaFiles.GetById(id);

                string? filePath = MediaFile.FilePath;

                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    string FullfilePath = _configuration["MediaSettings:MEDIAFILESURL"].Replace(@"\\", "/") + filePath;

                    System.IO.File.Delete(FullfilePath);

                    if (!System.IO.File.Exists(FullfilePath))
                    {
                        repositories.MediaFiles.Delete(id);
                        var res1 = await repositories.Save(UserId);

                        return NoContent();
                    }
                }

                repositories.MediaFiles.Delete(id);
                var res = await repositories.Save(UserId);

                return BadRequest();
            }
            catch (System.Exception ex)
            {

                return Ok();

            }
        }



        [Route("_SaveFile")]
        //[RequestSizeLimit(50_000_000)]
        [HttpPost]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.EDIT, "MediaFiles/Import")]
        [Authorize(Auth_Permissions.Media_files.CanAddMediaFiles)]
        public async Task<ActionResult> _SaveFile()
        {
            IFormFile file = null;

            try
            {
                file = HttpContext.Request?.Form?.Files[0];

                string dateString = DateTime.Now.Ticks.ToString();
                var DocPath = "/Media/Default/" + file.FileName.Insert(file.FileName.IndexOf("."), "_" + dateString);
                string path = Path.Combine(DocPath);

                string filename = file.FileName.Insert(file.FileName.IndexOf("."), "_" + dateString);
                string filepath = Path.Combine(_configuration["MediaSettings:MEDIAFILESURL"], "Media", "Default") + $@"\{filename}";

                using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                MediaFile myFile = new MediaFile();

                myFile.FilePath = path;
                myFile.FileName = filename;
                myFile.CaptionEn = "";
                myFile.CaptionAr = "";
                myFile.DisplayOrder = 0;
                myFile.CreationDate = DateTime.Now;
                myFile.MainImage = false;
                myFile.Deleted = false;


                if (myFile.Id > 0)
                    repositories.MediaFiles.Update(myFile);
                else
                    repositories.MediaFiles.Insert(myFile);

                await repositories.Save(UserId);


                return Ok(new { url = path, id = myFile.Id });

            }
            catch (System.Exception ex)
            {
                string message = ex.Message;

                if (ex.InnerException != null)
                {
                    message += "......." + ex.InnerException.Message;
                }
                return Ok(message);
            }

        }


        //[HttpDelete]
        //[Permission(RolesNames.MEDIA_FILES, ApiActions.EDIT, "MediaFiles/Remove")]
        //[Authorize(Auth_Permissions.Media_files.CanDeleteMediaFiles)]
        //public async Task<ActionResult> _RemoveFile(int id)
        //{

        //    repositories.MediaFiles.Delete(id);

        //    var res = await repositories.Save(UserId);

        //    if (res.Item1 == true)
        //        return NoContent();
        //    else
        //        return BadRequest(res.Item2);
        //}



        //[Route("_SaveFile")]
        //[HttpPost]
        //[Permission(RolesNames.MEDIA_FILES, ApiActions.EDIT, "MediaFiles/Import")]
        //[Authorize(Auth_Permissions.Media_files.CanAddMediaFiles)]
        //public async Task<ActionResult> _SaveFile()
        //{
        //    IFormFile file = null;

        //    try
        //    {
        //        file = HttpContext.Request?.Form?.Files[0];

        //        string dateString = DateTime.Now.Ticks.ToString();
        //        var DocPath = "/Media/Default/" + file.FileName.Insert(file.FileName.IndexOf("."), "_" + dateString);
        //        string path = Path.Combine(DocPath);

        //        string filename = file.FileName.Insert(file.FileName.IndexOf("."), "_" + dateString);
        //        string filepath = Path.Combine(_configuration["MediaSettings:MEDIAFILESURL"], "Media", "Default") + $@"\{filename}";

        //        using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
        //        {
        //            file.CopyTo(fs);
        //            fs.Flush();
        //        }

        //        MediaFile myFile = new MediaFile();

        //        myFile.FilePath = path;
        //        myFile.FileName = filename;
        //        myFile.CaptionEn = "";
        //        myFile.CaptionAr = "";
        //        myFile.DisplayOrder = 0;
        //        myFile.CreationDate = DateTime.Now;
        //        myFile.MainImage = false;
        //        myFile.Deleted = false;


        //        if (myFile.Id > 0)
        //            repositories.MediaFiles.Update(myFile);
        //        else
        //            repositories.MediaFiles.Insert(myFile);

        //        await repositories.Save(UserId);


        //        return Ok(new { url = path, id = myFile.Id });

        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.Message;

        //        if (ex.InnerException != null)
        //        {
        //            message += "......." + ex.InnerException.Message;
        //        }
        //        return Ok(message);
        //    }

        //}


        [HttpPost]
        [Route("SaveTinyMceFile")]
        [Permission(RolesNames.MEDIA_FILES, ApiActions.ADD, "MediaFile/SaveTinyMceFile")]
        public IActionResult SaveTinyMceFile(IFormFile file)
        {
            string dateString = DateTime.Now.Ticks.ToString();
            var DocPath = "../Media/Default/" + file.FileName.Insert(file.FileName.IndexOf("."), "_" + dateString);
            var RealDocPath = "/Media/Default/" + file.FileName.Insert(file.FileName.IndexOf("."), "_" + dateString);

            string path = Path.Combine(DocPath);
            string Realpath = Path.Combine(RealDocPath);

            string filename = file.FileName.Insert(file.FileName.IndexOf("."), "_" + dateString);
            string filepath = Path.Combine(_configuration["MediaSettings:MEDIAFILESURL"], "Media", "Default") + $@"\{filename}";

            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            string configurl = Configuration.GetConfigURL();
            string FullUrl = Configuration.GenerateURL(Realpath, configurl);

            return Ok(FullUrl);
        }





        public class FileUploadModel
        {
            [DataMember(Name = "fileName")]
            public string FileName { get; set; }

            [DataMember(Name = "fileBytes")]
            public byte[] FileBytes { get; set; }
        }

    }
}
