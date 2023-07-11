using System;
using System.Collections.Generic;
using System.Text;

namespace BaseProjectApp.Library.Templates.Responses
{
    public class UserResponse
    {
        public string Id
        {
            get;
            set;
        }
        public string? Email
        {
            get;
            set;
        }
        public string? UserName
        {
            get;
            set;
        }

        public bool PortalUser
        {
            get;
            set;
        }
        public DateTimeOffset? LockoutEnd
        {
            get;
            set;
        }
    }



    public class PortalUserResponse
    {
        public int Id
        {
            get;
            set;
        }

        public string? UserId
        {
            get;
            set;
        }
        public string? Email
        {
            get;
            set;
        }
        public bool? Verified 
        {
            get;
            set;
        }
        public string? Name
        {
            get;
            set;
        }
        public string? Country
        {
            get;
            set;
        } 


        public string? UserName
        {
            get;
            set;
        }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
