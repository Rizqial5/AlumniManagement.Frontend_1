using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Models
{
    public class AspNetUserModel
    {
        public class RoleModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public ICollection<RolePermissionModel> RolePermissions { get; set; }
        }

        public class PermissionModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public ICollection<RolePermissionModel> RolePermissions { get; set; }
        }

        public class RolePermissionModel
        {
            public string RoleId { get; set; }
            public RoleModel Role { get; set; }
            public int PermissionId { get; set; }
            public PermissionModel Permission { get; set; }
        }

        public class UserModel
        {
            public string Id { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public bool EmailConfirmed { get; set; }
            public string PasswordHash { get; set; }
            public string SecurityStamp { get; set; }
            public string PhoneNumber { get; set; }
            public bool TwoFactorEnabled { get; set; }
            public DateTime? LockoutEndDateUtc { get; set; }
            public bool LockoutEnabled { get; set; }
            public int AccessFailedCount { get; set; }
            public string UserName { get; set; }
            public ICollection<UserRoleModel> UserRoles { get; set; }
            public ICollection<UserClaimModel> UserClaims { get; set; }
            public ICollection<UserLoginModel> UserLogins { get; set; }
        }

        public class UserRoleModel
        {
            public string UserId { get; set; }
            public UserModel User { get; set; }
            public string RoleId { get; set; }
            public RoleModel Role { get; set; }
        }

        public class UserClaimModel
        {
            public int Id { get; set; }
            public string UserId { get; set; }
            public UserModel User { get; set; }
            public string ClaimType { get; set; }
            public string ClaimValue { get; set; }
        }

        public class UserLoginModel
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserId { get; set; }
            public UserModel User { get; set; }
        }
    }
}