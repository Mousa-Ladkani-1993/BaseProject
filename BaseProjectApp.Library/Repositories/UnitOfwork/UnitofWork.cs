using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Repositories.Base;
using BaseProjectApp.Library.Repositories.Custom.Interfaces;
using BaseProjectApp.Library.Repositories.Custom.Repos;
using BaseProjectApp.Library.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Repositories.UnitOfwork
{
    public class UnitofWork : IUnitofWork, IDisposable
    {
        private bool disposed = false;
        protected readonly BaseProjectDBContext _context;

        //Generic Repositories init
        #region     

        private IGenericRepository<MobileCustomMenu> MobileCustomMenusRepository;  
        private IGenericRepository<SystemException> ExceptionsRepository;  
        private IGenericRepository<TextVar> TextVarsRepository; 
        private IGenericRepository<Location> LocationsRepository;
        private IGenericRepository<UserPermission> userPermissionsRepository;
        private IGenericRepository<AspNetRole> aspNetRolesRepository;
        private IGenericRepository<Lookup> lookupsRepository; 
        private IGenericRepository<LookupValue> lookupValueRepository;
        private IGenericRepository<MediaFile> mediaFilesRepository;
        private IGenericRepository<SystemParameter> systemParameterRepository;
        private IGenericRepository<CompanyLookup> companyLookupRepository;
        private IGenericRepository<CompanyLookupValue> companyLookupValueRepository;
        private IGenericRepository<AspNetUser> aspNetUserRepository;
        private IGenericRepository<UserLog> userLogsRepository;


        #endregion

        //Custom Repositories init
        #region   
        private IGlobalRepository GlobalRepository;
        private IIdentityRepository IdentityRepository; 
        #endregion

        //Generic Repositories getters
        #region  
        public IGenericRepository<MobileCustomMenu> MobileCustomMenus
        {
            get
            {
                if (MobileCustomMenusRepository == null)
                    MobileCustomMenusRepository = new GenericRepository<MobileCustomMenu>(_context);
                return MobileCustomMenusRepository;
            }
        }  
        public IGenericRepository<SystemException> Exceptions 
        {
            get
            {
                if (ExceptionsRepository == null)
                    ExceptionsRepository = new GenericRepository<SystemException>(_context);
                return ExceptionsRepository;
            }

        }
           
        public IGenericRepository<TextVar> TextVars
        {
            get
            {
                if (TextVarsRepository == null)
                    TextVarsRepository = new GenericRepository<TextVar>(_context);
                return TextVarsRepository;
            }
        }      
        public IGenericRepository<Location> Locations
        {
            get
            {
                if (LocationsRepository == null)
                    LocationsRepository = new GenericRepository<Location>(_context);
                return LocationsRepository;
            }
        }
        public IGenericRepository<UserPermission> UserPermissions
        {
            get
            {
                if (userPermissionsRepository == null)
                    userPermissionsRepository = new GenericRepository<UserPermission>(_context);
                return userPermissionsRepository;
            }
        }
        public IGenericRepository<UserLog> UserLogs
        {
            get
            {
                if (userLogsRepository == null)
                    userLogsRepository = new GenericRepository<UserLog>(_context);
                return userLogsRepository;
            }
        }
        public IGenericRepository<AspNetRole> AspNetRoles
        {
            get
            {
                if (aspNetRolesRepository == null)
                    aspNetRolesRepository = new GenericRepository<AspNetRole>(_context);
                return aspNetRolesRepository;
            }
        }
        public IGenericRepository<Lookup> Lookups
        {
            get
            {
                if (lookupsRepository == null)
                    lookupsRepository = new GenericRepository<Lookup>(_context);
                return lookupsRepository;
            }
        } 
        public IGenericRepository<LookupValue> LookupValues
        {
            get
            {
                if (lookupValueRepository == null)
                    lookupValueRepository = new GenericRepository<LookupValue>(_context);
                return lookupValueRepository;
            }
        }
        public IGenericRepository<MediaFile> MediaFiles
        {
            get
            {
                if (mediaFilesRepository == null)
                    mediaFilesRepository = new GenericRepository<MediaFile>(_context);
                return mediaFilesRepository;
            }
        }
        public IGenericRepository<SystemParameter> SystemParameters
        {
            get
            {
                if (systemParameterRepository == null)
                    systemParameterRepository = new GenericRepository<SystemParameter>(_context);
                return systemParameterRepository;
            }
        }
        public IGenericRepository<CompanyLookup> CompanyLookups
        {
            get
            {
                if (companyLookupRepository == null)
                    companyLookupRepository = new GenericRepository<CompanyLookup>(_context);
                return companyLookupRepository;
            }
        }
        public IGenericRepository<CompanyLookupValue> CompanyLookupValues
        {
            get
            {
                if (companyLookupValueRepository == null)
                    companyLookupValueRepository = new GenericRepository<CompanyLookupValue>(_context);
                return companyLookupValueRepository;
            }
        }
        //public IGenericRepository<AspNetUserRole> AspNetUserRoles
        //{
        //    get
        //    {
        //        if (aspNetUserRoleRepository == null)
        //            aspNetUserRoleRepository = new GenericRepository<AspNetUserRole>(_context);
        //        return aspNetUserRoleRepository;
        //    }
        //}
        public IGenericRepository<AspNetUser> AspNetUsers
        {
            get
            {
                if (aspNetUserRepository == null)
                    aspNetUserRepository = new GenericRepository<AspNetUser>(_context);
                return aspNetUserRepository;
            }
        }
        #endregion

        //Custom Repositories getters 
        #region   
        public IGlobalRepository globalRepo
        {
            get
            {
                if (GlobalRepository == null)
                    GlobalRepository = new GlobalRepository(_context);
                return GlobalRepository;
            }
        }
        public IIdentityRepository IdentityRepo
        {
            get
            {
                if (IdentityRepository == null)
                    IdentityRepository = new IdentityRepository(_context);
                return IdentityRepository;
            }
        }
          
        #endregion

        public UnitofWork(BaseProjectDBContext _context)
        {
            this._context = _context;
        }
        public async Task<Tuple<bool, string>> Save(Guid UserId)
        {
            var oppDate = DateTime.Now;
            try
            {
                var changes = _context.ChangeTracker.Entries().ToList();
                var newlyAdded = changes.Where(x => x.State == EntityState.Added).ToList();
                var newleyUpdated = changes.Where(x => x.State == EntityState.Modified).ToList();
                var newleyDeleted = changes.Where(x => x.State == EntityState.Deleted).ToList();
                await _context.SaveChangesAsync();

                if (newleyUpdated != null && newleyUpdated.Count > 0)
                {
                    foreach (var entry in newleyUpdated)
                    {
                        string entityName = entry.Entity.GetType().Name;
                        var primaryKey = entry.Entity.GetType().GetProperty("Id");
                        if (primaryKey != null)
                        {
                            string primaryKeyValue = primaryKey.GetValue(entry.Entity).ToString();
                            var isDeleted = entry.Entity.GetType().GetProperty("Deleted");
                            if (isDeleted != null)
                            {
                                bool.TryParse(isDeleted.GetValue(entry.Entity).ToString(), out bool isDeletedValue);
                                if (isDeletedValue == true)
                                    _context.UserLogs.Add(new UserLog() { Date = oppDate, Description = entityName + "(Id=" + primaryKeyValue + ") is deleted", RecordId = DbTypeConvertor.ToNullableInt(primaryKeyValue), TableName = entityName, Type = "Delete", UserId = UserId.ToString() });
                                else
                                    _context.UserLogs.Add(new UserLog() { Date = oppDate, Description = entityName + "(Id=" + primaryKeyValue + ") is updated", RecordId = DbTypeConvertor.ToNullableInt(primaryKeyValue), TableName = entityName, Type = "Update", UserId = UserId.ToString() });
                            }
                        }

                    }
                }

                if (newleyDeleted != null && newleyDeleted.Count > 0)
                {
                    foreach (var entry in newleyDeleted)
                    {
                        string entityName = entry.Entity.GetType().Name;
                        var primaryKey = entry.Entity.GetType().GetProperty("Id");
                        if (primaryKey != null)
                        {
                            string primaryKeyValue = primaryKey.GetValue(entry.Entity).ToString();
                            _context.UserLogs.Add(new UserLog() { Date = oppDate, Description = entityName + "(Id=" + primaryKeyValue + ") is deleted", RecordId = DbTypeConvertor.ToNullableInt(primaryKeyValue), TableName = entityName, Type = "Delete", UserId = UserId.ToString() });
                        }
                    }
                }

                if (newlyAdded != null && newlyAdded.Count > 0)
                {
                    foreach (var entry in changes)
                    {
                        string entityName = entry.Entity.GetType().Name;
                        var primaryKey = entry.Entity.GetType().GetProperty("Id");
                        if (primaryKey != null)
                        {
                            string primaryKeyValue = primaryKey.GetValue(entry.Entity).ToString();
                            _context.UserLogs.Add(new UserLog() { Date = oppDate, Description = entityName + "(Id=" + primaryKeyValue + ") is added", RecordId = DbTypeConvertor.ToNullableInt(primaryKeyValue), TableName = entityName, Type = "Add", UserId = UserId.ToString() });
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return new Tuple<bool, string>(true, "");
            }
            catch (System.Exception ex)
            {
                var error = ex.Message + " " + ex.InnerException;

                _context.Dbexceptions.Add(new Dbexception() { Details = error, Subject = ex.Message, ExceptionDate = oppDate });
                _context.SaveChanges();
                return new Tuple<bool, string>(false, error);
            }
        }

        public async Task<Tuple<bool, string>> Save()
        {
            var oppDate = DateTime.Now;
            try
            {
                await _context.SaveChangesAsync();
                return new Tuple<bool, string>(true, "");
            }
            catch (System.Exception ex)
            {
                var error = ex.Message + " " + ex.InnerException;
                _context.Dbexceptions.Add(new DbModels.Dbexception() { Details = error, Subject = ex.Message, ExceptionDate = oppDate });
                _context.SaveChanges();
                return new Tuple<bool, string>(false, error);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
