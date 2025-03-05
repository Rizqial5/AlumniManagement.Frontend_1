using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Models
{
    public class EventModel
    {
        [Key]
        public int EventID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage ="Max char length is 100")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(255, ErrorMessage = "Max char length is 100")]
        public string Description { get; set; }

 
        public string EventImagePath { get; set; }


        public string EventImageName { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Start Date is Required")]
        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "End Date is Required")]
        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> EndDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [DisplayName("Mark as Closed")]
        public bool IsClosed { get; set; } = true;


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss tt}")]
        [DisplayName("Last Update")]
        public System.DateTime ModifiedDate { get; set; }

        [DisplayName("Date")]
        public string DateDisplay { get; set; }

        public string Status { get; set; }

        public string ShowImage { get;set; }


        [Required(ErrorMessage = "Image is Required")]
        public HttpPostedFileBase File {  get; set; }
    }
}