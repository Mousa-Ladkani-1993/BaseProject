using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProjectApp.API.Authorization
{
    public enum Permissions_Enum
    {
        Add = 1,
        Edit = 2,
        View = 3,
        Delete = 4,
    }

    public class PermissionsNum
    {
        public const string Add = "1";
        public string AddProp { get => Add; }

        public const string Edit = "2";
        public string EditProp { get => Edit; }

        public const string View = "3";
        public string ViewProp { get => View; }

        public const string Delete = "4";
        public string DeleteProp { get => Delete; }
    }


    public class RolesCode
    {
        public const string PushNotification = "24";
        public string PushNotificationProp { get => PushNotification; }

        public const string Dashboard = "23";
        public string DashboardProp { get => Dashboard; }

        public const string Packages = "22";
        public string PackagesProp { get => Packages; }

        public const string Currencies = "21";
        public string CurrenciesProp { get => Currencies; }


        public const string Properties = "20";
        public string PropertiesProp { get => Properties; }



        public const string Text_Variables = "01";
        public string Text_VariablesProp { get => Text_Variables; }

        public const string lookups = "02";
        public string lookupsProp { get => lookups; }

        public const string Media_files = "06";
        public string Media_filesProp { get => Media_files; }


        public const string Company_Lookups = "09";
        public string Company_LookupsProp { get => Company_Lookups; }

        public const string Content_Records = "08";
        public string Content_RecordsProp { get => Content_Records; }

        public const string Sliders = "07";
        public string SlidersProp { get => Sliders; }

        public const string Inquiry = "03";
        public string InquiryProp { get => Inquiry; }


        public const string Users = "10";
        public string UsersProp { get => Users; }

        public const string SystemParameters = "13";
        public string SystemParametersProp { get => SystemParameters; }

        public const string FAQS = "12";
        public string FAQSProp { get => FAQS; }



        public const string Countries = "16";
        public string CountriesProp { get => Countries; }


        public const string Keywords = "17";
        public string KeywordsProp { get => Keywords; }

        public const string Roles = "18";
        public string RolesProp { get => Roles; }

        public const string Menus = "19";
        public string MenusProp { get => Menus; }

        public const string About_Us = "04";
        public string About_UsProp { get => About_Us; }



    }


    public class Auth_Permissions
    { 
        public class Properties
        {
            public const string CanAddProperties = PermissionsNum.Add + RolesCode.Properties;
            public string CanAddPropertiesProp { get => CanAddProperties; }

            public const string CanEditProperties = PermissionsNum.Edit + RolesCode.Properties;
            public string CanEditPropertiesProp { get => CanEditProperties; }

            public const string CanViewProperties = PermissionsNum.View + RolesCode.Properties;
            public string CanViewPropertiesProp { get => CanViewProperties; }

            public const string CanDeleteProperties = PermissionsNum.Delete + RolesCode.Properties;
            public string CanDeletePropertiesProp { get => CanDeleteProperties; }
        }
         
        public class Currencies
        {
            public const string CanAddCurrencies = PermissionsNum.Add + RolesCode.Currencies;
            public string CanAddCurrenciesProp { get => CanAddCurrencies; }

            public const string CanEditCurrencies = PermissionsNum.Edit + RolesCode.Currencies;
            public string CanEditCurrenciesProp { get => CanEditCurrencies; }

            public const string CanViewCurrencies = PermissionsNum.View + RolesCode.Currencies;
            public string CanViewCurrenciesProp { get => CanViewCurrencies; }

            public const string CanDeleteCurrencies = PermissionsNum.Delete + RolesCode.Currencies;
            public string CanDeleteCurrenciesProp { get => CanDeleteCurrencies; }
        } 

        public class Dashboard
        {
            public const string CanAddDashboard = PermissionsNum.Add + RolesCode.Dashboard;
            public string CanAddDashboardProp { get => CanAddDashboard; }

            public const string CanEditDashboard = PermissionsNum.Edit + RolesCode.Dashboard;
            public string CanEditDashboardProp { get => CanEditDashboard; }

            public const string CanViewDashboard = PermissionsNum.View + RolesCode.Dashboard;
            public string CanViewDashboardProp { get => CanViewDashboard; }

            public const string CanDeleteDashboard = PermissionsNum.Delete + RolesCode.Dashboard;
            public string CanDeleteDashboardProp { get => CanDeleteDashboard; }
        }
        public class Packages
        {
            public const string CanAddPackages = PermissionsNum.Add + RolesCode.Packages;
            public string CanAddPackagesProp { get => CanAddPackages; }

            public const string CanEditPackages = PermissionsNum.Edit + RolesCode.Packages;
            public string CanEditPackagesProp { get => CanEditPackages; }

            public const string CanViewPackages = PermissionsNum.View + RolesCode.Packages;
            public string CanViewPackagesProp { get => CanViewPackages; }

            public const string CanDeletePackages = PermissionsNum.Delete + RolesCode.Packages;
            public string CanDeletePackagesProp { get => CanDeletePackages; }
        }


        public class Text_Variables
        {
            public const string CanAddTextVariables = PermissionsNum.Add + RolesCode.Text_Variables;
            public string CanAddTextVariablesProp { get => CanAddTextVariables; }

            public const string CanEditTextVariables = PermissionsNum.Edit + RolesCode.Text_Variables;
            public string CanEditTextVariablesProp { get => CanEditTextVariables; }

            public const string CanViewTextVariables = PermissionsNum.View + RolesCode.Text_Variables;
            public string CanViewTextVariablesProp { get => CanViewTextVariables; }

            public const string CanDeleteTextVariables = PermissionsNum.Delete + RolesCode.Text_Variables;
            public string CanDeleteTextVariablesProp { get => CanDeleteTextVariables; }
        }

        public class lookups
        {
            public const string CanAddlookups = PermissionsNum.Add + RolesCode.lookups;
            public string CanAddlookupsProp { get => CanAddlookups; }

            public const string CanEditlookups = PermissionsNum.Edit + RolesCode.lookups;
            public string CanEditlookupsProp { get => CanEditlookups; }

            public const string CanViewlookups = PermissionsNum.View + RolesCode.lookups;
            public string CanViewlookupsProp { get => CanViewlookups; }

            public const string CanDeletelookups = PermissionsNum.Delete + RolesCode.lookups;
            public string CanDeletelookupsProp { get => CanDeletelookups; }
        }

        public class Media_files
        {
            public const string CanAddMediaFiles = PermissionsNum.Add + RolesCode.Media_files;
            public string CanAddMediaFilesProp { get => CanAddMediaFiles; }

            public const string CanEditMediaFiles = PermissionsNum.Edit + RolesCode.Media_files;
            public string CanEditMediaFilesProp { get => CanEditMediaFiles; }

            public const string CanViewMediaFiles = PermissionsNum.View + RolesCode.Media_files;
            public string CanViewMediaFilesProp { get => CanViewMediaFiles; }

            public const string CanDeleteMediaFiles = PermissionsNum.Delete + RolesCode.Media_files;
            public string CanDeleteMediaFilesProp { get => CanDeleteMediaFiles; }
        }

        public class Company_Lookups
        {
            public const string CanAddCompanyLookups = PermissionsNum.Add + RolesCode.Company_Lookups;
            public string CanAddCompanyLookupsProp { get => CanAddCompanyLookups; }

            public const string CanEditCompanyLookups = PermissionsNum.Edit + RolesCode.Company_Lookups;
            public string CanEditCompanyLookupsProp { get => CanEditCompanyLookups; }

            public const string CanViewCompanyLookups = PermissionsNum.View + RolesCode.Company_Lookups;
            public string CanViewCompanyLookupsProp { get => CanViewCompanyLookups; }

            public const string CanDeleteCompanyLookups = PermissionsNum.Delete + RolesCode.Company_Lookups;
            public string CanDeleteCompanyLookupsProp { get => CanDeleteCompanyLookups; }
        }


        public class Content_Records
        {
            public const string CanAddContentRecords = PermissionsNum.Add + RolesCode.Content_Records;
            public string CanAddContentRecordsProp { get => CanAddContentRecords; }

            public const string CanEditContentRecords = PermissionsNum.Edit + RolesCode.Content_Records;
            public string CanEditContentRecordsProp { get => CanEditContentRecords; }

            public const string CanViewContentRecords = PermissionsNum.View + RolesCode.Content_Records;
            public string CanViewContentRecordsProp { get => CanViewContentRecords; }

            public const string CanDeleteContentRecords = PermissionsNum.Delete + RolesCode.Content_Records;
            public string CanDeleteContentRecordsProp { get => CanDeleteContentRecords; }
        }

        public class TeamMembers
        {
            public const string CanAddTeamMembers = PermissionsNum.Add + "05";
            public string CanAddTeamMembersProp { get => CanAddTeamMembers; }

            public const string CanEditTeamMembers = PermissionsNum.Edit + "05";
            public string CanEditTeamMembersProp { get => CanEditTeamMembers; }

            public const string CanViewTeamMembers = PermissionsNum.View + "05";
            public string CanViewTeamMembersProp { get => CanViewTeamMembers; }

            public const string CanDeleteTeamMembers = PermissionsNum.Delete + "05";
            public string CanDeleteTeamMembersProp { get => CanDeleteTeamMembers; }
        }

        public class Sliders
        {
            public const string CanAddSliders = PermissionsNum.Add + RolesCode.Sliders;
            public string CanAddSlidersProp { get => CanAddSliders; }

            public const string CanEditSliders = PermissionsNum.Edit + RolesCode.Sliders;
            public string CanEditSlidersProp { get => CanEditSliders; }

            public const string CanViewSliders = PermissionsNum.View + RolesCode.Sliders;
            public string CanViewSlidersProp { get => CanViewSliders; }

            public const string CanDeleteSliders = PermissionsNum.Delete + RolesCode.Sliders;
            public string CanDeleteSlidersProp { get => CanDeleteSliders; }
        }

        public class Inquiry
        {
            public const string CanAddInquiry = PermissionsNum.Add + RolesCode.Inquiry;
            public string CanAddInquiryProp { get => CanAddInquiry; }

            public const string CanEditInquiry = PermissionsNum.Edit + RolesCode.Inquiry;
            public string CanEditInquiryProp { get => CanEditInquiry; }

            public const string CanViewInquiry = PermissionsNum.View + RolesCode.Inquiry;
            public string CanViewInquiryProp { get => CanViewInquiry; }

            public const string CanDeleteInquiry = PermissionsNum.Delete + RolesCode.Inquiry;
            public string CanDeleteInquiryProp { get => CanDeleteInquiry; }
        }

        public class Users
        {
            public const string CanAddUsers = PermissionsNum.Add + RolesCode.Users;
            public string CanAddUsersProp { get => CanAddUsers; }

            public const string CanEditUsers = PermissionsNum.Edit + RolesCode.Users;
            public string CanEditUsersProp { get => CanEditUsers; }

            public const string CanViewUsers = PermissionsNum.View + RolesCode.Users;
            public string CanViewUsersProp { get => CanViewUsers; }

            public const string CanDeleteUsers = PermissionsNum.Delete + RolesCode.Users;
            public string CanDeleteUsersProp { get => CanDeleteUsers; }
        }

        public class SystemParameters
        {
            public const string CanAddSystemParameters = PermissionsNum.Add + RolesCode.SystemParameters;
            public string CanAddSystemParametersProp { get => CanAddSystemParameters; }

            public const string CanEditSystemParameters = PermissionsNum.Edit + RolesCode.SystemParameters;
            public string CanEditSystemParametersProp { get => CanEditSystemParameters; }

            public const string CanViewSystemParameters = PermissionsNum.View + RolesCode.SystemParameters;
            public string CanViewSystemParametersProp { get => CanViewSystemParameters; }

            public const string CanDeleteSystemParameters = PermissionsNum.Delete + RolesCode.SystemParameters;
            public string CanDeleteSystemParametersProp { get => CanDeleteSystemParameters; }
        }

        public class FAQS
        {
            public const string CanAddFAQS = PermissionsNum.Add + RolesCode.FAQS;
            public string CanAddFAQSProp { get => CanAddFAQS; }

            public const string CanEditFAQS = PermissionsNum.Edit + RolesCode.FAQS;
            public string CanEditFAQSProp { get => CanEditFAQS; }

            public const string CanViewFAQS = PermissionsNum.View + RolesCode.FAQS;
            public string CanViewFAQSProp { get => CanViewFAQS; }

            public const string CanDeleteFAQS = PermissionsNum.Delete + RolesCode.FAQS;
            public string CanDeleteFAQSProp { get => CanDeleteFAQS; }
        }

        public class Countries
        {
            public const string CanAddCountries = PermissionsNum.Add + RolesCode.Countries;
            public string CanAddCountriesProp { get => CanAddCountries; }

            public const string CanEditCountries = PermissionsNum.Edit + RolesCode.Countries;
            public string CanEditCountriesProp { get => CanEditCountries; }

            public const string CanViewCountries = PermissionsNum.View + RolesCode.Countries;
            public string CanViewCountriesProp { get => CanViewCountries; }

            public const string CanDeleteCountries = PermissionsNum.Delete + RolesCode.Countries;
            public string CanDeleteCountriesProp { get => CanDeleteCountries; }
        }


        public class Keywords
        {
            public const string CanAddKeywords = PermissionsNum.Add + RolesCode.Keywords;
            public string CanAddKeywordsProp { get => CanAddKeywords; }

            public const string CanEditKeywords = PermissionsNum.Edit + RolesCode.Keywords;
            public string CanEditKeywordsProp { get => CanEditKeywords; }

            public const string CanViewKeywords = PermissionsNum.View + RolesCode.Keywords;
            public string CanViewKeywordsProp { get => CanViewKeywords; }

            public const string CanDeleteKeywords = PermissionsNum.Delete + RolesCode.Keywords;
            public string CanDeleteKeywordsProp { get => CanDeleteKeywords; }
        }

        public class Roles
        {
            public const string CanAddRoles = PermissionsNum.Add + RolesCode.Roles;
            public string CanAddRolesProp { get => CanAddRoles; }

            public const string CanEditRoles = PermissionsNum.Edit + RolesCode.Roles;
            public string CanEditRolesProp { get => CanEditRoles; }

            public const string CanViewRoles = PermissionsNum.View + RolesCode.Roles;
            public string CanViewRolesProp { get => CanViewRoles; }

            public const string CanDeleteRoles = PermissionsNum.Delete + RolesCode.Roles;
            public string CanDeleteRolesProp { get => CanDeleteRoles; }
        }

        public class Menus
        {
            public const string CanAddMenus = PermissionsNum.Add + RolesCode.Menus;
            public string CanAddMenusandPagesProp { get => CanAddMenus; }

            public const string CanEditMenus = PermissionsNum.Edit + RolesCode.Menus;
            public string CanEditMenusandPagesProp { get => CanEditMenus; }

            public const string CanViewMenus = PermissionsNum.View + RolesCode.Menus;
            public string CanViewMenusandPagesProp { get => CanViewMenus; }

            public const string CanDeleteMenus = PermissionsNum.Delete + RolesCode.Menus;
            public string CanDeleteMenusandPagesProp { get => CanDeleteMenus; }
        }

        public class About_Us
        {
            public const string CanAddAboutUs = PermissionsNum.Add + RolesCode.About_Us;
            public string CanAddAboutUsProp { get => CanAddAboutUs; }

            public const string CanEditAboutUs = PermissionsNum.Edit + RolesCode.About_Us;
            public string CanEditAboutUsProp { get => CanEditAboutUs; }

            public const string CanViewAboutUs = PermissionsNum.View + RolesCode.About_Us;
            public string CanViewAboutUsProp { get => CanViewAboutUs; }

            public const string CanDeleteAboutUs = PermissionsNum.Delete + RolesCode.About_Us;
            public string CanDeleteAboutUsProp { get => CanDeleteAboutUs; }
        }

    }

}