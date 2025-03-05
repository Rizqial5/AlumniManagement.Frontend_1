using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlumniManagement.Frontend.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        private IJobRepository _jobRepository;
        private IAlumniRepository _alumniRepository;

        public JobController() : this(new JobRepository(), new AlumniRepository()) {  }
        public JobController(IJobRepository jobRepository, IAlumniRepository alumniRepository)
        {
            _jobRepository = jobRepository;
            _alumniRepository = alumniRepository;
        }


        // GET: Job
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

        public JsonResult GetJobs(int alumniId)
        {
            var jobData = _jobRepository.GetAll(alumniId);

            foreach (var item in jobData)
            {
                item.ShowStartDate = DateConverter(item.StartDate);
                item.ShowEndDate = DateConverter(item.EndDate);
            }

            var json = Json(new { data = jobData }, JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public string DateConverter(DateTime? dateTime)
        {
            if (dateTime == DateTime.MinValue || dateTime == null) { return "N/A"; }

            return dateTime.Value.ToString("dd-MMM-yyyy");
        }

        // GET: Job/Details/5


        // GET: Job/Create
        public ActionResult Create(int alumniId)
        {

            if (_alumniRepository.GetAlumni(alumniId) == null)
            {
                TempData["ErrorMessage"] = "Alumni Not Found";
                return RedirectToAction("Index", "Alumni");
            }
            ViewBag.AlumniId = alumniId;

            return PartialView("_CreatePartial");
        }

        // POST: Job/Create
        [HttpPost]
        public ActionResult Create(JobModel jobModel, int alumniId)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {

                    if(jobModel.StartDate > jobModel.EndDate)
                    {
                        ModelState.AddModelError("", "Error creating Job: Start date not supposed to be over than end date");
                        return View();
                    }

                    //if(jobModel.StartDate == null || jobModel.EndDate == null)
                    //{
                    //    jobModel.StartDate = DateTime.Empty()
                    //}

                    _jobRepository.InsertJob(jobModel, alumniId);

                    TempData["SuccessMessage"] = "Job created succesfully";
                }

                return RedirectToAction("Index", new { alumniId = alumniId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                ModelState.AddModelError("", "Error creating Job: " + ex.Message);
                return View();
            }
        }

        // GET: Job/Edit/5
        public ActionResult Edit(int id, int alumniId)
        {

            if (_alumniRepository.GetAlumni(alumniId) == null)
            {
                TempData["ErrorMessage"] = "Alumni Not Found";
                return RedirectToAction("Index", "Alumni");
            }

            var existingData = _jobRepository.GetJob(id,alumniId);

            ViewBag.AlumniId = alumniId;
            if (existingData == null)
            {
                TempData["ErrorMessage"] = "Job Not Found";

                return RedirectToAction("Index", new { alumniId = alumniId });
            }

            return PartialView("_EditPartial", existingData);
        }

        // POST: Job/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, JobModel jobModel, int alumniId)
        {
            var existingData = _jobRepository.GetJob(id, alumniId);
            try
            {
                // TODO: Add update logic here
                

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Job Not Found";

                    return RedirectToAction("Index", new { alumniId = alumniId });
                }

                if (ModelState.IsValid)
                {
                    if (jobModel.StartDate > jobModel.EndDate)
                    {
                        ModelState.AddModelError("", "Error creating Job: Start date not supposed to be over than end date");
                        return View(existingData);
                    }

                    _jobRepository.UpdateJob(jobModel, alumniId);
                    TempData["SuccessMessage"] = "Job Updated succesfully";
                }

                return RedirectToAction("Index", new { alumniId = alumniId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                ModelState.AddModelError("", "Unable to Update due to " + ex.Message);

                return View(existingData);
            }
        }

        // GET: Job/Delete/5

        // POST: Job/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, JobModel jobModel, int alumniId)
        {
            try
            {
                // TODO: Add delete logic here
                var existingData = _jobRepository.GetJob(id, alumniId);

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "Job Not Found";

                    return RedirectToAction("Index", new { alumniId = alumniId });
                }

                _jobRepository.DeleteJob(id, alumniId);

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to delete due to " + ex.Message);

                return Json(new
                {
                    error = true
                });
            }
        }
    }
}
