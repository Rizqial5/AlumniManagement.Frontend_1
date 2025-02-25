using AlumniManagement.Frontend.PostingJobService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace AlumniManagement.Frontend.Models
{
    public class ShowCandidateModel
    {
        [Key]
        public int AlumniID { get; set; }
        public Guid JobID { get; set; }
        public DateTime ApplyDate { get; set; }

        public string FullName { get; set; }

        public List<JobAttachmentDTO> JobAttachments { get; set; }
        
        public List<ShowUrlModel> ListUrls { get; set; }
    }

    public class ShowUrlModel
    {
        public string Urls { get; set; }
        public string NameType { get; set; }
    }
}