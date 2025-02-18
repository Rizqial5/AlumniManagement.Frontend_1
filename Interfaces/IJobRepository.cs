using AlumniManagement.Frontend.JobHistoryService;
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlumniManagement.Frontend.Interfaces
{
    public interface IJobRepository
    {
        IEnumerable<JobModel> GetAll(int alumniId);

        JobModel GetJob(int jobId, int alumniId);
    
        void InsertJob(JobModel JobModel, int alumniId);

        void UpdateJob(JobModel JobModel, int alumniId);

        void DeleteJob(int jobId, int alumniId);
    }
}
