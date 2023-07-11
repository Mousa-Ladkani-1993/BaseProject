using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.Responses
{
   public class RoleResponse
    { 
        public string name { get; set; }
        public string? Code { get; set; } 
        public string? Section { get; set; } 
        public string? IconClass { get; set; }

    }

    public class RoleRes
    { 
        public string Id { get; set; }
        public string? Name { get; set; }  
    }
}
