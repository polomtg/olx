﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Repozytorium.IRepo;
using Repozytorium.Models;
using Repozytorium.Repo;

namespace OGL.Controllers
{
    public class OgloszenieController : Controller
    {
        //Wstrzykiwanie repozytorium
        //Poprzez konstruktor i prywatne pole _repo
        private readonly IOgloszenieRepo _repo;
        public OgloszenieController(IOgloszenieRepo repo)
        {
            _repo = repo;
        }

        // GET: Ogloszenie
        public ActionResult Index()
        {
            var ogloszenia = _repo.PobierzOgloszenia();
            //Usuniecie .ToList() powoduje pobranie danych kiedy sa potrzebne
            //a nie kiedy jest wywoływane .ToList()
            //return View(ogloszenia.ToList());
            return View(ogloszenia);
        }

        // GET: Ogloszenie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);
            if (ogloszenie == null)
            {
                return HttpNotFound();
            }
            return View(ogloszenie);
        }



        // GET: Ogloszenie/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ogloszenie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tresc,Tytul")] Ogloszenie ogloszenie)
        {
            if (ModelState.IsValid)
            {
                ogloszenie.UzytkownikId = User.Identity.GetUserId();
                ogloszenie.DataDodania = DateTime.Now;
                try
                {
                    _repo.Dodaj(ogloszenie);
                    _repo.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View(ogloszenie);
                }
            }
            return View(ogloszenie);
        }

        // GET: Ogloszenie/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);
            if (ogloszenie == null)
            {
                return HttpNotFound();
            }
            return View(ogloszenie);
        }

        // POST: Ogloszenie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Tresc,Tytul,DataDodania,UzytkownikId")] Ogloszenie ogloszenie)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ogloszenie.UzytkownikId="asfdasdf";
                    _repo.Aktualizuj(ogloszenie);
                    _repo.SaveChanges();
                }
                catch (Exception)
                {
                    ViewBag.Blad = true;
                    return View(ogloszenie);
                }
            }
            ViewBag.Blad = false;
            return View(ogloszenie);
        }        

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //} s

        // GET: Ogloszenie/Delete/5
        public ActionResult Delete(int? id, bool? blad)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);
            if (ogloszenie == null)
            {
                return HttpNotFound();
            }
            if (blad!=null)
            {
                ViewBag.Blad = true;
            }
            return View(ogloszenie);
        }

        // POST: Ogloszenie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.UsunOgloszenie(id);
            try
            {
                _repo.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new { id = id, blad = true });
            }
            return RedirectToAction("Index");
        }

    }
}
