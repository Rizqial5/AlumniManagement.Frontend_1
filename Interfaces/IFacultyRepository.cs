using AlumniManagement.Frontend.FacultyService;
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlumniManagement.Frontend.Interfaces
{
    public interface IFacultyRepository
    {
        
        IEnumerable<FacultyModel> GetAll();

        
        FacultyModel GetFaculty(int FacultyId);

        
        FacultyModel GetFacultyIdByName(string name);

        
        void InsertFaculty(FacultyModel Faculty);

        
        void UpdateFaculty(FacultyModel Faculty);

        
        void DeleteFaculty(int FacultyId);

    }
}
