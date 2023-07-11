using System.Linq;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using BaseProjectApp.Library.Templates.Enums;
 

namespace BaseProjectApp.Admin.Middlewares
{
    
    public class Perm
    {
        public bool canView;
        public bool canAdd;
        public bool canEdit;
        public bool canDelete;
        public PageType ptype;
        
        
        private IUnitofWork _repo;
        private RolesNames _role;
        private string _userId;


        public Perm( RolesNames role, IUnitofWork repo, string userId)
        {
            _role = role;
            _repo = repo;
            _userId = userId;
            exe();
        }

        private void exe()
        {
            var perms = _repo.UserPermissions.GetAllById(up => up.UserId == _userId ); 

            var role = _repo.AspNetRoles.GetAll(r => r.Name.ToLower() == _role.Str().Trim().ToLower()).Result?.FirstOrDefault(); 

            if (role == null)
            {
                return;
            }

            var res = perms.Find(up => up.RoleId == role.Id);

            if (res == null)
            {
                return;
            }
            canView = res.CanView.GetValueOrDefault();
            canAdd = res.CanAdd.GetValueOrDefault();
            canEdit = res.CanEdit.GetValueOrDefault();
            canDelete = res.CanDelete.GetValueOrDefault();
            ptype = PageType.ALL;
        }
    }

}