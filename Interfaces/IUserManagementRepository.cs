
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlumniManagement.Frontend.Interfaces
{
    public interface IUserManagementRepository
    {
        
        bool IsSuperadminExists();
        
        void RegisterUser(string fullName, string email, string password, bool superadmin);
        
        void UpdatePassword(string id, string password);
        
        void UpdateUserFullName(string email, string fullName);
        
        void DeleteUser(string id);
        
        AspNetUserModel.UserModel GetUser(string id);
        
        IEnumerable<AspNetUserModel.UserModel> GetAllUsers();
        
        IEnumerable<AspNetUserModel.UserModel> GetUsersByRole(string roleName);

        IEnumerable<AspNetUserModel.RoleModel> GetAllRoles();

        void UpdateUserRoles(string id, List<string> rolesAdded);

        void AssignSuperadmin(string id);

        void InsertRoles(AspNetUserModel.RoleModel roleDTO);
        void DeleteRoles(string id);

        AspNetUserModel.RoleModel GetRoleById(string id);
    }
}
