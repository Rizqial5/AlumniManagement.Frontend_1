using AlumniManagement.Frontend;
using AlumniManagement.Frontend.AlumniService;
using AlumniManagement.Frontend.FacultyService;
using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
            try
            {

                var data = _facultyServiceClient.GetAll();


                var result = data.Select(f => Mapping.Mapper.Map<FacultyModel>(f));

                return result;
            }
            catch
            {

                //Error Log for developer


                //Error for Client Side
                throw new Exception("Faculty Service is not available."); 


            }


        }

        public FacultyModel GetFaculty(int FacultyId)
        {
            try
            {
                var selectedData = _facultyServiceClient.GetFaculty(FacultyId);

                return Mapping.Mapper.Map<FacultyModel>(selectedData);
            }
            catch
            {
                //Error Log for developer


                //Error for Client Side
                throw new Exception("Faculty Service is not available.");
            }

        }

        public FacultyModel GetFacultyIdByName(string name)
        {
            try
            {
                var selectedData = _facultyServiceClient.GetFacultyIdByName(name);

                return Mapping.Mapper.Map<FacultyModel>(selectedData);
            }
            catch
            {
                //Error Log for developer


                //Error for Client Side
                throw new Exception("Faculty Service is not available.");
            }

        }

        public void InsertFaculty(FacultyModel Faculty)
        {
            try
            {
                var result = Mapping.Mapper.Map<FacultyDTO>(Faculty);

                _facultyServiceClient.InsertFaculty(result);
            }
            catch
            {
                //Error Log for developer


                //Error for Client Side
                throw new Exception("Faculty Service is not available.");
            }

        }

        public void UpdateFaculty(FacultyModel Faculty)
        {
            try
            {
                var result = Mapping.Mapper.Map<FacultyDTO>(Faculty);

                _facultyServiceClient.UpdateFaculty(result);
            }
            catch 
            {
                //Error Log for developer


                //Error for Client Side
                throw new Exception("Faculty Service is not available.");
            }

        }

        public void DeleteFaculty(int FacultyId)
        {
            try
            {
                _facultyServiceClient.DeleteFaculty(FacultyId);
            }
            catch
            {
                //Error Log for developer


                //Error for Client Side
                throw new Exception("Faculty Service is not available.");
            }

        }


    }
}