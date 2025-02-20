using AlumniManagement.Frontend.AlumniService;
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
    public class AlumniModel
    {
        [Key]
        public int AlumniID { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [StringLength(50, ErrorMessage = "Character is over limit")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Character is over limit")]
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [StringLength(50, ErrorMessage = "Character is over limit")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [StringLength(50, ErrorMessage = "Character is over limit")]
        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(15, ErrorMessage = "Character is over limit")]
        [DisplayName("Mobile Number")]
        [Required(ErrorMessage = "Mobile Number is required")]
        public string MobileNumber { get; set; }

        [StringLength(255, ErrorMessage = "Character is over limit" )]
        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }

        [DisplayName("District")]
        public System.Nullable<int> DistrictID { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> DateOfBirth { get; set; } = DateTime.Now;

        [DisplayName("Graduation Year")]
        [Range(1960,2025)]
        [Required(ErrorMessage = "Graduation Year is required")]
        public System.Nullable<int> GraduationYear { get; set; }

        [StringLength(100, ErrorMessage = "Character is over limit")]
        [Required(ErrorMessage = "Degree is required")]
        public string Degree { get; set; }

        [DisplayName("Major")]
        [Required(ErrorMessage = "Major is required")]
        public System.Nullable<int> MajorID { get; set; }

        [DisplayName("LinkedIn Profile")]
        [StringLength(255, ErrorMessage = "Character is over limit")]
        public string LinkedInProfile { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss tt}")]
        [DisplayName("Last Update")]
        public System.DateTime ModifiedDate { get; set; }

        // tambahan
        [DisplayName("Address")]
        public string FullAddress { get; set; }


        [DisplayName("Name")]
        public string FullName { get; set; }

        [DisplayName("Faculty")]
        public string FacultyName { get; set; }

        [DisplayName("Major")]
        public string MajorName { get; set; }

        [DisplayName("Faculty")]
        public int FacultyID { get; set; }

        [DisplayName("State")]
        public int StateID { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }


        public IEnumerable<SelectListItem> DistrictDDL { get; set; }
        
        public IEnumerable<SelectListItem> MajorDDl { get; set; }


        public string HobbiesListName { get; set; }

        public IEnumerable<HobbyDTO> Hobbies { get; set; }


        public string ShowDateOfBirth { get; set; }

        public IEnumerable<SelectListItem> HobbiesDDl { get; set; }

        //    [Ignore]
        // Catatan Opsi Lain
        //    public string DateOfBirthDisplay
        //    {
        //        get
        //        {
        //            return (DateOfBirth.Value.ToString("yyyy-MM-dd") != "0001-01-01") ? DateOfBirth.Value.ToString("yyyy-MM-dd") : "N/A";
        //        }
        //    }
    }
}