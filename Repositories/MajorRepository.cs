using AlumniManagement.Frontend.AlumniService;
using AlumniManagement.Frontend.FacultyService;
using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.MajorService;
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Repositories
{
    public class MajorRepository : IMajorRepository
    {
        private MajorServiceClient _majorServiceClient;

        public MajorRepository()
        {
            _majorServiceClient = new MajorServiceClient();
        }

        public IEnumerable<MajorModel> GetAll()
        {
            try
            {
                var data = _majorServiceClient.GetAll();
                var result = data.Select(f => Mapping.Mapper.Map<MajorModel>(f));


                return result;
            }
            catch
            {

                //Error Log for developer


                //Error for Client Side
                throw new Exception("Major Service is not available.");
            }
        }

        public MajorModel GetMajor(int majorId)
        {
            try
            {
                var selectedData = _majorServiceClient.GetMajor(majorId);

                return Mapping.Mapper.Map<MajorModel>(selectedData);
            }
            catch
            {

                //Error Log for developer


                //Error for Client Side
                throw new Exception("Major Service is not available.");
            }
        }

        public MajorModel GetMajorIdByName(string name)
        {
            try
            {

                var selectedData = _majorServiceClient.GetMajorIdByName(name);

                return Mapping.Mapper.Map<MajorModel>(selectedData);
            }
            catch
            {
                //Error Log for developer


                //Error for Client Side
                throw new Exception("Major Service is not available.");
            }
        }

        public void InsertMajor(MajorModel major)
        {
            try
            {


                var result = Mapping.Mapper.Map<MajorDTO>(major);

                _majorServiceClient.InsertMajor(result);
            }
            catch
            {
                //Error Log for developer


                //Error for Client Side
                throw new Exception("Major Service is not available.");
            }
        }

        public void UpdateMajor(MajorModel major)
        {
            try
            {
                var result = Mapping.Mapper.Map<MajorDTO>(major);

                _majorServiceClient.UpdateMajor(result);
            }
            catch
            {
                //Error Log for developer


                //Error for Client Side
                throw new Exception("Major Service is not available.");
            }
        }

        public void DeleteMajor(int majorId)
        {
            try
            {
                _majorServiceClient.DeleteMajor(majorId);
            }
            catch
            {
                //Error Log for developer


                //Error for Client Side
                throw new Exception("Major Service is not available.");
            }
        }

        public IEnumerable<MajorModel> GetMajorIdByFacultyId(int facultyId)
        {
            try
            {
                var data = _majorServiceClient.GetMajorIdByFacultyId(facultyId);

                var result = data.Select(d => Mapping.Mapper.Map<MajorModel>(d));

                return result;
            }
            catch
            {
                //Error Log for developer


                //Error for Client Side
                throw new Exception("Major Service is not available.");
            }
        }
    }
}