using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProjectApp.API.Authorization
{
    internal class PermissionsRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public PermissionsRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
