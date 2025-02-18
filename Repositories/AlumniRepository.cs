using AlumniManagement.Frontend.AlumniService;
using AlumniManagement.Frontend.FacultyService;
using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Repositories
{
    public class AlumniRepository : IAlumniRepository
    {

        private AlumniServiceClient _alumniServiceClient;

        public AlumniRepository()
        {
            _alumniServiceClient = new AlumniServiceClient();
        }


        public IEnumerable<AlumniModel> GetAll()
        {
            var data = _alumniServiceClient.GetAll();
            var result = data.Select(f => Mapping.Mapper.Map<AlumniModel>(f)).ToList();

            return result;
        }

        public AlumniModel GetAlumni(int alumniId)
        {
            var selectedData = _alumniServiceClient.GetAlumni(alumniId);

            return Mapping.Mapper.Map<AlumniModel>(selectedData);
        }

        public int GetDistrictIdByName(string districtName)
        {
            var selectedData = _alumniServiceClient.GetDistrictIdByName(districtName);

            return selectedData;

        }

        public int GetStateIdByName(string stateName)
        {
            var selectedData = _alumniServiceClient.GetStateIdByName(stateName);

            return selectedData;
        }

        public void InsertAlumni(AlumniModel alumni)
        {
            var result = Mapping.Mapper.Map<AlumniDTO>(alumni);

            _alumniServiceClient.InsertAlumni(result);
        }

        public void UpdateAlumni(AlumniModel alumni)
        {
            var result = Mapping.Mapper.Map<AlumniDTO>(alumni);

            _alumniServiceClient.UpdateAlumni(result);
        }

        public void DeleteAlumni(int alumniId)
        {
            _alumniServiceClient.DeleteAlumni(alumniId);
        }

        public IEnumerable<StateDTO> GetAllStates()
        {
            var data = _alumniServiceClient.GetAllStates();


            return data;
        }

        public IEnumerable<DistrictDTO> GetDistrictByStateId(int stateId)
        {
            var data = _alumniServiceClient.GetDistrictByStateId(stateId);


            return data;
        }

        public IEnumerable<DistrictDTO> GetAllDistricts()
        {
            var data = _alumniServiceClient.GetAllDistricts().ToList();

            return data;
        }

        public IEnumerable<string> GetStatesDistrictName()
        {
            var data = _alumniServiceClient.GetStatesDistrictName();

            return data;
        }

        public IEnumerable<string> GetMajorFacultiesName()
        {
            var data = _alumniServiceClient.GetMajorFacultiesName();

            return data;
        }

        public void ImportFromExcel(AlumniModel alumniDTO)
        {
            throw new NotImplementedException();
        }
    }
}