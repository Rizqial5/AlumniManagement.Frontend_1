using AlumniManagement.Frontend.FacultyService;
using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.JobHistoryService;
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Repositories
{
    public class JobRepository : IJobRepository
    {
        private JobHistorySericeClient _jobServiceClient;

        public JobRepository()
        {
            _jobServiceClient = new JobHistorySericeClient();
        }

        public IEnumerable<JobModel> GetAll(int alumniId)
        {
            try
            {
                var data = _jobServiceClient.GetAll(alumniId);
                return data.Select(f => Mapping.Mapper.Map<JobModel>(f)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving data", ex);
            }
        }

        public JobModel GetJob(int jobId, int alumniId)
        {
            try
            {
                var selectedData = _jobServiceClient.GetJob(jobId, alumniId);
                return Mapping.Mapper.Map<JobModel>(selectedData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving data", ex);
            }
        }

        public void InsertJob(JobModel jobModel, int alumniId)
        {
            try
            {
                var result = Mapping.Mapper.Map<JobDTO>(jobModel);
                _jobServiceClient.InsertJob(result, alumniId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting data", ex);
            }
        }

        public void UpdateJob(JobModel jobModel, int alumniId)
        {
            try
            {
                var result = Mapping.Mapper.Map<JobDTO>(jobModel);
                _jobServiceClient.UpdateJob(result, alumniId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating data", ex);
            }
        }

        public void DeleteJob(int jobId, int alumniId)
        {
            try
            {
                _jobServiceClient.DeleteJob(jobId, alumniId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting data", ex);
            }
        }
    }

}