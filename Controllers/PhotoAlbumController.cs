using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlumniManagement.Frontend.Controllers
{
    public class PhotoAlbumController : Controller
    {
        private IPhotoAlbumRepository _photoAlbumRepository;

        public PhotoAlbumController(): this(new PhotoAlbumRepository()) { }

        public PhotoAlbumController(IPhotoAlbumRepository photoAlbumRepository)
        {
            _photoAlbumRepository = photoAlbumRepository;
        }



        // GET: PhotoAlbum
        public ActionResult Index()
        {
            var data = _photoAlbumRepository.GetPhotoAlbums();



            return View(data);
        }

        // GET: PhotoAlbum/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PhotoAlbum/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhotoAlbum/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PhotoAlbum/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PhotoAlbum/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PhotoAlbum/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PhotoAlbum/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
