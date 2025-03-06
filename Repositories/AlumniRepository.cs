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
                try
                {
                    var data = _alumniServiceClient.GetAll();
                    var result = data.Select(f => Mapping.Mapper.Map<AlumniModel>(f)).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to retrieve alumni data. Please contact support.");
                }
            }

            public AlumniModel GetAlumni(int alumniId)
            {
                try
                {
                    var selectedData = _alumniServiceClient.GetAlumni(alumniId);
                    return Mapping.Mapper.Map<AlumniModel>(selectedData);
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception($"Failed to retrieve data. Please contact support.");
                }
            }

            public int GetDistrictIdByName(string districtName)
            {
                try
                {
                    return _alumniServiceClient.GetDistrictIdByName(districtName);
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to get data. Please contact support.");
                }
            }

            public int GetStateIdByName(string stateName)
            {
                try
                {
                    return _alumniServiceClient.GetStateIdByName(stateName);
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to get data. Please contact support.");
                }
            }

            public void InsertAlumni(AlumniModel alumni)
            {
                try
                {
                    var result = Mapping.Mapper.Map<AlumniDTO>(alumni);
                    _alumniServiceClient.InsertAlumni(result);
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to add alumni data. Please contact support.");
                }
            }

            public void UpdateAlumni(AlumniModel alumni)
            {
                try
                {
                    var result = Mapping.Mapper.Map<AlumniDTO>(alumni);
                    _alumniServiceClient.UpdateAlumni(result);
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to update alumni data. Please contact support.");
                }
            }

            public void DeleteAlumni(int alumniId)
            {
                try
                {
                    _alumniServiceClient.DeleteAlumni(alumniId);
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception($"Failed to delete alumni . Please contact support.");
                }
            }

            public IEnumerable<StateDTO> GetAllStates()
            {
                try
                {
                    return _alumniServiceClient.GetAllStates();
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to retrieve data. Please contact support.");
                }
            }

            public IEnumerable<DistrictDTO> GetDistrictByStateId(int stateId)
            {
                try
                {
                    return _alumniServiceClient.GetDistrictByStateId(stateId);
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to retrieve data. Please contact support.");
                }
            }

            public IEnumerable<DistrictDTO> GetAllDistricts()
            {
                try
                {
                    return _alumniServiceClient.GetAllDistricts().ToList();
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to retrieve data. Please contact support.");
                }
            }

            public IEnumerable<HobbyDTO> GetAllHobbies()
            {
                try
                {
                    return _alumniServiceClient.GetAllHobbies().ToList();
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to retrieve data. Please contact support.");
                }
            }

            public IEnumerable<string> GetStatesDistrictName()
            {
                try
                {
                    return _alumniServiceClient.GetStatesDistrictName();
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to retrieve data. Please contact support.");
                }
            }

            public IEnumerable<string> GetMajorFacultiesName()
            {
                try
                {
                    return _alumniServiceClient.GetMajorFacultiesName();
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to retrieve data. Please contact support.");
                }
            }

            public void InsertAlumniWitHobbies(AlumniModel alumni)
            {
                try
                {
                    var result = Mapping.Mapper.Map<AlumniDTO>(alumni);
                    _alumniServiceClient.InsertAlumniWithHobbies(result);
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to insert alumni . Please contact support.");
                }
            }

            public void UpdateAlumniWithHobbies(AlumniModel alumni)
            {
                try
                {
                    var result = Mapping.Mapper.Map<AlumniDTO>(alumni);
                    _alumniServiceClient.UpdateAlumniWithHobbies(result);
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to update alumni . Please contact support.");
                }
            }

            public void UpsertAlumni(AlumniModel alumni)
            {
                try
                {
                    var result = Mapping.Mapper.Map<AlumniDTO>(alumni);
                    _alumniServiceClient.UpsertAlumni(result);
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to add alumni data. Please contact support.");
                }
            }

            public void UpsertMultipleAlumni(List<AlumniModel> alumni)
            {
                try
                {
                    var result = Mapping.Mapper.Map<List<AlumniDTO>>(alumni);
                    _alumniServiceClient.UpsertMultipleAlumni(result.ToArray());
                }
                catch (Exception ex)
                {
                    // Developer Debug: Log ex.ToString() atau ex.StackTrace
                    throw new Exception("Failed to import multiple alumni records. Please contact support.");
                }
            }
        }

    
}