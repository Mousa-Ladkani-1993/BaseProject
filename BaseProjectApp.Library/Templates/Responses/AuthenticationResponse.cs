using BaseProjectApp.Library.Templates.SecurityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.Responses
{
    public class TokenRequest
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; } 
        public string? UserId { get; set; } 
    }


}
