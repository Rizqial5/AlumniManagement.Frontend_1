using AlumniManagement.Frontend;
using AlumniManagement.Frontend.FacultyService;
using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.Web.Repositories
{
    public class FacultyRepository : IFacultyRepository
    {
        private FacultyServiceClient _facultyServiceClient;

        public FacultyRepository()
        {
            _facultyServiceClient = new FacultyServiceClient();
        }

        public IEnumerable<FacultyModel> GetAll()
        {
            var data = _facultyServiceClient.GetAll();
            var result = data.Select(f => Mapping.Mapper.Map<FacultyModel>(f));

            return result;
        }

        public FacultyModel GetFaculty(int FacultyId)
        {
            var selectedData = _facultyServiceClient.GetFaculty(FacultyId);

            return Mapping.Mapper.Map<FacultyModel>(selectedData);
        }

        public FacultyModel GetFacultyIdByName(string name)
        {
            var selectedData = _facultyServiceClient.GetFacultyIdByName(name);

            return Mapping.Mapper.Map<FacultyModel>(selectedData);
        }

        public void InsertFaculty(FacultyModel Faculty)
        {
            var result = Mapping.Mapper.Map<FacultyDTO>(Faculty);

            _facultyServiceClient.InsertFaculty(result);
        }

        public void UpdateFaculty(FacultyModel Faculty)
        {
            var result = Mapping.Mapper.Map<FacultyDTO>(Faculty);

            _facultyServiceClient.UpdateFaculty(result);
        }

        public void DeleteFaculty(int FacultyId)
        {
            _facultyServiceClient.DeleteFaculty(FacultyId);
        }


    }
}