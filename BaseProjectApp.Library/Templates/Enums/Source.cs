using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Templates.Enums
{
    public enum AdType_ENUM : int
    {
        Featured = 1,
        Premium = 2,
        Regular = 3,
    }
    public enum CustommenulocationEnum : int
    {
        Top = 1,
        Bottom = 2
    }
    public enum SocialAccount_Enum
    { 
        Google = 1,
        Facebook = 2,
        Apple = 3, 
    } 

    public enum SystemParameterType_Enum
    {
        Text = 1,
        Number = 2,
        Date = 3,
        Boolean
    }
     
    public enum SendVerificationCodeType_Enum
    {
        SMS = 1,
        Email = 2
    }

     

    public enum MediaType
    {
        Image = 1,
        Video = 2,
        File = 3
    }
     


}
