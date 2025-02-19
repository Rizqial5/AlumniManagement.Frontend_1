using AlumniManagement.Frontend.AlumniImageService;
using AlumniManagement.Frontend.AlumniService;
using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.JobHistoryService;
using AlumniManagement.Frontend.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AlumniManagement.Frontend.Repositories
{
    public class AlumniImageRepository : IAlumniImageRepository
    {
        private AlumniImageServiceClient _imageServiceClient;
        private AlumniServiceClient _alumniServiceclient;

        public AlumniImageRepository()
        {
            _imageServiceClient = new AlumniImageServiceClient();
            _alumniServiceclient = new AlumniServiceClient();
        }

        public async Task AddImageAsync(IEnumerable<AlumniImageModel> imageDTO, int alumniId)
        {
            var result = Mapping.Mapper.Map<List<ImageDTO>>(imageDTO);

            await _imageServiceClient.AddImageAsync(result.ToArray(), alumniId);
        }

        public async Task DeleteImageByIdAsync(int imageId, int alumniId)
        {
            var alumni = _alumniServiceclient.GetAlumni(alumniId);
            if (alumni != null)
            {
                var existingImage = _imageServiceClient.GetImageById(imageId,alumniId);
                
                await _imageServiceClient.DeleteImageByIdAsync(imageId, alumniId);
            }
            else
            {
                throw new Exception("Data not found");
            }
        }

        public IEnumerable<AlumniImageModel> GetAllImage(int alumniId)
        {
            var alumni = _alumniServiceclient.GetAlumni(alumniId);
            if (alumni != null)
            {
                var images = _imageServiceClient.GetAllImage( alumniId);

                var result = Mapping.Mapper.Map<IEnumerable<AlumniImageModel>>(images);

                return result;
            }
            else
            {
                return Enumerable.Empty<AlumniImageModel>();
                            
            }
        }

        public AlumniImageModel GetImageById(int imageId, int alumniId)
        {
            var alumni = _alumniServiceclient.GetAlumni(alumniId);
            if (alumni != null)
            {
                var images = _imageServiceClient.GetImageById(imageId,alumniId);

                var result = Mapping.Mapper.Map<AlumniImageModel>(images);

                return result;
            }
            else
            {
                return null;

            }
        }
    }
}