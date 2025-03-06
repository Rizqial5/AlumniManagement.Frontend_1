using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlumniManagement.Web.Controllers
{
    [Authorize]
    public class FacultyController : Controller
    {
        private IFacultyRepository _facultyRepository;

        public FacultyController() : this(new FacultyRepository()) { }

        public FacultyController(IFacultyRepository facultyRepository)
        {
            _facultyRepository = facultyRepository;
        }

        // GET: Faculty
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetRooms()
        {
            try
            {
                var facultiesData = _facultyRepository.GetAll();

                if (facultiesData == null || !facultiesData.Any())
                {
                    return Json(new
                    {
                        error = true,
                        message = "No data available.",
                        data = new List<FacultyModel>() // Pastikan `data` selalu ada sebagai array
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    error = false,
                    message = "Success",
                    data = facultiesData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = true,
                    message = ex.Message,
                    data = new List<FacultyModel>() // Hindari null untuk menghindari DataTable error
                }, JsonRequestBehavior.AllowGet);
            }

        }





        // GET: Faculty/Create
        [Authorize(Roles = "Superadmin")]
        public ActionResult Create()
        {
            return PartialView("_CreatePartial");
        }

        // POST: Faculty/Create
        [Authorize(Roles = "Superadmin")]
        [HttpPost]
        public ActionResult Create(FacultyModel facultyModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _facultyRepository.InsertFaculty(facultyModel);

                    TempData["SuccessMessage"] = "Faculty created succesfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                

                return RedirectToAction("Index");
            }
        }

        // GET: Faculty/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                 var existingData = _facultyRepository.GetFaculty(id);

            if (existingData == null)
            {
                TempData["ErrorMessage"] = "Faculty Not Found";

                return RedirectToAction("Index");
            }

                return PartialView("_EditPartial",existingData);
            }
            catch (Exception ex)
            {
                
                return PartialView("_EditPartial");
            }
           
        }

        // POST: Faculty/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FacultyModel facultyModel)
        {
            try
            {

                var existingData = _facultyRepository.GetFaculty(id);

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Faculty not Found";

                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {

                    _facultyRepository.UpdateFaculty(facultyModel);

                    TempData["SuccessMessage"] = "Faculty updated succesfully";
                }


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                

                return RedirectToAction("Index");
            }
        }

        // GET: Faculty/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Faculty/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                //if(!_facultyRepository.CheckFaucltyFKConstraint(id))
                //{
                //    TempData["ErrorMessage"] = "Faculty has record in other table ";

                //    return Json(new
                //    {
                //        success = false,
                //        message = "Faculty has record in other table "
                //    });
                //}

                // TODO: Add delete logic here
                var existingData = _facultyRepository.GetFaculty(id);

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Faculty not Found";

                    return RedirectToAction("Index");
                }

                _facultyRepository.DeleteFaculty(id);

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
        public ActionResult DeleteSelected(int[] ids)
        {
            try
            {
                if(ids != null && ids.Length > 0)
                {
                    foreach (var item in ids)
                    {
                        _facultyRepository.DeleteFaculty(item);
                    }

                    return Json(new 
                    { 
                        success = true,
                        message = "Selected item have been deleted succesfully"
                    });

                }

                return Json(new
                {
                    error = true,
                    message = "no item selected for item deletion "
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
