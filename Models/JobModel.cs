using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Models
{
    public class JobModel
    {
        [Key]
        public int JobHistoryID { get; set; }


        public System.Nullable<int> AlumniID { get; set; }

        [Required(ErrorMessage ="Job Title is Required")]
        [DisplayName("Job Title")]
        [StringLength(100, ErrorMessage = "Total kata melebihi 100 karakter")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "Company is Required")]
        [StringLength(100, ErrorMessage = "Total kata melebihi 100 karakter")]
        public string Company { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss tt}")]
        [DisplayName("Last Update")]
        public System.DateTime ModifiedDate { get; set; }

        [Ignore]
        public string ShowStartDate { get; set; }
        [Ignore]
        public string ShowEndDate { get; set; }
    }
}