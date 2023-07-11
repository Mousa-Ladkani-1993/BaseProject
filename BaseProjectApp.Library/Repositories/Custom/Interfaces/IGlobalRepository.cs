using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.DbSpsResult;
using BaseProjectApp.Library.Repositories.Base;
using BaseProjectApp.Library.Templates.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseProjectApp.Library.Repositories.Custom.Interfaces
{
    public interface IGlobalRepository
    { 
        Task<List<GlobalSearchDTO>> Search(string Keyword = "", string lang = "en");
        List<LocationsResponse>? GetLocations(string? Lang = "en", string? SearchTerm = "", int? TypeId = 1, int? ParentId = 0);
    }
}
