using AlumniManagement.Frontend.AlumniImageService;
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlumniManagement.Frontend.Interfaces
{
    public interface IAlumniImageRepository
    {
        
        IEnumerable<AlumniImageModel> GetAllImage(int alumniId);

        
        AlumniImageModel GetImageById(int imageId, int alumniId);

        
        Task DeleteImageByIdAsync(int imageId, int alumniId);

        
        Task AddImageAsync(IEnumerable<AlumniImageModel> imageDTO, int alumniId);
    }
}
