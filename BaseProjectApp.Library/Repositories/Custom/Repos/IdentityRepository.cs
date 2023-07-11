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
using BaseProjectApp.Library.Templates;

namespace BaseProjectApp.Library.Repositories.Custom.Repos
{
    public class IdentityRepository : IIdentityRepository
    {
        protected BaseProjectDBContext Context { get; set; }
        public IdentityRepository(BaseProjectDBContext context)
        {
            this.Context = context;
        }
        public List<string>? GetPortalUsers()
        {
            return Context.UserPermissions
                .Where(s => !string.IsNullOrWhiteSpace(s.UserId))
                .Select(s => s.UserId.ToString()).Distinct().ToList();
        }

        public List<ListItemNUser>? PortalUsers()
        {
            return Context.UserPermissions
                .Include(s => s.User)
                .Where(s => !string.IsNullOrWhiteSpace(s.UserId))
                .Select(s => new ListItemNUser { Id = s.UserId, Value = s.User == null ? "" : s.User.FullName }).Distinct().ToList();
        }

        public async Task<List<AspNetRole>> GetFullUserRoles(string? userId = "")
        {
            return await Context.AspNetUsers
                .Include(s => s.Roles)
                .ThenInclude(s => s.AspNetRoleClaims)
                     .Where(u => u.Id == userId)
                     .SelectMany(u => u.Roles)
                     .ToListAsync();

        }

        public async Task<bool> DeleteUserRoles(string? userId = "")
        {
            try
            {
                var res = await Context.Database.ExecuteSqlRawAsync($"delete from AspNetUserRoles where UserId='{userId}'");
                return res > 0;
            }

            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
