using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>(); 
            UserLogs = new HashSet<UserLog>();
            UserPermissions = new HashSet<UserPermission>();
            Roles = new HashSet<AspNetRole>();
        }

        public string Id { get; set; } = null!;
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; } = null!;
        public string? NormalizedUserName { get; set; }
        public string? NormalizedEmail { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? ResetPasswordCode { get; set; }
        public string? FullName { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } 
        public virtual ICollection<UserLog> UserLogs { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }

        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
