using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaim>();
            UserPermissions = new HashSet<UserPermission>();
            Users = new HashSet<AspNetUser>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? NormalizedName { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? SectionName { get; set; }
        public bool? ShowForSupplier { get; set; }
        public string? CssClassName { get; set; }
        public string? Code { get; set; }
        public string? ModuleName { get; set; }

        public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }

        public virtual ICollection<AspNetUser> Users { get; set; }
    }
}
