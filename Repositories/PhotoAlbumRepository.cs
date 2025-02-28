using AlumniManagement.Frontend.FacultyService;
using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.PhotoAlbumService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Repositories
{
    public class PhotoAlbumRepository : IPhotoAlbumRepository
    {

        private PhotoAlbumServiceClient _photoAlbumClient;

        public PhotoAlbumRepository()
        {
            _photoAlbumClient = new PhotoAlbumServiceClient();
        }


        public IEnumerable<PhotoModel> GetAllPhotoByAlbumId(int AlbumId)
        {
            var data = _photoAlbumClient.GetAllPhotoByAlbumId(AlbumId);
            var result = data.Select(f => Mapping.Mapper.Map<PhotoModel>(f));

            return result.ToList();
        }

        public IEnumerable<PhotoAlbumModel> GetPhotoAlbums()
        {
            var data = _photoAlbumClient.GetPhotoAlbums();
            var result = data.Select(f => Mapping.Mapper.Map<PhotoAlbumModel>(f));

            return result.ToList();
        }

        public PhotoAlbumModel GetPhotoAlbumById(int AlbumId)
        {
            var selectedData = _photoAlbumClient.GetPhotoAlbumById(AlbumId);

            return Mapping.Mapper.Map<PhotoAlbumModel>(selectedData);
        }


        public PhotoModel GetPhotoByAlbumIdAndPhotoId(int AlbumId, int photoId)
        {
            var selectedData = _photoAlbumClient.GetPhotoByAlbumIdAndPhotoId(AlbumId, photoId);

            return Mapping.Mapper.Map<PhotoModel>(selectedData);
        }

        public void InsertPhoto(PhotoModel photo, int albumID)
        {
            var result = Mapping.Mapper.Map<PhotoDTO>(photo);

            _photoAlbumClient.InsertPhoto(result, albumID);
        }

        public void InsertPhotoAlbum(PhotoAlbumModel photoAlbum)
        {
            var result = Mapping.Mapper.Map<PhotoAlbumDTO>(photoAlbum);

            _photoAlbumClient.InsertPhotoAlbum(result);
        }

        public void UpdatePhotoAlbum(PhotoAlbumModel photoAlbum)
        {
            var result = Mapping.Mapper.Map<PhotoAlbumDTO>(photoAlbum);

            _photoAlbumClient.UpdatePhotoAlbum(result);
        }

        public void DeletePhoto(int albumID, int photoID)
        {
            _photoAlbumClient.DeletePhoto(albumID,photoID);
        }

        public void DeletePhotoAlbum(int albumID)
        {
            _photoAlbumClient.DeletePhotoAlbum(albumID);
        }
    }
}