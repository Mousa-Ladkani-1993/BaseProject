using System;
using System.Collections.Generic;

namespace BaseProjectApp.Library.DbModels
{
    public partial class AspNetUserLogin
    {
        public string LoginProvider { get; set; } = null!;
        public string ProviderKey { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string? ProviderDisplayName { get; set; }

        public virtual AspNetUser User { get; set; } = null!;
    }
}
