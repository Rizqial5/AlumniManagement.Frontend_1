using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.PostingJobService;
using AlumniManagement.Frontend.Repositories;
using AlumniManagement.Web.Repositories;
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
    public class JobPostingController : Controller
    {
        private IJobPostingRepository _jobPostingRepository;
        private IAlumniRepository _alumniRepository;

        private string uploadPath = ConfigurationManager.AppSettings["AttachmentPath"];
        public JobPostingController() : this(new JobPostingRepository(), new AlumniRepository()) { }

        public JobPostingController(IJobPostingRepository jobPostingRepository, IAlumniRepository alumniRepository)
        {
            _jobPostingRepository = jobPostingRepository;
            _alumniRepository = alumniRepository;
        }

        // GET: JobPosting
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetJobPostings()
        {
            var jobPostingData = _jobPostingRepository.GetJobPostings();


            return Json(new { data = jobPostingData }, JsonRequestBehavior.AllowGet);
        }



        // GET: JobPosting/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: JobPosting/Create
        public ActionResult Create()
        {
            InitiateDDL();

            var jobPostingModel = new JobPostingModel();

            jobPostingModel.IsClosed = false;

            return PartialView("_CreatePartial", jobPostingModel);
        }

        private void InitiateDDL()
        {
            var skillsDDL = _jobPostingRepository.GetSkills();
            var employeeTypesDDl = _jobPostingRepository.GetEmploymentTypes();


            ViewBag.EmploymentTypeDDL = new SelectList(employeeTypesDDl, "EmploymentTypeID", "Name");
            ViewBag.SkillDDL = new MultiSelectList(skillsDDL, "SkillID", "Name");
            ViewBag.AttachmentDDL = _jobPostingRepository.GetAttachmentTypes();
        }

        // POST: JobPosting/Create
        [HttpPost]
        public ActionResult Create(JobPostingModel jobPostingModel)
        {
            try
            {
                // TODO: Add insert logic here

                if (ModelState.IsValid)
                {
                    _jobPostingRepository.InsertJobPosting(jobPostingModel);


                    TempData["SuccessMessage"] = "Job Posted";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = ex.Message;
                ModelState.AddModelError("", "Unable to Add due to " + ex.Message);

                return RedirectToAction("Index");
            }
        }

        public ActionResult ApplyJob(Guid id)
        {
            var job = _jobPostingRepository.GetJobPosting(id);

            if(job.IsClosed)
            {
                TempData["ErrorMessage"] = "Job is Already Closed";
                return RedirectToAction("Index");
            }


            var listAlumnisId  = _jobPostingRepository.GetAllCandidateBYJObId(id).Select(c=> c.AlumniID).ToList();

            var alumnisDdl = _alumniRepository.GetAll()
                      .Where(a => !listAlumnisId.Contains(a.AlumniID))
                      .ToList();


            var selectedCheckbox = new List<AttachmentTypeModel>();

            var existingAttachmenct = _jobPostingRepository.GetJobPosting(id);

            foreach (var item in existingAttachmenct.SelectedAttachmentTypes)
            {
                var selectBox = _jobPostingRepository.GetAttachmentTypes()
                    .FirstOrDefault(at => at.AttachmentTypeID == item);

                selectedCheckbox.Add(selectBox);
            }

            var availableTypes = selectedCheckbox;

            ViewBag.AlumniDDL = new SelectList(alumnisDdl, "AlumniID", "FullName");
            ViewBag.AvailableTypes = availableTypes;
            ViewBag.JobId = id;

            var listSkills = _jobPostingRepository.GetSkillsbyId(id);



            ViewBag.JobTitle = _jobPostingRepository.GetJobPosting(id).Title;
            ViewBag.JobDesc = _jobPostingRepository.GetJobPosting(id).JobDescription;
            ViewBag.Exp = _jobPostingRepository.GetJobPosting(id).MinimumExperience;
            ViewBag.Skills = String.Join(",", listSkills.Select(s => s.Name));

            var newAttahcmentModel = new JobAttachmentModel();

            newAttahcmentModel.JobID = id;
        

            return PartialView("_ApplyPartial", newAttahcmentModel);
        }

        [HttpPost]
        public ActionResult ApplyJob(JobAttachmentModel[] jobAttachments, HttpPostedFileBase[] fileUpload, int alumniId)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var checkClosed = _jobPostingRepository.GetJobPosting(jobAttachments[0].JobID).IsClosed;

                    if(checkClosed)
                    {
                        TempData["ErrorMessage"] = "Error: job is closed";
                        return RedirectToAction("Index");
                    }

                    var listJobAttachments = new List<JobAttachmentModel>();

                    for (int i = 0; i < fileUpload.Length; i++)
                    {
                        if (fileUpload[i] != null && fileUpload[i].ContentLength > 0)
                        {
                            string[] notAllowedExtensions = { ".exe", ".dll" };
                            string fileExtension = Path.GetExtension(fileUpload[i].FileName).ToLower();

                            if (notAllowedExtensions.Contains(fileExtension))
                            {
                                TempData["ErrorMessage"] = "Invalid file type. EXE and DLL are not allowed.";
                                return RedirectToAction("Index");
                            }

                            string fileName = Guid.NewGuid().ToString() + fileExtension;
                            string filePath = Path.Combine(Server.MapPath(uploadPath), fileName);
                            fileUpload[i].SaveAs(filePath);

                            // Ambil attachment model terkait jika ada
                            var attachment = new JobAttachmentModel();

                            // Simpan ke database
                            //attachment.JobID = jobId;
                            attachment.FilePath = uploadPath;
                            attachment.FileName = fileName;
                            attachment.AlumniID = alumniId;
                            attachment.JobID = jobAttachments[i].JobID;
                            attachment.AttachmentTypeID = jobAttachments[i].AttachmentTypeID;

                           listJobAttachments.Add(attachment);

                        }
                    }

                    _jobPostingRepository.InsertApplyJob(listJobAttachments, alumniId, listJobAttachments[0].JobID);
                    

                    TempData["SuccessMessage"] = "Job Applied Successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Model validation failed.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to upload files: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: JobPosting/Edit/5
        public ActionResult Edit(Guid id)
        {
            var existingData = _jobPostingRepository.GetJobPosting(id);

            if (existingData == null)
            {
                TempData["ErrorMessage"] = "Alumni Not Found";

                return RedirectToAction("Index");
            }

            InitiateDDL();

            existingData.EmployemenTypeDDL = _jobPostingRepository.GetEmploymentTypes()
                .Select(et => new SelectListItem
                {
                    Value = et.EmploymentTypeID.ToString(),
                    Text = et.Name,
                    Selected = et.EmploymentTypeID == existingData.EmploymentTypeID 
                });

            existingData.SkillDDL = _jobPostingRepository.GetSkills()
                .Select(s => new SelectListItem
                {
                    Value = s.SkillID.ToString(),
                    Text = s.Name,
                    Selected = existingData.SelectedSkills != null && existingData.SelectedSkills.Contains(s.SkillID)
                });

            var selectedCheckbox = new List<AttachmentTypeModel>();

            foreach (var item in existingData.SelectedAttachmentTypes)
            {
                var selectBox = _jobPostingRepository.GetAttachmentTypes()
                    .FirstOrDefault(at => at.AttachmentTypeID == item);

                selectedCheckbox.Add(selectBox);
            }

            existingData.AttachMentCheckBox = selectedCheckbox;

            

            return PartialView("_EditPartial", existingData);
        }

        // POST: JobPosting/Edit/5
        [HttpPost]
        public ActionResult Edit(Guid id, JobPostingModel jobPostingModel)
        {


            var existingData = _jobPostingRepository.GetJobPosting(id);
            try
            {
                // TODO: Add update logic here

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "JOb posting Not Found";

                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {

                    _jobPostingRepository.UpdateJobPosting(jobPostingModel);


                    TempData["SuccessMessage"] = "Job Posting updated succesfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to Update due to " + ex.Message);

                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index"); 
            }
        }


        // POST: JobPosting/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var existingData = _jobPostingRepository.GetJobPosting(id);


                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Job Posting Not Found";

                    return RedirectToAction("Index");
                }

                _jobPostingRepository.DeletingJobPosting(id);

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
        public ActionResult UpsertJobPosting(JobPostingModel jobPostingModel)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _jobPostingRepository.UpsertJobPosting(jobPostingModel);
                }

                TempData["SuccessMessage"] = "Job posting inserted and updated Succesfully";

                return RedirectToAction("Index");
            }
            catch ( Exception ex )
            {

                TempData["ErrorMessage"] = "Job Posintg updated Failed " + ex.Message;

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult DeleteSelected(Guid[] ids)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {
                    foreach (var item in ids)
                    {
                        _jobPostingRepository.DeletingJobPosting(item);
                    }

                    return Json(new
                    {
                        success = true,
                        message = "Selected job posting has been deleted succesfully"
                    });

                }

                return Json(new
                {
                    error = true,
                    message = "no job posting selected"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = true,
                    message = "Error deleting job posting " + ex.Message
                });
            }
        }
    }
}
