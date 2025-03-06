using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.Repositories;
using AlumniManagement.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace AlumniManagement.Frontend.Controllers
{
    [Authorize]
    public class MajorController : Controller
    {
        private IMajorRepository _majorRepository;
        private IFacultyRepository _facultyRepository;

        public MajorController() : this(new MajorRepository(), new FacultyRepository()) { }
        public MajorController(IMajorRepository majorRepository, IFacultyRepository facultyRepository)
        {
            _majorRepository = majorRepository;
            _facultyRepository = facultyRepository;
        }

        // GET: Major
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetMajors()
        {
            try
            {
                var majorsData = _majorRepository.GetAll();

                return Json(new { data = majorsData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = true,
                    message = ex.Message,
                    data = new List<MajorModel>() // Hindari null untuk menghindari DataTable error
                }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Major/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Major/Create
        public ActionResult Create()
        {
            try
            {
                FacultyDDl();

                return PartialView("_CreatePartial");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500; // Set status code agar masuk error AJAX
                return Json(new { message = "Please contact Administrator" }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Major/Create
        [Authorize(Roles = "Superadmin")]
        [HttpPost]
        public ActionResult Create(MajorModel majorModel)
        {
            try
            {
                // TODO: Add insert logic here

                if (ModelState.IsValid)
                {
                    _majorRepository.InsertMajor(majorModel);

                    TempData["SuccessMessage"] = "Major created succesfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;


                return RedirectToAction("Index");
            }
        }

        // GET: Major/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {


                var existingData = _majorRepository.GetMajor(id);

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Major Not Found";

                    return RedirectToAction("Index");
                }

                FacultyDDl();

                return PartialView("_EditPartial", existingData);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500; // Set status code agar masuk error AJAX
                return Json(new { message = "Please contact Administrator" }, JsonRequestBehavior.AllowGet);
            }

        }

        // POST: Major/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, MajorModel majorModel)
        {
            try
            {
                // TODO: Add update logic here

                var existingData = _majorRepository.GetMajor(id);

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Major Not Found";

                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {
                    _majorRepository.UpdateMajor(majorModel);

                    TempData["SuccessMessage"] = "Major created succesfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;


                return RedirectToAction("Index");
            }
        }

        

        // POST: Major/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var existingData = _majorRepository.GetMajor(id);

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Major Not Found";

                    return RedirectToAction("Index");
                }

                _majorRepository.DeleteMajor(id);

                return Json(new
                {
                    success = true
                });
            }
            catch(Exception ex) 
            {
                return Json(new
                {
                    error = true,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public ActionResult DeleteSelected(int[] ids)
        {
            try
            {
                if(ids != null && ids.Length > 0)
                {
                    foreach (var item in ids)
                    {
                        _majorRepository.DeleteMajor(item);
                    }

                    return Json(new
                    {
                        success = true,
                        message = "Selected major has been deleted succesfully"
                    });
                }

                return Json(new
                {
                    success = true,
                    message = "There is no selected major"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = true,
                    message = "Error deleting major: " + ex.Message

                });


            }
        }


        private void FacultyDDl()
        {
            try
            {
                var ddl = _facultyRepository.GetAll();

                ViewBag.FacultyLists = new SelectList(ddl, "FacultyID", "FacultyName");
            }
            catch (Exception ex)
            {
                /// buat message developer, add.errorlog(ex)
                
                throw new Exception();
            }



        }
    }
}
