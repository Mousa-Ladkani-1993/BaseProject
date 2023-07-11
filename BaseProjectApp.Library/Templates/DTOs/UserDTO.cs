using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.DTOs
{


    public class UserSetIntoDTO
    { 
        public int CountryId { get; set; } 
        public string? PreferredLanguage { get; set; } = "";
        public int? AreaUnitId { get; set; } = 0;
        public int? CurrencyId { get; set; } = 0;
        public bool? Recommendations { get; set; } = false;
        public bool? SpecialCommunications { get; set; } = false;
    }

    public class UserDTO
    {
        public string? UserId { get; set; }
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
         
        public string? FullNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CountryCode { get; set; }

        public string? WhatsappFullNumber { get; set; } 
        public string? WhatsappNumber { get; set; }
        public string? WhatsappCountryCode { get; set; }
        public string? Address { get; set; }
        public string? ImgUrl { get; set; }
        public int? SocialAccountTypeId { get; set; }
    }
     

    public class UserPutDTO
    {
        public string? FullName { get; set; }
        public string? Address { get; set; } 

        public string? PhoneNumber { get; set; }
        public string? CountryCode { get; set; }
        public string? Email { get; set; }  
        public bool? WhatsappSamePhone { get; set; }
        public string? WhatsappPhone { get; set; }
        public string? WhatsappCountryCode { get; set; }
    }
}
