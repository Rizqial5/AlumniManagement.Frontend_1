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
            var data = _majorServiceClient.GetAll();
            var result = data.Select(f => Mapping.Mapper.Map<MajorModel>(f));


            return result;
        }

        public MajorModel GetMajor(int majorId)
        {
            var selectedData = _majorServiceClient.GetMajor(majorId);

            

            return Mapping.Mapper.Map<MajorModel>(selectedData);
        }

        public MajorModel GetMajorIdByName(string name)
        {
            var selectedData = _majorServiceClient.GetMajorIdByName(name);

            return Mapping.Mapper.Map<MajorModel>(selectedData);
        }

        public void InsertMajor(MajorModel major)
        {
            var result = Mapping.Mapper.Map<MajorDTO>(major);

            _majorServiceClient.InsertMajor(result);
        }

        public void UpdateMajor(MajorModel major)
        {
            var result = Mapping.Mapper.Map<MajorDTO>(major);

            _majorServiceClient.UpdateMajor(result);
        }

        public void DeleteMajor(int majorId)
        {
           _majorServiceClient.DeleteMajor(majorId);
        }

        public IEnumerable<MajorModel> GetMajorIdByFacultyId(int facultyId)
        {
            var data = _majorServiceClient.GetMajorIdByFacultyId(facultyId);

            var result = data.Select(d => Mapping.Mapper.Map<MajorModel>(d));

            return result;
        }
    }
}