using AlumniManagement.Frontend.AlumniService;
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlumniManagement.Frontend.Interfaces
{
    public interface IAlumniRepository
    {
        
        IEnumerable<AlumniModel> GetAll();

        
        AlumniModel GetAlumni(int alumniId);

        
        void InsertAlumni(AlumniModel alumni);

        void InsertAlumniWitHobbies(AlumniModel alumni);

        
        void UpdateAlumni(AlumniModel alumni);

        void UpdateAlumniWithHobbies(AlumniModel alumni);

        void DeleteAlumni(int alumniId);

        
        int GetDistrictIdByName(string districtName);

        
        int GetStateIdByName(string stateName);

        
        IEnumerable<StateDTO> GetAllStates();

        IEnumerable<DistrictDTO> GetAllDistricts();
        IEnumerable<HobbyDTO> GetAllHobbies();

        IEnumerable<string> GetStatesDistrictName();

        IEnumerable<string> GetMajorFacultiesName();
        IEnumerable<DistrictDTO> GetDistrictByStateId(int stateId);

        void ImportFromExcel(AlumniModel alumniDTO);

        void UpsertAlumni(AlumniModel alumni);

        void UpsertMultipleAlumni(List<AlumniModel> alumni);
    }
}
