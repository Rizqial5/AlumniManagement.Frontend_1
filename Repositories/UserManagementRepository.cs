using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.UserManagementService;
using AutoMapper.QueryableExtensions.Impl;
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

            _userManagementServices.DeleteUser(id);

        }

        public IEnumerable<AspNetUserModel.UserModel> GetAllUsers()
        {
            var data = Mapping.Mapper.Map<List<AspNetUserModel.UserModel>>(_userManagementServices.GetAllUsers());

            return data;
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

        public IEnumerable<AspNetUserModel.RoleModel> GetAllRoles()
        {
            var data = _userManagementServices.GetAllRoles();

            var result = Mapping.Mapper.Map<List<AspNetUserModel.RoleModel>>(data);

            return result;
        }

        public void UpdateUserRoles(string id, List<string> rolesAdded)
        {
            foreach (var item in rolesAdded)
            {
                _userManagementServices.UpdateUserRoles(id, item);
            }
        }

        public void InsertRoles(AspNetUserModel.RoleModel roleDTO)
        {
            var mapped = Mapping.Mapper.Map<AspNetUserDTORoleDTO>(roleDTO);

            _userManagementServices.InsertRoles(mapped);
        }

        public void DeleteRoles(string id)
        {
            _userManagementServices.DeleteRoles(id);
        }

        public AspNetUserModel.RoleModel GetRoleById(string id)
        {
            var data = _userManagementServices.GetRoleById(id);

            var mapped = Mapping.Mapper.Map<AspNetUserModel.RoleModel>(data);

            return mapped;
        }
    }
}