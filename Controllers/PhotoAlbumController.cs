using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.Repositories;
using AlumniManagement.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PagedList.Mvc;
using PagedList;

namespace AlumniManagement.Frontend.Controllers
{
    public class PhotoAlbumController : Controller
    {
        private IPhotoAlbumRepository _photoAlbumRepository;

        private string photoPath = ConfigurationManager.AppSettings["GalleryPath"];
        private string fileTypes = ConfigurationManager.AppSettings["FileTypes"];
        private int fileSizeLimit = Convert.ToInt32(ConfigurationManager.AppSettings["PhotoSizeLimit"]);

        public PhotoAlbumController(): this(new PhotoAlbumRepository()) { }

        public PhotoAlbumController(IPhotoAlbumRepository photoAlbumRepository)
        {
            _photoAlbumRepository = photoAlbumRepository;
        }



        // GET: PhotoAlbum
        public ActionResult Index(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int size = pageSize ?? 5;
            ViewBag.PageSize = size;

            var data = _photoAlbumRepository.GetPhotoAlbums().ToList();


            foreach (var item in data)
            {
                if(item.ThumbnailImage == "")
                {
                    item.ThumbnailImage = "/App_Data/Albums/blank-profile-picture-973460_1280.png";
                }
            }


            return View(data.ToPagedList(pageNumber,size));
        }

        // GET: PhotoAlbum/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PhotoAlbum/Create
        public ActionResult CreateAlbum()
        {
            return PartialView("_CreatePhotoAlbum");
        }

        public ActionResult CreateAlbumFromGallery(int albumId)
        {

            var addPhoto = new PhotoModel
            {
                AlbumID = albumId
            };

            return PartialView("_CreatePhotoAlbum", addPhoto);
        }

        // POST: PhotoAlbum/Create
        [HttpPost]
        public ActionResult CreateAlbum(PhotoAlbumModel albumModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _photoAlbumRepository.InsertPhotoAlbum(albumModel);

                    TempData["SuccessMessage"] = "Album created succesfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                ModelState.AddModelError("", "Unable to Add Album due to " + ex.Message);

                return View();
            }
        }

        // GET: PhotoAlbum/Edit/5
        public ActionResult EditAlbum(int id)
        {
            var existingData = _photoAlbumRepository.GetPhotoAlbumById(id);

            if (existingData == null)
            {
                TempData["ErrorMessage"] = "Album Not Found";

                return RedirectToAction("Index");
            }

            return PartialView("_EditPhotoAlbum", existingData);
        }

        // POST: PhotoAlbum/Edit/5
        [HttpPost]
        public ActionResult EditAlbum(int id, PhotoAlbumModel photoAlbumModel)
        {
            try
            {

                var existingData = _photoAlbumRepository.GetPhotoAlbumById(id);

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Album not Found";

                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {

                    _photoAlbumRepository.UpdatePhotoAlbum(photoAlbumModel);

                    TempData["SuccessMessage"] = "Album updated succesfully";
                }


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                ModelState.AddModelError("", "Unable to Update due to " + ex.Message);

                return View();
            }
        }


        // POST: PhotoAlbum/Delete/5
        [HttpPost]
        public ActionResult DeleteAlbum(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var existingData = _photoAlbumRepository.GetPhotoAlbumById(id);

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Album not Found";

                    return Json(new
                    {
                        error = true,
                        message = "Album not Found"
                    });
                }

                _photoAlbumRepository.DeletePhotoAlbum(id);

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    error = true,
                    message = ex.Message
                });
            }
        }

        public ActionResult ListPhoto(int albumId, int? page, int? pageSize)
        {
            var listPhotoData = _photoAlbumRepository.GetAllPhotoByAlbumId(albumId).ToList();

            int pageNumber = page ?? 1;
            int size = pageSize ?? 10;
            ViewBag.PageSize = size;

            foreach (var item in listPhotoData)
            {
                if (item.PhotoPath != null)
                {
                    item.ShowPhoto = @Url.Content(item.PhotoPath.Replace("~", "") + '/' + item.PhotoFilleName);
                }

            }

            ViewBag.AlbumName = _photoAlbumRepository.GetPhotoAlbumById(albumId).AlbumName;
            ViewBag.AlbumId = albumId;
            

            return View("ListPhoto",listPhotoData.ToPagedList(pageNumber,size));
        }


        public ActionResult AddPhoto()
        {
            var albumDDL = _photoAlbumRepository.GetPhotoAlbums();

            ViewBag.PhotoAlbum = new SelectList(albumDDL, "AlbumID", "AlbumName");
            ViewBag.DisableDropdown = false; // atau false
            ViewBag.isFromPhoto = false;
            ViewBag.AlbumId = 0;

            return PartialView("_AddPhoto");
        }

        public ActionResult AddPhotoFromPhoto(int albumId)
        {
            var albumDDL = _photoAlbumRepository.GetPhotoAlbums();

            ViewBag.PhotoAlbum = new SelectList(albumDDL, "AlbumID", "AlbumName");
            ViewBag.DisableDropdown = true;
            ViewBag.IsFromPhoto = true;
            ViewBag.AlbumId=albumId;

            return PartialView("_AddPhoto");
        }

        [HttpPost]
        public ActionResult AddPhoto(PhotoModel model, HttpPostedFileBase photoUpload, bool isFromPhoto, int checkRedirect)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    UploadBehaviour(model, photoUpload);

                    _photoAlbumRepository.InsertPhoto(model, model.AlbumID);
                }

                TempData["SuccessMessage"] = "Photo Added Succesfully";

                if(isFromPhoto)
                {
                    return RedirectToAction("ListPhoto", new {albumId = model.AlbumID});
                }

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = "Photo Added Failed " + ex.Message;

                if (isFromPhoto)
                {
                    return RedirectToAction("ListPhoto", new { albumId = model.AlbumID });
                }

                return RedirectToAction("Index");
            }
        }

        private void UploadBehaviour(PhotoModel model, HttpPostedFileBase photoUpload)
        {
            if (photoUpload == null)
            {
                return;
            }

            if (model.PhotoPath != null)
            {
                var fileExist = Path.Combine(Server.MapPath(model.PhotoPath), model.PhotoFilleName);

                if (System.IO.File.Exists(fileExist))
                {
                    System.IO.File.Delete(fileExist);
                }
            }

            string[] allowedExtensions = { ".jpeg", ".jpg", ".png" };
            string fileExtension = Path.GetExtension(photoUpload.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("", "Invalid file type. Only JPEG, JPG, and PNG are allowed.");
            }

            // Validasi ukuran file (3MB)
            if (photoUpload.ContentLength > fileSizeLimit)
            {
                ModelState.AddModelError("", "File size exceeds 3MB. Please select a smaller file.");

            }

            // Simpan file ke server
            string fileName = Guid.NewGuid().ToString() + fileExtension;
            string filePath = Path.Combine(Server.MapPath(photoPath), fileName);
            photoUpload.SaveAs(filePath);

            // Simpan path ke model
            model.PhotoPath = photoPath;
            model.PhotoFilleName = fileName;
        }

        [HttpPost]
        public ActionResult DeletePhoto(int photoId, int albumId)
        {
            try
            {
                // TODO: Add delete logic here

                var existingData = _photoAlbumRepository.GetPhotoByAlbumIdAndPhotoId(photoId, albumId);


                if (existingData == null)
                {

                    return Json(new
                    {
                        error = true,
                        message = "Photo Not found"
                    });
                }

                DeleteLocalImage(existingData);

                _photoAlbumRepository.DeletePhoto(albumId,photoId);

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = true,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public ActionResult DeletePhotoSelected(int[] ids, int albumId)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {
                    foreach (var item in ids)
                    {
                        var existingData = _photoAlbumRepository.GetPhotoByAlbumIdAndPhotoId(albumId,item);

                        DeleteLocalImage(existingData);

                        _photoAlbumRepository.DeletePhoto(albumId, item); // template yang dirubah
                    }

                    return Json(new
                    {
                        success = true,
                        message = "Selected Photo have been deleted succesfully" // message dirubah sesuai objek
                    });

                }

                return Json(new
                {
                    error = true,
                    message = "No Photo selected "  // message dirubah sesuai objek
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = true,
                    message = "Error deleting Photo:  " + ex.Message  // message dirubah sesuai objek
                });
            }
        }

        [HttpPost]
        public ActionResult DeleteAlbumSelected(int[] ids)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {
                    foreach (var item in ids)
                    {

                        _photoAlbumRepository.DeletePhotoAlbum(item);// template yang dirubah
                    }

                    return Json(new
                    {
                        success = true,
                        message = "Selected Album have been deleted succesfully" // message dirubah sesuai objek
                    });

                }

                return Json(new
                {
                    error = true,
                    message = "No Album selected "  // message dirubah sesuai objek
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = true,
                    message = "Error deleting Album:  " + ex.Message  // message dirubah sesuai objek
                });
            }
        }

        private void DeleteLocalImage(PhotoModel existingData)
        {
            if (existingData.PhotoFilleName != null)
            {
                var filePath = Path.Combine(Server.MapPath(photoPath), existingData.PhotoFilleName);

                if (filePath != null)
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult SetThumbnail(int photoId, int albumId)
        {
            try
            {
                var albumPhotos = _photoAlbumRepository.GetAllPhotoByAlbumId(albumId);

                if (albumPhotos == null || !albumPhotos.Any())
                {
                    return Json(new { success = false, message = "Album not found or empty." });
                }


                // Set foto yang dipilih sebagai thumbnail
                var selectedPhoto = albumPhotos.FirstOrDefault(p => p.PhotoID == photoId);
                if (selectedPhoto != null)
                {
                    
                    _photoAlbumRepository.SetThumbnail(photoId, albumId);
                }

                return Json(new { success = true, message = "Album thumbnail updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error updating thumbnail: " + ex.Message });
            }
        }
    }
}
