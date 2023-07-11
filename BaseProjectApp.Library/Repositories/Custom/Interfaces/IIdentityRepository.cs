using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.DbSpsResult;
using BaseProjectApp.Library.Repositories.Base;
using BaseProjectApp.Library.Templates;
using BaseProjectApp.Library.Templates.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseProjectApp.Library.Repositories.Custom.Interfaces
{
    public interface IIdentityRepository
    {
        List<ListItemNUser>? PortalUsers();
        List<string>? GetPortalUsers();
        //Task<List<GlobalSearchDTO>> 
        Task<List<AspNetRole>> GetFullUserRoles(string UserId = "");
        Task<bool> DeleteUserRoles(string? userId = "");
    }
}
