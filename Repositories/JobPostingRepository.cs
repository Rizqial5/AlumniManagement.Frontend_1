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
            try
            {
                var data = _service.GetAttachmentTypes();
                return data.Select(f => Mapping.Mapper.Map<AttachmentTypeModel>(f));
            }
            catch (Exception ex)
            {
                // Developer Error: "Error retrieving attachment types: " + ex.Message
                throw new Exception("Failed to retrieve data. Please try again later.");
            }
        }

        public IEnumerable<EmploymentTypeModel> GetEmploymentTypes()
        {
            try
            {
                var data = _service.GetEmploymentTypes();
                return data.Select(f => Mapping.Mapper.Map<EmploymentTypeModel>(f));
            }
            catch (Exception ex)
            {
                // Developer Error: "Error retrieving employment types: " + ex.Message
                throw new Exception("Failed to retrieve data. Please try again later.");
            }
        }

        public JobPostingModel GetJobPosting(Guid jobId)
        {
            try
            {
                var selectedData = _service.GetJobPosting(jobId);
                return Mapping.Mapper.Map<JobPostingModel>(selectedData);
            }
            catch (Exception ex)
            {
                // Developer Error: "Error retrieving job posting with ID " + jobId + ": " + ex.Message
                throw new Exception("Failed to retrieve data. Please try again later.");
            }
        }

        public IEnumerable<JobPostingModel> GetJobPostings()
        {
            try
            {
                var data = _service.GetJobPostings();
                return data.Select(f => Mapping.Mapper.Map<JobPostingModel>(f));
            }
            catch (Exception ex)
            {
                // Developer Error: "Error retrieving job postings: " + ex.Message
                throw new Exception("Failed to retrieve data. Please try again later.");
            }
        }

        public IEnumerable<SkillModel> GetSkills()
        {
            try
            {
                var data = _service.GetSkills();
                return data.Select(f => Mapping.Mapper.Map<SkillModel>(f));
            }
            catch (Exception ex)
            {
                // Developer Error: "Error retrieving skills: " + ex.Message
                throw new Exception("Failed to retrieve data. Please try again later.");
            }
        }

        public void InsertJobPosting(JobPostingModel jobPostingModel)
        {
            try
            {
                var result = new JobPostingDTO
                {
                    JobID = Guid.NewGuid(),
                    Title = jobPostingModel.Title,
                    Company = jobPostingModel.Company,
                    JobDescription = jobPostingModel.JobDescription,
                    EmploymentTypeID = Convert.ToByte(jobPostingModel.EmploymentTypeID),
                    MinimumExperience = Convert.ToByte(jobPostingModel.MinimumExperience),
                    ModifiedDate = DateTime.Now,
                    IsActive = jobPostingModel.IsActive,
                    IsClosed = jobPostingModel.IsClosed,
                    SelectedAttachmentTypes = jobPostingModel.SelectedAttachmentTypes.ToArray(),
                    SelectedSkills = jobPostingModel.SelectedSkills.ToArray()
                };
                _service.InsertJobPosting(result);
            }
            catch (Exception ex)
            {
                // Developer Error: "Error inserting job posting: " + ex.Message
                throw new Exception("Failed to insert data. Please try again later.");
            }
        }

        public void UpdateJobPosting(JobPostingModel jobPostingModel)
        {
            try
            {
                var result = Mapping.Mapper.Map<JobPostingDTO>(jobPostingModel);
                _service.UpdateJobPosting(result);
            }
            catch (Exception ex)
            {
                // Developer Error: "Error updating job posting: " + ex.Message
                throw new Exception("Failed to update data. Please try again later.");
            }
        }

        public void DeletingJobPosting(Guid jobId)
        {
            try
            {
                _service.DeletingJobPosting(jobId);
            }
            catch (Exception ex)
            {
                // Developer Error: "Error deleting job posting with ID " + jobId + ": " + ex.Message
                throw new Exception("Failed to delete data. Please try again later.");
            }
        }

        public void InsertApplyJob(List<JobAttachmentModel> jobAttachmentModel, int alumniId, Guid jobId)
        {
            try
            {
                var result = Mapping.Mapper.Map<List<JobAttachmentDTO>>(jobAttachmentModel).ToArray();
                _service.InsertApplyJob(result, jobId, alumniId);
            }
            catch (Exception ex)
            {
                // Developer Error: "Error inserting application for job ID " + jobId + ": " + ex.Message
                throw new Exception("Failed to apply for job. Please try again later.");
            }
        }

        public IEnumerable<ShowCandidateModel> GetAllCandidateBYJObId(Guid jobID)
        {
            try
            {
                var data = _service.GetAllCandidateBYJObId(jobID);
                return Mapping.Mapper.Map<List<ShowCandidateModel>>(data);
            }
            catch (Exception ex)
            {
                // Developer Error: "Error retrieving candidates for job ID " + jobID + ": " + ex.Message
                throw new Exception("Failed to retrieve candidates. Please try again later.");
            }
        }

        public IEnumerable<SkillModel> GetSkillsbyId(Guid jobId)
        {
            try
            {
                var data = _service.GetSkillsByJobId(jobId);
                return Mapping.Mapper.Map<List<SkillModel>>(data);
            }
            catch (Exception ex)
            {
                // Developer Error: "Error retrieving skills for job ID " + jobId + ": " + ex.Message
                throw new Exception("Failed to retrieve data. Please try again later.");
            }
        }

        public void UpsertJobPosting(JobPostingModel jobPostingModel)
        {
            try
            {
                var result = Mapping.Mapper.Map<JobPostingDTO>(jobPostingModel);
                _service.UpsertJobPosting(result);
            }
            catch (Exception ex)
            {
                // Developer Error: "Error upserting job posting: " + ex.Message
                throw new Exception("Failed to save job posting. Please try again later.");
            }
        }
    }

}