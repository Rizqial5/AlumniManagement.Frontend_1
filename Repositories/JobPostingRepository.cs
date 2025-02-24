using AlumniManagement.Frontend.FacultyService;
using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.PostingJobService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Repositories
{
    public class JobPostingRepository : IJobPostingRepository
    {
        private PostingJobServiceClient _service;

        public JobPostingRepository()
        {
            _service = new PostingJobServiceClient();
        }

        public IEnumerable<AttachmentTypeModel> GetAttachmentTypes()
        {
            var data = _service.GetAttachmentTypes();
            var result = data.Select(f => Mapping.Mapper.Map<AttachmentTypeModel>(f));

            return result;
        }

        public IEnumerable<EmploymentTypeModel> GetEmploymentTypes()
        {
            var data = _service.GetEmploymentTypes();
            var result = data.Select(f => Mapping.Mapper.Map<EmploymentTypeModel>(f));

            return result;
        }

        public JobPostingModel GetJobPosting(Guid jobId)
        {
            var selectedData = _service.GetJobPosting(jobId);

            return Mapping.Mapper.Map<JobPostingModel>(selectedData);
        }

        public IEnumerable<JobPostingModel> GetJobPostings()
        {
            var data = _service.GetJobPostings();
            var result = data.Select(f => Mapping.Mapper.Map<JobPostingModel>(f));



            return result;
        }

       

        public IEnumerable<SkillModel> GetSkills()
        {
            var data = _service.GetSkills();
            var result = data.Select(f => Mapping.Mapper.Map<SkillModel>(f));

            return result;
        }

        public void InsertJobPosting(JobPostingModel JobPostingModel)
        {
            //var result = Mapping.Mapper.Map<JobPostingDTO>(JobPostingModel);

            // Mapping to JobPostingDTO
            var result = new JobPostingDTO
            {
                JobID = Guid.NewGuid(),
                Title = JobPostingModel.Title,
                Company = JobPostingModel.Company,
                JobDescription = JobPostingModel.JobDescription,
                EmploymentTypeID = Convert.ToByte(JobPostingModel.EmploymentTypeID),
                MinimumExperience = Convert.ToByte(JobPostingModel.MinimumExperience),
                ModifiedDate = DateTime.Now,
                IsActive = JobPostingModel.IsActive,
                IsClosed = JobPostingModel.IsClosed,
                SelectedAttachmentTypes = JobPostingModel.SelectedAttachmentTypes.ToArray(),
                SelectedSkills = JobPostingModel.SelectedSkills.ToArray()
            };

            //result.JobID = Guid.NewGuid();


            _service.InsertJobPosting(result);
        }

        public void UpdateJobPosting(JobPostingModel JobPostingModel)
        {
            var result = Mapping.Mapper.Map<JobPostingDTO>(JobPostingModel);

            _service.UpdateJobPosting(result);
        }

        public void DeletingJobPosting(Guid jobId)
        {
            _service.DeletingJobPosting(jobId);
        }

        public void InsertApplyJob(JobAttachmentModel jobAttachmentModel)
        {
            var result = Mapping.Mapper.Map<JobAttachmentDTO>(jobAttachmentModel);

            _service.InsertApplyJob(result);
        }

        public IEnumerable<ShowCandidateModel> GetAllCandidateBYJObId(Guid jobID)
        {
            var data = _service.GetAllCandidateBYJObId(jobID);

            var result = Mapping.Mapper.Map<List<ShowCandidateModel>>(data);

            return result;
        }

        public IEnumerable<SkillModel> GetSkillsbyId(Guid jobId)
        {
            var data = _service.GetSkillsByJobId(jobId);

            var result = Mapping.Mapper.Map<List<SkillModel>>(data);

            return result;
        }

        public void UpsertJobPosting(JobPostingModel jobPostingModel)
        {
           var result = Mapping.Mapper.Map<JobPostingDTO>(jobPostingModel);

            _service.UpsertJobPosting(result);
        }
    }
}