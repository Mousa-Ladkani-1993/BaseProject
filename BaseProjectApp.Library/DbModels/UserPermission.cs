using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class UserPermission
    {
        public int Id { get; set; }
        public bool? CanView { get; set; }
        public bool? CanAdd { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }
        public string? UserId { get; set; }
        public string? RoleId { get; set; }

        public virtual AspNetRole? Role { get; set; }
        public virtual AspNetUser? User { get; set; }
    }
}
