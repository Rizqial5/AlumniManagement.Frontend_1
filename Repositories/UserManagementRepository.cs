using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.UserManagementService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Repositories
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private UserManagementServiceClient _userManagementServices;

        public UserManagementRepository()
        {
            _userManagementServices = new UserManagementServiceClient();
        }

        public void DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AspNetUserModel.UserModel> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public AspNetUserModel.UserModel GetUser(string id)
        {
            var user = Mapping.Mapper.Map<AspNetUserModel.UserModel>(_userManagementServices.GetUser(id));
            return user;
        }

        public IEnumerable<AspNetUserModel.UserModel> GetUsersByRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public bool IsSuperadminExists()
        {
            bool isSuperadminExists = _userManagementServices.IsSuperAdminExists();
            return isSuperadminExists;
        }

        public void RegisterUser(string fullName, string email, string password, bool superadmin)
        {
            throw new NotImplementedException();
        }

        public void AssignSuperadmin(string id)
        {
            var user = _userManagementServices.GetUser(id);
            if (user != null)
            {
                _userManagementServices.AssignSuperadmin(id);
            }
        }

        public void UpdatePassword(string id, string password)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserFullName(string id, string fullName)
        {
            _userManagementServices.UpdateUserFullName(id, fullName);
        }
    }
}