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
            var data = _jobServiceClient.GetAll(alumniId);
            var result = data.Select(f => Mapping.Mapper.Map<JobModel>(f));




            return result.ToList();
        }



        public JobModel GetJob(int jobId, int alumniId)
        {
            var selectedData = _jobServiceClient.GetJob(jobId, alumniId);

            return Mapping.Mapper.Map<JobModel>(selectedData);
        }

        public void InsertJob(JobModel JobModel, int alumniId)
        {
            var result = Mapping.Mapper.Map<JobDTO>(JobModel);

            _jobServiceClient.InsertJob(result, alumniId);
        }

        public void UpdateJob(JobModel JobModel, int alumniId)
        {
            var result = Mapping.Mapper.Map<JobDTO>(JobModel);

            _jobServiceClient.UpdateJob(result, alumniId);
        }

        public void DeleteJob(int jobId, int alumniId)
        {
            _jobServiceClient.DeleteJob(jobId, alumniId);
        }
    }
}