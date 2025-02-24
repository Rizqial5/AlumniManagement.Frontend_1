using AlumniManagement.Frontend.PostingJobService;
using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlumniManagement.Frontend.Models
{
    public class JobPostingModel
    {
        [Key]
        public Guid JobID { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        [StringLength(50, ErrorMessage = "Max char is 50")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Company is Required")]
        [StringLength(50, ErrorMessage = "Max char is 50")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        [DisplayName("Description")]
        public string JobDescription { get; set; }

        [Required(ErrorMessage = "Employment type is Required")]
        [DisplayName("Employment Type")]
        public byte EmploymentTypeID { get; set; }

        [Required(ErrorMessage = "Minimum Experience is Required")]
        [DisplayName("Minimum Experience")]
        public byte MinimumExperience { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss tt}")]
        [DisplayName("Last Update")]
        public DateTime ModifiedDate { get; set; }

        [Required(ErrorMessage = "Active is Required")]
        [DisplayName("Status")]
        public bool IsActive { get; set; }

        [DisplayName("Close Job Posting")]
        public bool IsClosed { get; set; }

        [DisplayName("Total Candidates")]
        public int TotalCandidates { get; set; }



        [DisplayName("Attachment")]
        [Required(ErrorMessage = "Attachment type required")]
        [Range(0,127)]
        public List<int> SelectedAttachmentTypes { get; set; }

        [DisplayName("Skill")]
        [Required(ErrorMessage = "Skill is required")]
        [Range(0, 127)]
        public List<int> SelectedSkills { get; set; }


        //tambahan
        [Ignore]
        public IEnumerable<SelectListItem> EmployemenTypeDDL { get; set; }
        [Ignore]
        public IEnumerable<SelectListItem> SkillDDL {  get; set; }
        [Ignore]
        public IEnumerable<AttachmentTypeModel> AttachMentCheckBox { get; set; }

        
        public string ActiveDetails { get; set;}

        
        public string ClosedDetails { get; set; }   


    }
}