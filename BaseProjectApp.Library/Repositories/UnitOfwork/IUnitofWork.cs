using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Repositories.Base;
using BaseProjectApp.Library.Repositories.Custom.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Repositories.UnitOfwork
{
    public interface IUnitofWork
    {
        //Generic Repositories
        #region 
        IGenericRepository<MobileCustomMenu> MobileCustomMenus { get; }  
        IGenericRepository<SystemException> Exceptions { get; } 
        IGenericRepository<TextVar> TextVars { get; } 
        IGenericRepository<Location> Locations { get; }
        IGenericRepository<UserPermission> UserPermissions { get; }
        IGenericRepository<AspNetRole> AspNetRoles { get; } 
        IGenericRepository<LookupValue> LookupValues { get; }
        IGenericRepository<MediaFile> MediaFiles { get; }
        IGenericRepository<SystemParameter> SystemParameters { get; }
        IGenericRepository<CompanyLookup> CompanyLookups { get; } 
        IGenericRepository<CompanyLookupValue> CompanyLookupValues { get; }
        IGenericRepository<AspNetUser> AspNetUsers { get; }
        IGenericRepository<Lookup> Lookups { get; }
        IGenericRepository<UserLog> UserLogs { get; }
        #endregion

        //Custom Repositories
        #region  
        IGlobalRepository globalRepo { get; }
        IIdentityRepository IdentityRepo { get; } 
        #endregion

        Task<Tuple<bool, string>> Save(Guid UserId);
        Task<Tuple<bool, string>> Save();
    }
}
