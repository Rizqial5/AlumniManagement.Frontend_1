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
    [Authorize(Roles = "Superadmin")]
    public class UserManagementController : Controller
    {
        private IUserManagementRepository _userManagementRepository;

        public UserManagementController() : this(new UserManagementRepository()) { }
        public UserManagementController(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        #region User Region

        // GET: UserManagement
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetUsers()
        {
            var usersData = _userManagementRepository.GetAllUsers();

            foreach (var item in usersData)
            {
                item.ShowRoles = string.Join(", ", item.UserRoles.Select(ur => ur.Role.Name));
            }

            return Json(new {data = usersData}, JsonRequestBehavior.AllowGet);
        }


        // GET: UserManagement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

       

        // GET: UserManagement/Edit/5
        public ActionResult Edit(string id)
        {
            var existingData = _userManagementRepository.GetUser(id);

            if (existingData == null)
            {
                TempData["ErrorMessage"] = "User Not Found";

                return RedirectToAction("Index");
            }



            existingData.ListRoles = existingData.UserRoles.Select(p => p.RoleId);

            existingData.RolesDDL = _userManagementRepository.GetAllRoles()
                .Select(r => new SelectListItem
                {
                    Value = r.Id,
                    Text = r.Name,
                    Selected = existingData.ListRoles != null
                    && existingData.ListRoles.Contains(r.Id)
                });


            return PartialView("_EditPartial", existingData);
        }

        // POST: UserManagement/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, AspNetUserModel.UserModel userModel)
        {
            try
            {
                // TODO: Add update logic here

                var existingData = _userManagementRepository.GetUser(id);

                if (existingData == null)
                {
                    TempData["ErrorMessage"] = "User Not Found";

                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {


                    if (existingData.FullName.ToLower() != userModel.FullName.ToLower())
                    {
                        _userManagementRepository.UpdateUserFullName(id, userModel.FullName);
                    }

                    _userManagementRepository.UpdateUserRoles(id, userModel.ListRoles.ToList());

                    TempData["SuccessMessage"] = "User updated succesfully";
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


        // POST: UserManagement/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                _userManagementRepository.DeleteUser(id);

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

        #endregion
    }
}
