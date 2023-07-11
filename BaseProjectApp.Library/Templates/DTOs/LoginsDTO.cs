using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.DTOs
{

    public class ExternalAuthDto
    {
        public string? Provider { get; set; }
        public string? IdToken { get; set; }
        public string? PreferredLanguage { get; set; } = "en";
    }

    public class ExternalAppleAuthDto
    { 
        public string IdToken { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        //public string LastName { get; set; } 
        public string? PreferredLanguage { get; set; } = "en";
    }

    public class FacebookDTO
    {
        public string? id { get; set; }
        public string? name { get; set; } 
        public string? full_name { get; set; }
        public string? last_name { get; set; }
    }

    public class FacebookAccount
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Locale { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; } 
        public string Gender { get; set; }
    }

    public class ExternalCLientDTO
    {
        public string? FacebookId { get; set; }
        public string? GoogleId { get; set; }
        public string? AppleId { get; set; } 
        public string? FullName { get; set; }
        public string? LastName { get; set; }
        public string? CountryCode { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }


        public string? UserId { get; set; }


        public string? PreferredLanguage { get; set; } = "en";
        public bool? Active { get; set; } = true;
        public bool? Verified { get; set; } = true;
        public DateTime? RegistrationDate { get; set; } = DateTime.Now;
        public string? Address { get; set; }


        public string? Subject { get; set; }

        public string? Type { get; set; }


    }




}
