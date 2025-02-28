using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.PhotoAlbumService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlumniManagement.Frontend.Interfaces
{
    public interface IPhotoAlbumRepository
    {
        
        IEnumerable<PhotoAlbumModel> GetPhotoAlbums();



        IEnumerable<PhotoModel> GetAllPhotoByAlbumId(int AlbumId);

        
        PhotoAlbumModel GetPhotoAlbumById(int AlbumId);

        
        PhotoModel GetPhotoByAlbumIdAndPhotoId(int AlbumId, int photoId);

        
        void InsertPhotoAlbum(PhotoAlbumModel photoAlbum);

        
        void InsertPhoto(PhotoModel photo, int albumID);

        
        void UpdatePhotoAlbum(PhotoAlbumModel photoAlbum);

        
        void DeletePhotoAlbum(int albumID);

        
        void DeletePhoto(int albumID, int photoID);

        void SetThumbnail(int photoId, int albumId);
    }
}
