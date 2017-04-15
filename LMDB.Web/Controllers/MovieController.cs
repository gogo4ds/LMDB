﻿using System;
using System.Linq;

namespace LMDB.Web.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Net;
    using System.Web.Mvc;
    using LMDB.Data;
    using LMDB.Models;
    using LMDB.ViewModels.Movie;
    using AutoMapper;
    using System.IO;
    using System.Web;

    public class MovieController : Controller
    {
        private MoviesContext db = new MoviesContext();

        // GET: Movie
        public ActionResult Index()
        {
            var movies = db.Movies.Include(m => m.Director).Include(m => m.Genres);
            var moviesViewModels = Mapper.Map<List<MovieIndexViewModel>>(movies);
            var genres = db.Genres.ToList();
            var model = Tuple.Create(moviesViewModels, genres);
            return View(model);
        }

        // GET: Movie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = db.Movies.Find(id);
            var movieDetails = Mapper.Map<MovieDetailsViewModel>(movie);

            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movieDetails);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                

                return RedirectToAction("Index");
            }

            //ViewBag.DirectorId = new SelectList(db.Directors, "Id", "FirstName", movie.DirectorId);
            //ViewBag.Id = new SelectList(db.Reviews, "ReviewedMovieId", "Content", movie.Id);
            return View(model);
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            ViewBag.DirectorId = new SelectList(db.Directors, "Id", "FirstName", movie.DirectorId);
            ViewBag.Id = new SelectList(db.Reviews, "ReviewedMovieId", "Content", movie.Id);
            return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,DateReleased,DirectorId")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DirectorId = new SelectList(db.Directors, "Id", "FirstName", movie.DirectorId);
            ViewBag.Id = new SelectList(db.Reviews, "ReviewedMovieId", "Content", movie.Id);
            return View(movie);
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private static byte[] GetBytesFromFile(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return null;
            }

            MemoryStream stream = new MemoryStream();
            file.InputStream.CopyTo(stream);
            byte[] data = stream.ToArray();
            return data;
        }
    }
}
