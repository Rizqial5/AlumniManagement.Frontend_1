using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlumniManagement.Frontend.Models
{
    public class AspNetUserModel
    {
        public class RoleModel
        {
            [Key]
            public string Id { get; set; }

            [Required(ErrorMessage ="Name is Required")]
            public string Name { get; set; }
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
            [Key]
            public string Id { get; set; }

            [Required]
            [DisplayName("Full Name")]
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

            [Ignore]
            public string ShowRoles { get; set; }

            [Ignore]
            public IEnumerable<string> ListRoles { get; set; }

            [Ignore]
            public IEnumerable<SelectListItem> RolesDDL { get; set; }


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