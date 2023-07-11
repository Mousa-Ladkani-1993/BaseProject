using System.Collections.Generic;
using System.Linq;

namespace BaseProjectApp.Admin.Middlewares
{

    public class EndpointPermissionsSet
    {

        public static List<(string path, RolesNames role, ApiActions action, PageType ptype)> endpointPermissionsSet { get; private set; }

        static EndpointPermissionsSet()
        {
            endpointPermissionsSet = new List<(string path, RolesNames role, ApiActions action, PageType ptype)>();
        }

        public static void Add(string path, RolesNames role, ApiActions action, PageType ptype)
        {

            var isExist = endpointPermissionsSet.Any(x => 
                path.Trim().ToLower() == x.path &&
                role == x.role &&
                action == x.action &&
                ptype == x.ptype
            );

            if(isExist)
            {
                return;
            }

            endpointPermissionsSet.Add((
                path.Trim().ToLower(), role, action, ptype
            ));

        }


        public static (string path, RolesNames role, ApiActions action, PageType ptype) GetByPath(string path)
        {
            return endpointPermissionsSet.Find(x => x.path == path.Trim().ToLower());
        }

        public static List<(string path, RolesNames role, ApiActions action, PageType ptype)> GetAllByPath(string path)
        {
            return endpointPermissionsSet.Where(x => x.path == path.Trim().ToLower()).ToList();
        }


    }

}