using Microsoft.AspNetCore.Identity;

namespace BaseProjectApp.Admin.Areas.Identity.Model
{
    public class ApplicationRole : IdentityRole
    {
        public string? SectionName { get; set; } 
        public string? CssClassName { get; set; } 
        public string? Code { get; set; }
        public string? ModuleName { get; set; } 

        //public int? RoleType { get; set; }

    }
}
