using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Models
{
    public class JobAttachmentModel
    {
        [Key]
        public int AttachmentID { get; set; }

        [Required(ErrorMessage = "Job is Required")]
        public Guid JobID { get; set; }

        [Required(ErrorMessage = "Alumni is Required")]
        [DisplayName("Alumni")]
        public int AlumniID { get; set; }

        [Required(ErrorMessage = "Alumni is Required")]
        public byte AttachmentTypeID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}