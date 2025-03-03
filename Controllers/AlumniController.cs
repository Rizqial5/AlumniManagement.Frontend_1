using AlumniManagement.Frontend.AlumniService;
using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.Repositories;
using AlumniManagement.Web.Repositories;
using Aspose.Cells;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace AlumniManagement.Frontend.Controllers
{
    [Authorize]
    public class AlumniController : Controller
    {

        private IAlumniRepository _alumniRepository;
        private IFacultyRepository _facultyRepository;
        private IMajorRepository _majorRepository;
        private IExcelRepository _excelRepository;
        private IJobRepository _jobRepository;


        private string photoPath = ConfigurationManager.AppSettings["PhotoPath"];
        private string fileTypes = ConfigurationManager.AppSettings["FileTypes"];
        private int fileSizeLimit = Convert.ToInt32(ConfigurationManager.AppSettings["PhotoSizeLimit"]);

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
            var facultyDdl = _facultyRepository.GetAll();
            var majorDdl = _majorRepository.GetAll();

            ViewBag.FacultyDdl = new SelectList(facultyDdl, "FacultyID", "FacultyName");
            ViewBag.MajorDDL = new SelectList(majorDdl, "MajorID", "MajorName");
            ViewBag.SuperAdmin = false;

            if (User.IsInRole("Superadmin"))
            {
                ViewBag.SuperAdmin = true;
            }

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

        public JsonResult GetAlumnis(int? facultyId, int? majorId)
        {
            var alumniesData = _alumniRepository.GetAll();

            if (facultyId.HasValue && facultyId.Value > 0)
            {
                alumniesData = alumniesData.Where(a => a.FacultyID == facultyId.Value).ToList();
            }

            if (majorId.HasValue && majorId.Value > 0)
            {
                alumniesData = alumniesData.Where(a => a.MajorID == majorId.Value).ToList();
            }

            foreach (var item in alumniesData)
            {
                item.ShowDateOfBirth = DateConverter(item.DateOfBirth);
                if(item.PhotoPath != null)
                {
                    item.ShowImagePath = @Url.Content(item.PhotoPath.Replace("~", "") + '/' + item.PhotoName);
                }
                
            }

            return Json(new { data = alumniesData }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMajorsByFaculty(int facultyId)
        {
            var majors = _majorRepository.GetAll()
                .Where(m => m.FacultyID == facultyId)
                .Select(m => new
                {
                    m.MajorID,
                    m.MajorName
                }).ToList();

            return Json(majors, JsonRequestBehavior.AllowGet);
        }


        //multi select selected
        //public JsonResult GetAlumni(int id)
        //{
        //    var existingData = _alumniRepository.GetAlumni(id);

        //    existingData.HobbiesDDl = _alumniRepository.GetAllHobbies()
        //       .Select(h => new SelectListItem
        //       {
        //           Value = h.HobbyID.ToString(),
        //           Text = h.Name,
        //           Selected = existingData.Hobbies != null && existingData.Hobbies.Contains(h.HobbyID)
        //       }).ToList();

        //    return Json(existingData, JsonRequestBehavior.AllowGet);
        //}

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

        [Authorize(Roles = "Superadmin")]
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
            //alumniModel.HobbiesDDl = new MultiSelectList()

            StateAndFacultyDdl();

            return View(alumniModel);
        }

        // POST: Alumni/Create
        [HttpPost]
        [Authorize(Roles = "Superadmin")]
        public ActionResult Create(AlumniModel alumniModel, HttpPostedFileBase photoUpload)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {

                    if (photoUpload != null && photoUpload.ContentLength > 0)
                    {
                        UploadBehaviour(alumniModel, photoUpload);
                    }

                    //Check if there any hobbies

                    if (alumniModel.Hobbies!= null)
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

        private void UploadBehaviour(AlumniModel alumniModel, HttpPostedFileBase photoUpload)
        {
            // Validasi tipe file
            if(photoUpload == null)
            {
                return;
            }


            if (alumniModel.PhotoPath!= null)
            {
                var fileExist = Path.Combine(Server.MapPath(alumniModel.PhotoPath), alumniModel.PhotoName);

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
                PopulateData(alumniModel);
            }

            // Validasi ukuran file (3MB)
            if (photoUpload.ContentLength > fileSizeLimit)
            {
                ModelState.AddModelError("", "File size exceeds 3MB. Please select a smaller file.");
                StateAndFacultyDdl();
                RepopulateCascadeDdl(alumniModel);
                PopulateData(alumniModel);
            }

            // Simpan file ke server
            string fileName = Guid.NewGuid().ToString() + fileExtension;
            string filePath = Path.Combine(Server.MapPath(photoPath), fileName);
            photoUpload.SaveAs(filePath);

            // Simpan path ke model
            alumniModel.PhotoPath = photoPath;
            alumniModel.PhotoName = fileName;
        }

        private ActionResult PopulateData(AlumniModel alumniModel)
        {
            StateAndFacultyDdl();
            RepopulateCascadeDdl(alumniModel);
            return View("Index",alumniModel);
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

            ViewBag.SourceImage = "";
            ViewBag.NameFile = "";

            if (existingData.PhotoPath!= null)
            {
                ViewBag.SourceImage = @Url.Content(populateForm.PhotoPath.Replace("~", "") + '/' + populateForm.PhotoName);
                ViewBag.NameFile = populateForm.PhotoName;
            }

           


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

            alumniModel.HobbiesDDl = _alumniRepository.GetAllHobbies()
              .Select(h => new SelectListItem
              {
                  Value = h.HobbyID.ToString(),
                  Text = h.Name,
                  Selected = alumniModel.Hobbies != null && alumniModel.Hobbies.Contains(h.HobbyID)
              });


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

                    if (alumniModel.Hobbies != null)
                    {
                        _alumniRepository.UpdateAlumniWithHobbies(alumniModel);
                    }
                    else
                    {
                        _alumniRepository.UpdateAlumni(alumniModel);
                    }
                    

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
             

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Alumni Not Found";

                    return RedirectToAction("Index");
                }

                if(existingData.PhotoName != null)
                {
                    var filePath = Path.Combine(Server.MapPath(photoPath), existingData.PhotoName);

                    if (filePath != null)
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }

                

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
                if (ids != null && ids.Length > 0)
                {
                    foreach (var item in ids)
                    {
                        _alumniRepository.DeleteAlumni(item); // template yang dirubah
                    }

                    return Json(new
                    {
                        success = true,
                        message = "Selected Alumni have been deleted succesfully" // message dirubah sesuai objek
                    });

                }

                return Json(new
                {
                    error = true,
                    message = "no alumni selected "  // message dirubah sesuai objek
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = true,
                    message = "Error deleting alumni:  " + ex.Message  // message dirubah sesuai objek
                });
            }
        }

        [Authorize(Roles = "Superadmin")]
        public ActionResult ExportExcel()
        {


            var workBook = _excelRepository.AlumniExportExcel();

            var stream = new System.IO.MemoryStream();

            workBook.Save(stream, SaveFormat.Xlsx);

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AlumniData.xlsx");
        }

        // Get Show Tabel
        [HttpPost]
        [Authorize(Roles = "Superadmin")]
        public ActionResult ShowTableView(HttpPostedFileBase file)
        {
            var resultListImported = ImportExcel(file);

         


            ViewBag.ValidData = resultListImported.Where(a => a.ErrorDetails.Count() == 0).Count();
            ViewBag.ErrorData = resultListImported.Where(a => a.ErrorDetails.Count() > 0).Count();
            ViewBag.RecordData = resultListImported.Count();
            ViewBag.CheckIsError = false;
            var totalError = resultListImported.Where(a => a.ErrorDetails.Count() > 0).Count();

            if (totalError > 0)
            {
                ViewBag.CheckIsError = true;
            }

            ViewBag.AllData = resultListImported;

            return View("TabelSummaryView", resultListImported);


        }

        [HttpPost]
        public JsonResult SubmitImportedTable(FormCollection form)
        {
            try
            {
                string jsonString = form["submittedData"]; // Ambil data sebagai string
                var submittedData = JsonConvert.DeserializeObject<List<AlumniModel>>(jsonString);

                if (submittedData == null || !submittedData.Any())
                {
                    return Json(new { success = false, message = "No data received!" }, JsonRequestBehavior.AllowGet);
                }

                _alumniRepository.UpsertMultipleAlumni(submittedData);

                return Json(new { success = true, message = "Alumni updated successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Alumni insert failed: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Superadmin")]
        [HttpPost]
        public List<AlumniModel> ImportExcel(HttpPostedFileBase file)
        {
     
            var resultList = new List<AlumniModel>();

            if (file != null && file.ContentLength > 0)
            {
                resultList = _excelRepository.ImportAlumniFromExcel(file);
            }
            return resultList;
            

        }

        [Authorize(Roles = "Superadmin")]
        [HttpPost]
        public ActionResult UpsertAlumni(AlumniModel alumni, HttpPostedFileBase photoUpload)
        {
            try
            {
                if(ModelState.IsValid)
                {

                    UploadBehaviour(alumni, photoUpload);

                    _alumniRepository.UpsertAlumni(alumni);
                }

                TempData["SuccessMessage"] = "Alumni updated Succesfully";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = "Alumni updated Failed " + ex.Message;

                return RedirectToAction("Index");
            }
        }

        private void StateAndFacultyDdl()
        {
            var facultyDdl = _facultyRepository.GetAll();
            var statesDdl = _alumniRepository.GetAllStates();
            var hobbiesDdl = _alumniRepository.GetAllHobbies();

            ViewBag.FacultyDdl = new SelectList(facultyDdl, "FacultyID", "FacultyName");
            ViewBag.StatesDdl = new SelectList(statesDdl, "StateID", "StateName");
            ViewBag.HobbiesDdl = new MultiSelectList(hobbiesDdl, "HobbyID", "Name");
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
