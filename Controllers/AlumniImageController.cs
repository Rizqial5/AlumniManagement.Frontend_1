using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.JobHistoryService;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AlumniManagement.Frontend.Controllers
{

    public class AlumniImageController : Controller
    {
        private IAlumniImageRepository _imageRepository;
        private IAlumniRepository _alumniRepository;
        private int fileSizeLimit = Convert.ToInt32(ConfigurationManager.AppSettings["FileSizeLimit"]);
        private string fileTypes = ConfigurationManager.AppSettings["FileTypes"];
        private string alumniImagesPath = ConfigurationManager.AppSettings["AlumniImagesPath"];

        public AlumniImageController() : this(new AlumniImageRepository(), new AlumniRepository()) { }

        public AlumniImageController(IAlumniImageRepository imageRepository, IAlumniRepository alumniRepository)
        {
            _imageRepository = imageRepository;
            _alumniRepository = alumniRepository;
        }

        // GET: AlumniImage
        public ActionResult Index(int alumniId)
        {

            if (_alumniRepository.GetAlumni(alumniId) == null)
            {
                TempData["ErrorMessage"] = "Alumni Not Found";
                return RedirectToAction("Index", "Alumni");
            }



            ViewBag.AlumniId = alumniId;
            ViewBag.FullName = _alumniRepository.GetAlumni(alumniId).FullName;

            return View();
        }

        public JsonResult GetImages(int alumniId)
        {
            var alumnImages = _imageRepository.GetAllImage(alumniId);

            foreach (var item in alumnImages)
            {
                item.ShowImagePath = @Url.Content(item.ImagePath.Replace("~", "") + '/' + item.FileName);
            }

            var json = Json(new { data = alumnImages }, JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        // GET: AlumniImage/Details/5
        public ActionResult Upload(int alumniId)
        {
            if (_alumniRepository.GetAlumni(alumniId) == null)
            {
                TempData["ErrorMessage"] = "Alumni Not Found";
                return RedirectToAction("Index", "Alumni");
            }
            ViewBag.AlumniId = alumniId;

            return PartialView("_UploadPartial");
        }


        [HttpPost]
        public async Task<ActionResult> UploadImages(int alumniId, HttpPostedFileBase[] files)
        {
            try
            {
                if (files == null || files.Length == 0)
                {
                    TempData["ErrorMessage"] = "Please upload at least one valid file.";
                    return RedirectToAction("Index", new { alumniId = alumniId });
                }

                //validation files

                foreach (var item in files)
                {
                    if (item.ContentLength > fileSizeLimit)
                    {
                        TempData["ErrorMessage"] = "File size must be less than 10 mb";
                        return RedirectToAction("Index", new { alumniId = alumniId });
                    }

                    var extension = Path.GetExtension(item.FileName).ToLower();

                    if (extension != ".jpeg" && extension != ".png")
                    {
                        TempData["ErrorMessage"] = "Only .jpeg anf .png files are allowed";
                        return RedirectToAction("Index", new { alumniId = alumniId });
                    }
                }

                //Upload each file

                var alumniImages = new List<AlumniImageModel>();

                foreach (var item in files)
                {
                    var extension = Path.GetExtension(item.FileName).ToLower();

                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(Server.MapPath(alumniImagesPath), fileName);

                    item.SaveAs(filePath);

                    var image = new AlumniImageModel
                    {
                        AlumniID = alumniId,
                        ImagePath = alumniImagesPath,
                        FileName = fileName,
                        UploadDate = DateTime.Now,
                    };

                    alumniImages.Add(image);
                }

                await _imageRepository.AddImageAsync(alumniImages, alumniId);
                TempData["SuccessMessage"] = "Images uploaded successfully.";
                return RedirectToAction("Index", new { alumniId = alumniId });

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occured while uploading images" + ex.Message;
                return RedirectToAction("Index", new { alumniId = alumniId });
            }
        }



        // POST: AlumniImage/Delete/5
        [HttpPost]
        public async Task<ActionResult> DeleteImage(int id, int alumniId)
        {
            try
            {
                // TODO: Add delete logic here
                var image = _imageRepository.GetImageById(id, alumniId);

                if (image == null)
                {
                    ModelState.AddModelError("", "Image not found.");
                    return RedirectToAction("Index", new { alumniId = alumniId });
                }

                var filePath = Path.Combine(Server.MapPath(alumniImagesPath), image.FileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                await _imageRepository.DeleteImageByIdAsync(id, alumniId);

                return Json(new
                {
                    success = true
                });
            }
            catch
            {
                return Json(new
                {
                    error = true
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteSelectedImages(int alumniId, List<int> selectedImages)
        {
            try
            {
                if (selectedImages == null || selectedImages.Count == 0)
                {
                    ModelState.AddModelError("", "Please select at least one image to delete.");
                    return Json(new
                    {
                        error = true,
                        message = "no item selected for item deletion "
                    });
                }

                foreach (var item in selectedImages)
                {
                    await _imageRepository.DeleteImageByIdAsync(item, alumniId);
                }

                return Json(new
                {
                    success = true,
                    message = "Selected item have been deleted succesfully"
                });
            }
            catch (Exception ex) 
            {
                return Json(new
                {
                    error = true,
                    message = "Error deleting item " + ex.Message
                });
            }
        }
    }
}
