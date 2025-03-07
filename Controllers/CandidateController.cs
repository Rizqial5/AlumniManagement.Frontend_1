using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.JobHistoryService;
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
    public class CandidateController : Controller
    {
        private IJobPostingRepository _jobPostingRepository;

        public CandidateController() : this(new JobPostingRepository()) { }

        public CandidateController(IJobPostingRepository jobPostingRepository)
        {
            _jobPostingRepository = jobPostingRepository;
            
        }

        // GET: Candidate
        public ActionResult Index(Guid jobId)
        {
            try
            {


                var listSkills = _jobPostingRepository.GetSkillsbyId(jobId);


                ViewBag.JobId = jobId;
                ViewBag.JobTitle = _jobPostingRepository.GetJobPosting(jobId).Title;
                ViewBag.JobDesc = _jobPostingRepository.GetJobPosting(jobId).JobDescription;
                ViewBag.Exp = _jobPostingRepository.GetJobPosting(jobId).MinimumExperience;
                ViewBag.Skills = String.Join(",", listSkills.Select(s => s.Name));

                return View();
            }
            catch
            {
                TempData["ErrorMessage"] = "Error load candidates page";
                return RedirectToAction("Index", "JobPosting");
            }
        }

        public JsonResult GetCandidates(Guid jobId)
        {
            try
            {


                var candidatesData = _jobPostingRepository.GetAllCandidateBYJObId(jobId);

                foreach (var candidate in candidatesData)
                {
                    var listUrls = new List<ShowUrlModel>();

                    if (candidate.JobAttachments != null && candidate.JobAttachments.Any())
                    {
                        foreach (var item in candidate.JobAttachments)
                        {
                            var showUrlModel = new ShowUrlModel
                            {
                                Urls = Url.Content(item.FilePath.Replace("~", "") + "/" + item.FileName),
                                NameType = GetTypeName(item.AttachmentTypeID)

                            };

                            listUrls.Add(showUrlModel);
                        }

                    }

                    candidate.ListUrls = listUrls;
                }

                var json = Json(new
                {
                    error = false,
                    message = "Success",
                    data = candidatesData
                }, JsonRequestBehavior.AllowGet);

                json.MaxJsonLength = int.MaxValue;

                return json;
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = true,
                    message = "Error load candidates data",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public string GetTypeName(int attahcmentTypeId)
        {
            switch(attahcmentTypeId)
            {
                case 1:
                    return "CV File";
                case 2:
                    return "Identity Card File";
                case 3:
                    return "Certificate File";
                default:
                    return "";

            }
        }


    }
}
