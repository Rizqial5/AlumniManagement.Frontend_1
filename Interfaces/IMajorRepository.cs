using AlumniManagement.Frontend.MajorService;
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlumniManagement.Frontend.Interfaces
{
    public interface IMajorRepository
    {
        
        IEnumerable<MajorModel> GetAll();

        
        MajorModel GetMajor(int majorId);

        
        MajorModel GetMajorIdByName(string name);

        
        void InsertMajor(MajorModel major);

        
        void UpdateMajor(MajorModel major);

        
        void DeleteMajor(int majorId);

        IEnumerable<MajorModel> GetMajorIdByFacultyId(int facultyId);
    }
}
