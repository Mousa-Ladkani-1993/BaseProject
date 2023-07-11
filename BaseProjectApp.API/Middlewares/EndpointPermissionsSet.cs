using System.Collections.Generic;
using System.Linq;

namespace BaseProjectApp.API.Middlewares
{

    public class EndpointPermissionsSet
    {

        public static List<(string path, RolesNames role, ApiActions action)> endpointPermissionsSet { get; private set; }

        static EndpointPermissionsSet()
        {
            endpointPermissionsSet = new List<(string path, RolesNames role, ApiActions action)>();
        }

        public static void Add(string path, RolesNames role, ApiActions action)
        {

            var isExist = endpointPermissionsSet.Any(x => 
                path.Trim().ToLower() == x.path &&
                role == x.role &&
                action == x.action
            );

            if(isExist)
            {
                return;
            }

            endpointPermissionsSet.Add((
                path.Trim().ToLower(), role, action
            ));

        }


        public static (string path, RolesNames role, ApiActions action) GetByPath(string path)
        {
            return endpointPermissionsSet.Find(x => x.path == path.Trim().ToLower());
        }


    }

}