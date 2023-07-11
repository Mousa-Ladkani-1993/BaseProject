using AutoMapper;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.DbSpsResult;
using BaseProjectApp.Library.Templates.DTOs;
using BaseProjectApp.Library.Templates.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {



            CreateMap<MobileCustomMenuDTO, MobileCustomMenuNode>().ReverseMap(); 
            CreateMap<PropertySpRes_lbl, PropertySpRes>().ReverseMap(); 
            CreateMap<LookupValue, LookupValueReponse>().ReverseMap(); 
            CreateMap<MediaFile, MediaFileResponse>()
                .ForPath(
                 dest => dest.FilePath,
                 opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.FilePath) ? "" : src.FilePath.Insert(0, "..")))

                .ForPath(
                 dest => dest.CaptionEn,
                 opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.CaptionEn) ? "No Caption" : src.CaptionEn));


            CreateMap<MediaFile, MediaFilePublicationResponse>().ForPath(
                 dest => dest.Language,
                 opt => opt.MapFrom(src => src.LanguageId == "ar" ? "Arabic" : "English"));
             
            CreateMap<AspNetUser, UserResponse>();

            CreateMap<CompanyLookupInnerDTO, CompanyLookupValue>();
            CreateMap<CompanyLookupValue, CompanyLookupInnerDTO>().ForPath(
                 dest => dest.CompanyLookupName,
                 opt => opt.MapFrom(src => src.CompanyLookup == null ? "" : src.CompanyLookup.Name));


            CreateMap<LookupInnerDTO, LookupValue>();
            CreateMap<LookupValue, LookupInnerDTO>().ForPath(
                 dest => dest.LookupName,
                 opt => opt.MapFrom(src => src.Lookup == null ? "" : src.Lookup.Name));


               

        }
    }
}
