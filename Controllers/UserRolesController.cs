using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using AlumniManagement.Frontend.Repositories;
using AlumniManagement.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlumniManagement.Frontend.Controllers
{
    [Authorize(Roles = "Superadmin")]
    public class UserRolesController : Controller
    {
        private IUserManagementRepository _userManagementRepository;

        public UserRolesController() : this(new UserManagementRepository()) { }
        public UserRolesController(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        // GET: UserRoles
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetRoles()
        {
            var rolesData = _userManagementRepository.GetAllRoles();

            return Json(new {data = rolesData}, JsonRequestBehavior.AllowGet);
        }

        // GET: UserRoles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserRoles/Create
        public ActionResult Create()
        {
            var newRole = new AspNetUserModel.RoleModel();

            return PartialView("_CreateView", newRole);
        }

        // POST: UserRoles/Create
        [HttpPost]
        public ActionResult Create(AspNetUserModel.RoleModel roleModel)
        {
            try
            {
                // TODO: Add insert logic here
                if(ModelState.IsValid)
                {
                    _userManagementRepository.InsertRoles(roleModel);

                    TempData["SuccessMessage"] = "Role created succesfully";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = "Unable to Create role due to : " + ex.Message;

                return RedirectToAction("Index");
            }
        }


        // GET: UserRoles/Delete/5

        // POST: UserRoles/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var existingRoles = _userManagementRepository.GetRoleById(id);

                if(existingRoles == null)
                {
                    TempData["ErrorMessage"] = "Faculty not Found";

                    return Json(new
                    {
                        error = true,
                        message = "Role Not Found"
                    });
                }

                _userManagementRepository.DeleteRoles(id);


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
        public ActionResult DeleteSelected(string[] ids)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {
                    foreach (var item in ids)
                    {
                        _userManagementRepository.DeleteRoles(item);
                    }

                    return Json(new
                    {
                        success = true,
                        message = "Selected role have been deleted succesfully"
                    });

                }

                return Json(new
                {
                    error = true,
                    message = "no role selected for item deletion "
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
