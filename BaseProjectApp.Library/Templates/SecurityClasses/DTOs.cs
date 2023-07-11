using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.SecurityClasses
{
    public class LogInDto
    {
        public string? AccountId { get; set; }
        public string? EncryptedPassword { get; set; }


    }

    public class SocialMedia_LogInDto
    {
        public string? GoogleId { get; set; }
        public string? FacebookId { get; set; }
        public string? AppleId { get; set; }

        public string? Email { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? CountryCode { get; set; }
        public string? PreferredLanguage { get; set; }
    }


    public class RegisterDto
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string? FullName { get; set; }
         
         
        [EmailAddress]
        public string? Email { get; set; }

        public string? Password { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        public string  Phone { get; set; }

        [Required(ErrorMessage = "Country Code is required.")]
        public string  CountryCode { get; set; }

        [Required(ErrorMessage = "Preferred Language is required.")]
        public string? PreferredLanguage { get; set; } = "en";
         
        public bool? WhatsappSamePhone { get; set; } 
        public string? WhatsappPhone { get; set; } 
        public string? WhatsappCountryCode { get; set; }

    }



    public class AccountResponse
    {
        public bool IsEmail { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
    }



    public class UserData
    {
        public bool Verified { get; set; }
        public string? FullName { get; set; } 
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? CountryCode { get; set; }
       
        public string? PreferredLanguage { get; set; }

        public int? AreaUnitId { get; set; } 
        public int? CurrencyId { get; set; }

        public int? CountryId { get; set; } 
        public string? FacebookId { get; set; }
        public string? GoogleId { get; set; }
        public string? AppleId { get; set; }

        public bool? Recommendations { get; set; }
        public bool? SpecialCommunications { get; set; }
    }

    public class ForgerPasswordDto
    {
        //[Required(ErrorMessage ="email is required")]
        //[EmailAddress]
        public string? email { get; set; }

        //[Required(ErrorMessage = "new password is required")]
        //[MinLength(6)]
        public string? newPassword { get; set; }
    }

    public class ForgerPasswordV2Dto
    {
        //[Required(ErrorMessage = "email is required")]
        //[EmailAddress]
        public string? AccountId { get; set; }

        //[Required(ErrorMessage = "new password is required")]
        //[MinLength(6)]
        public string? newPassword { get; set; }
        public string? Code { get; set; }
    }


    public class ChangePasswordDto
    {
        //[Required(ErrorMessage = "email is required")]
        //[EmailAddress]
        public string? AccountId { get; set; }

        //[Required(ErrorMessage = "old password is required")]
        //[MinLength(6)]
        public string? oldPassword { get; set; }

        //[Required(ErrorMessage = "new password is required")]
        //[MinLength(6)]
        public string? newPassword { get; set; }
    }


    public class ChangeMyPasswordDto
    {

        public string? OldPassword { get; set; }

        public string? NewPassword { get; set; }
    }


    public class AddClientFetuseDto
    {
        public int? Id { get; set; }
        public string? Gender { get; set; }
    }
}
