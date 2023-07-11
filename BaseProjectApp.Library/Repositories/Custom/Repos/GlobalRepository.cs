using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.DbSpsResult;
using BaseProjectApp.Library.Repositories.Base;
using BaseProjectApp.Library.Repositories.Custom.Interfaces;
using BaseProjectApp.Library.Templates.DTOs;
using BaseProjectApp.Library.Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static BaseProjectApp.Library.Utility.Enumerations;

namespace BaseProjectApp.Library.Repositories.Custom.Repos
{
    public class GlobalRepository : IGlobalRepository
    {
        protected BaseProjectDBContext Context { get; set; }
        public GlobalRepository(BaseProjectDBContext context)
        {
            this.Context = context;
        }

        public List<LocationsResponse>? GetLocations(string? Lang = "en", string? SearchTerm = "", int? TypeId = 1, int? ParentId = 0)
        {

            Lang = Lang ?? "en";
            SearchTerm = SearchTerm ?? "";
            TypeId = TypeId ?? 1;
            ParentId = ParentId ?? 0;

            List<LocationsResponse>? Data = new List<LocationsResponse>();

            string sql = $"EXEC GetLocations  @SearchTerm = '{SearchTerm}', @Lang = N'{Lang}', @TypeId = {TypeId} , @ParentId = {ParentId}";
             
            Data =
                this.Context
                ?.LocationsData
                ?.FromSqlRaw(sql)
                ?.ToList();

            return Data;

        }
         
        public async Task<List<GlobalSearchDTO>> Search(string Keyword = "", string lang = "en")
        {

            List<GlobalSearchDTO> Data = new List<GlobalSearchDTO>();

            string LowerKeyword = string.IsNullOrWhiteSpace(Keyword) ? "" : Keyword.ToLower();

            //string ConfUrl = Configuration.GetConfigURL();

            //var BlogTypeId = (Context.RecordCategories.FirstOrDefault(s => s.NameEn.ToLower().Contains("blog")))?.Id;

            //BlogTypeId = BlogTypeId ?? (int)RecordsCategoriesEnum.blogs;

            //if (lang == "en")
            //{

            //    //var Publications = Context.Publications.Where(s =>
            //    //            s.Deleted == false &&
            //    //            s.Published == true && (
            //    //            (s.TitleEn != null && s.TitleEn.ToLower().Contains(LowerKeyword)) ||
            //    //            (s.SummaryEn != null && s.SummaryEn.ToLower().Contains(LowerKeyword))
            //    //            )
            //    //            ).Select(rec => new GlobalSearchDTO
            //    //            {
            //    //                Title = rec.TitleEn,
            //    //                Type = "Publication",
            //    //                Link = string.Concat("/publications/", rec.Id),
            //    //            })?.ToList();


            //    var Blogs = Context.Records.Where(s =>
            //    s.CategoryId == BlogTypeId &&
            //    s.Deleted == false &&
            //    s.Published == true && ( 
            //   (s.TitleEn != null && s.TitleEn.ToLower().Contains(LowerKeyword)) ||
            //   (s.SummaryEn != null && s.SummaryEn.ToLower().Contains(LowerKeyword)))
            //    ).Select(rec => new GlobalSearchDTO
            //    {
            //        Title = rec.TitleEn,
            //        Type = "Blog",
            //        Link = string.Concat("/blogs/", (rec.TitleEn == null ? "" : rec.TitleEn.ToLower().Replace("-", " ")), "___", rec.Id),
            //    })?.ToList();


            //    var menus = Context.CustomMenus.Where(s =>
            //        s.Deleted == false &&
            //       (s.LanguageId != null && s.LanguageId.ToLower() == "en") &&
            //       (s.Name != null && s.Name.ToLower().Contains(LowerKeyword))
            //        ).Select(rec => new GlobalSearchDTO
            //        {
            //            Title = rec.Name,
            //            Type = "Page",
            //            Link = rec.PageUrl
            //        })?.ToList();


            //    Data = Blogs.Concat(menus).ToList();


            //}
            //else
            //{

            //    //var Publications = Context.Publications.Where(s =>
            //    //      s.Deleted == false &&
            //    //      s.Published == true && (
            //    //     (s.TitleAr != null && s.TitleAr.ToLower().Contains(LowerKeyword)) ||
            //    //     (s.SummaryAr != null && s.SummaryAr.ToLower().Contains(LowerKeyword)))
            //    //      ).Select(rec => new GlobalSearchDTO
            //    //      {
            //    //          Title = rec.TitleAr,
            //    //          Type = "مطبوع",
            //    //          Link = string.Concat("/publications/", rec.Id),
            //    //      })?.ToList();


            //    var Blogs = Context.Records.Where(s =>
            //         s.CategoryId == BlogTypeId &&
            //         s.Deleted == false &&
            //         s.Published == true && (
            //        (s.TitleAr != null && s.TitleAr.ToLower().Contains(LowerKeyword)) ||
            //        (s.SummaryAr != null && s.SummaryAr.ToLower().Contains(LowerKeyword)))
            //         ).Select(rec => new GlobalSearchDTO
            //         {
            //             Title = rec.TitleAr,
            //             Type = "مدونة",
            //             Link = string.Concat("/blogs/", (rec.TitleAr == null ? "" : rec.TitleAr.ToLower().Replace("-", " ")), "___", rec.Id),
            //         })?.ToList();


            //    var menus = Context.CustomMenus.Where(s =>
            //        s.Deleted == false && 
            //       (s.LanguageId != null && s.LanguageId.ToLower() == "ar") &&
            //       (s.Name != null && s.Name.ToLower().Contains(LowerKeyword))
            //        ).Select(rec => new GlobalSearchDTO
            //        {
            //            Title = rec.Name,
            //            Type = "صفحة",
            //            Link = rec.PageUrl
            //        })?.ToList();

            //    //.Concat(Blogs)
            //    Data = Blogs .Concat(menus).ToList();


            //}

            return Data;

        }
    }
}
