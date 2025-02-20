using AlumniManagement.Frontend.AlumniService;
using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.Repositories;
using AlumniManagement.Web.Repositories;
using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlumniManagement.Frontend.Controllers
{
    public class AlumniController : Controller
    {

        private IAlumniRepository _alumniRepository;
        private IFacultyRepository _facultyRepository;
        private IMajorRepository _majorRepository;
        private IExcelRepository _excelRepository;
        private IJobRepository _jobRepository;

        public AlumniController() : this(new AlumniRepository(), new FacultyRepository(),
            new MajorRepository(), new ExcelRepository(), new JobRepository()) { }

        public AlumniController(IAlumniRepository alumniRepository, 
            IFacultyRepository facultyRepository,
            IMajorRepository majorRepository,
            IExcelRepository excelRepository,
            IJobRepository jobRepository)
        {
            _alumniRepository = alumniRepository;
            _facultyRepository = facultyRepository;
            _majorRepository = majorRepository;
            _excelRepository = excelRepository;
            _jobRepository = jobRepository;
        }


        // GET: Alumni
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadPartialView(string partialViewName, string modalTitle, int id)
        {
            ViewBag.ModalTitle = modalTitle;

            switch (partialViewName)
            {
                case "_CreatePartial":

                    var alumniModel = new AlumniModel();

                    alumniModel.DistrictDDL = new List<SelectListItem>();
                    alumniModel.MajorDDl = new List<SelectListItem>();

                    StateAndFacultyDdl();

                    return PartialView(partialViewName, alumniModel);
                case "_EditPartial":
                    var editModel = _alumniRepository.GetAlumni(id);
                    if (editModel == null) return Content("Data not found");
                    return PartialView(partialViewName, editModel);

                case "_DetailsPartial":
                    var detailModel = _alumniRepository.GetAlumni(id);
                    if (detailModel == null) return Content("Data not found");
                    return PartialView(partialViewName, detailModel);

                default:
                    return Content("Invalid partial view requested.");
            }
        }

        public JsonResult GetAlumnis()
        {
            var alumniesData = _alumniRepository.GetAll();

            foreach (var item in alumniesData)
            {
                item.ShowDateOfBirth = DateConverter(item.DateOfBirth);
            }

            return Json(new { data = alumniesData}, JsonRequestBehavior.AllowGet);
        }

        public string DateConverter(DateTime? dateTime)
        {
            if (dateTime == DateTime.MinValue || dateTime ==  null) { return "N/A"; }

            return dateTime.Value.ToString("yyyy-MM-dd");
        }

        // GET: Alumni/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Alumni/Create
        //public ActionResult Create()
        //{

        //    var alumniModel = new AlumniModel();

        //    alumniModel.DistrictDDL = new List<SelectListItem>();
        //    alumniModel.MajorDDl = new List<SelectListItem>();

        //    StateAndFacultyDdl();
        //    return View(alumniModel);
        //}

        public ActionResult Create()
        {

            var alumniModel = new AlumniModel();

            alumniModel.DistrictDDL = new List<SelectListItem>();
            alumniModel.MajorDDl = new List<SelectListItem>();

            StateAndFacultyDdl();
            return PartialView("_CreatePartial", alumniModel);
        }

        public ActionResult CreateLast()
        {

            var alumniModel = new AlumniModel();

            alumniModel.DistrictDDL = new List<SelectListItem>();
            alumniModel.MajorDDl = new List<SelectListItem>();

            StateAndFacultyDdl();

            return View(alumniModel);
        }

        // POST: Alumni/Create
        [HttpPost]
        public ActionResult Create(AlumniModel alumniModel)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {

                    //Check if there any hobbies

                    if(alumniModel.Hobbies.Count() >0)
                    {
                        _alumniRepository.InsertAlumniWitHobbies(alumniModel);
                    }
                    else
                    {
                        _alumniRepository.InsertAlumni(alumniModel);
                    }

                    TempData["SuccessMessage"] = "Alumni created succesfully";
                }


                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                ModelState.AddModelError("", "Unable to Add due to " + ex.Message);
                StateAndFacultyDdl();
                RepopulateCascadeDdl(alumniModel);

                return View();
            }
        }

        // GET: Alumni/Edit/5
        public ActionResult Edit(int id)
        {

            var existingData = _alumniRepository.GetAlumni(id);

            if (existingData == null)
            {
                TempData["ErrorMessage"] = "Alumni Not Found";

                return RedirectToAction("Index");
            }

            StateAndFacultyDdl();

            var populateForm = RepopulateEditForm(existingData);


            return PartialView("_EditPartial",populateForm);
        }

        private AlumniModel RepopulateEditForm(AlumniModel alumniModel)
        {
            alumniModel.DistrictDDL = _alumniRepository.GetAllDistricts()
                .Where(d => d.StateID == alumniModel.StateID)
                .Select(d => new SelectListItem
                {
                    Value = d.DistrictID.ToString(),
                    Text = d.DistrictName,
                    Selected = d.StateID == alumniModel.StateID
                }).ToList();

            alumniModel.MajorDDl = _majorRepository.GetAll()
                .Where(m => m.FacultyID == alumniModel.FacultyID)
                .Select(m => new SelectListItem
                {
                    Value = m.MajorID.ToString(),
                    Text = m.MajorName,
                    Selected = m.FacultyID == alumniModel.FacultyID

                }).ToList();

            

            StateAndFacultyDdl();

            return alumniModel;
        }

        // POST: Alumni/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, AlumniModel alumniModel)
        {
            var existingData = _alumniRepository.GetAlumni(id);

            try
            {
                // TODO: Add update logic here

                

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Alumni Not Found";

                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {
                    _alumniRepository.UpdateAlumni(alumniModel);

                    TempData["SuccessMessage"] = "Alumni updated succesfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to Update due to " + ex.Message);

                TempData["ErrorMessage"] = ex.Message;

                StateAndFacultyDdl();
                RepopulateCascadeDdl(existingData);

                return View(existingData);
            }
        }

        // GET: Alumni/Delete/5

        // POST: Alumni/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var existingData = _alumniRepository.GetAlumni(id);
                var jobData = _jobRepository.GetAll(id);

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Alumni Not Found";

                    return RedirectToAction("Index");
                }

                //if(jobData.Count() >= 1)
                //{
                //    TempData["ErrorMessage"] = "Job history still exists cannot delete alumni, delete job history first";

                //    return Json(new
                //    {
                //        success = false,
                //        message = "Job history still exists cannot delete alumni, delete job history first"
                //    });
                //}

                _alumniRepository.DeleteAlumni(id);

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = true
                });
            }
        }

        public ActionResult ExportExcel()
        {


            var workBook = _excelRepository.AlumniExportExcel();

            var stream = new System.IO.MemoryStream();

            workBook.Save(stream, SaveFormat.Xlsx);

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AlumniData.xlsx");
        }

        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    _excelRepository.ImportAlumniFromExcel(file);
                }

                TempData["SuccessMessage"] = "Data imported Succesfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                ModelState.AddModelError("", "Unable to import due to " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        private void StateAndFacultyDdl()
        {
            var facultyDdl = _facultyRepository.GetAll();
            var statesDdl = _alumniRepository.GetAllStates();

            ViewBag.FacultyDdl = new SelectList(facultyDdl, "FacultyID", "FacultyName");
            ViewBag.StatesDdl = new SelectList(statesDdl, "StateID", "StateName");
        }

        public JsonResult ShowMajorList(int facultyId)
        {
            var ddlMajor = _majorRepository.GetMajorIdByFacultyId(facultyId)
                .Select(m => new SelectListItem
                {
                    Value = m.MajorID.ToString(),
                    Text = m.MajorName
                });

            return Json(ddlMajor, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShowDistrictList(int stateId)
        {
            var ddlDistrict = _alumniRepository.GetDistrictByStateId(stateId)
                .Select(m => new SelectListItem
                {
                    Value = m.DistrictID.ToString(),
                    Text = m.DistrictName
                });

            return Json(ddlDistrict, JsonRequestBehavior.AllowGet);
        }

        private void RepopulateCascadeDdl(AlumniModel alumniModel)
        {
            alumniModel.DistrictDDL = _alumniRepository.GetAllDistricts()
                .Where(d => d.StateID == alumniModel.StateID)
                .Select(d => new SelectListItem
                {
                    Value = d.DistrictID.ToString(),
                    Text = d.DistrictName,
                    Selected = d.StateID == alumniModel.StateID
                }).ToList();

            alumniModel.MajorDDl = _majorRepository.GetAll()
                .Where(m => m.FacultyID == alumniModel.FacultyID)
                .Select(m => new SelectListItem
                {
                    Value = m.MajorID.ToString(),
                    Text = m.MajorName,
                    Selected = m.FacultyID == alumniModel.FacultyID

                }).ToList();
        }
    }
}
