using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.PostingJobService;
using AlumniManagement.Frontend.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlumniManagement.Frontend.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private IEventRepository _eventRepository;

        private string photoPath = ConfigurationManager.AppSettings["EventPath"];
        private string fileTypes = ConfigurationManager.AppSettings["FileTypes"];
        private int fileSizeLimit = Convert.ToInt32(ConfigurationManager.AppSettings["PhotoSizeLimit"]);
        public EventController() : this(new EventRepository()) { }

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }


        // GET: Event
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            try
            {


                var eventsData = _eventRepository.GetAllEvents();
                foreach (var item in eventsData)
                {
                    if (item.EventImagePath != null)
                    {
                        item.ShowImage = @Url.Content(item.EventImagePath.Replace("~", "") + '/' + item.EventImageName);
                    }

                }

                return Json(new
                {
                    error = false,
                    message = "Success",
                    data = eventsData
                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    error = true,
                    message = ex.Message,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            var newEvent = new EventModel();

            newEvent.IsClosed = false;

            return PartialView("_CreatePartial", newEvent);
        }

        // GET: Event/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {


                var existingData = _eventRepository.GetEventById(id);

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Event Not Found";

                    return RedirectToAction("Index");
                }




                ViewBag.SourceImage = "";
                ViewBag.NameFile = "";

                if (existingData.EventImagePath != null)
                {
                    ViewBag.SourceImage = @Url.Content(existingData.EventImagePath.Replace("~", "") + '/' + existingData.EventImageName);
                    ViewBag.NameFile = existingData.EventImageName;
                }

                return PartialView("_EditPartial", existingData);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 500; // Set status code agar masuk error AJAX
                return Json(new { message = "Please contact support" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpsertEvent(EventModel model, HttpPostedFileBase photoUpload)
        {
            try
            {
                var errors = new Dictionary<string, string>();

                if (photoUpload == null || photoUpload.ContentLength == 0)
                {
                    errors["photoUpload"] = "Photo is required.";
                }

                if (errors.Any())
                {
                    return Json(new { success = false, errors });
                }


                if (ModelState.IsValid)
                {


                    UploadBehaviour(model, photoUpload);

                    _eventRepository.UpsertEvent(model);

                }



                

                return Json(new { success = true, message = "Event Added Successfully" });

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Event update failed. Please try again later." });
            }
        }
        

        private void UploadBehaviour(EventModel model, HttpPostedFileBase photoUpload)
        {

            try
            {

                if (model.EventImagePath != null)
                {
                    var fileExist = Path.Combine(Server.MapPath(model.EventImagePath), model.EventImageName);

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
                model.EventImagePath = photoPath;
                model.EventImageName = fileName;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Failed to upload photo. Please try again later.");
            }
        }









        // POST: Event/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here

                var existingData = _eventRepository.GetEventById(id);


                if (existingData == null)
                {

                    return Json(new
                    {
                        error = true,
                        message = "Event Not found"
                    });
                }

                DeleteLocalImage(existingData);

                _eventRepository.DeleteEvent(id);

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
                    message = "Failed delete event"
                });
            }
        }

        private void DeleteLocalImage(EventModel existingData)
        {
            if (existingData.EventImageName != null)
            {
                var filePath = Path.Combine(Server.MapPath(photoPath), existingData.EventImageName);

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
        public ActionResult DeleteSelected(int[] ids)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {
                    foreach (var item in ids)
                    {
                        var existingData =  _eventRepository.GetEventById(item);

                        DeleteLocalImage(existingData);

                        _eventRepository.DeleteEvent(item); // template yang dirubah
                    }

                    return Json(new
                    {
                        success = true,
                        message = "Selected Event have been deleted succesfully" // message dirubah sesuai objek
                    });

                }

                return Json(new
                {
                    error = true,
                    message = "No Event selected "  // message dirubah sesuai objek
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = true,
                    message = "Failed deleting event"  // message dirubah sesuai objek
                });
            }
        }
    }
}
