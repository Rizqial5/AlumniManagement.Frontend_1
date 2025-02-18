using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Models
{
    public class FacultyModel
    {
        [Key]
        public int FacultyID { get; set; }

        [Required(ErrorMessage ="Faculty is required")]
        [StringLength(100)]
        [DisplayName("Name")]
        public string FacultyName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss tt}")]
        [DisplayName("Last Update")]
        public System.DateTime ModifiedDate { get; set; }
    }
}