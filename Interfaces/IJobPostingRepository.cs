using AlumniManagement.Frontend.PostingJobService;
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlumniManagement.Frontend.Interfaces
{
    public interface IJobPostingRepository
    {
        
        IEnumerable<JobPostingModel> GetJobPostings();

        
        IEnumerable<EmploymentTypeModel> GetEmploymentTypes();

        
        IEnumerable<SkillModel> GetSkills();
        IEnumerable<SkillModel> GetSkillsbyId(Guid jobId);


        IEnumerable<AttachmentTypeModel> GetAttachmentTypes();

        
        JobPostingModel GetJobPosting(Guid jobId);

        
        void InsertJobPosting(JobPostingModel JobPostingModel);

        
        void UpdateJobPosting(JobPostingModel JobPostingModel);

        
        void DeletingJobPosting(Guid jobId);

        void InsertApplyJob(JobAttachmentModel jobAttachmentModel);

        IEnumerable<ShowCandidateModel> GetAllCandidateBYJObId(Guid jobID);

        void UpsertJobPosting(JobPostingModel jobPostingModel);

    }
}
