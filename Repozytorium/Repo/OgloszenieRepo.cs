﻿using Repozytorium.IRepo;
using Repozytorium.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Repozytorium.Repo
{
    public class OgloszenieRepo: IOgloszenieRepo
    {
        private readonly IOglContext _db;
        public OgloszenieRepo(IOglContext db)
        {
            _db = db;
        }

        private OglContext db = new OglContext();

        public IQueryable<Ogloszenie> PobierzOgloszenia()
        {
            _db.Database.Log = message => Trace.WriteLine(message);
            //Optymalizacja zapytania SQL poprzez usuniecie Include (joinw w sql)
            //var ogloszenia = db.Ogloszenia.Include(o => o.Uzytkownik);
            return _db.Ogloszenia.AsNoTracking();
        }

        public Ogloszenie GetOgloszenieById(int id)
        {
            Ogloszenie ogloszenie = _db.Ogloszenia.Find(id);
            return ogloszenie;
        }

        public void UsunOgloszenie(int id)
        {
            UsunPowiazanieOgloszenieKategoria(id);
            Ogloszenie ogloszenie = _db.Ogloszenia.Find(id);
            _db.Ogloszenia.Remove(ogloszenie);
            //try
            //{
            //    _db.SaveChanges();
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        private void UsunPowiazanieOgloszenieKategoria(int idOgloszenia)
        {
            var list = _db.Ogloszenie_Kategoria.Where(o => o.OgloszenieId == idOgloszenia);
            foreach (var el in list)
            {
                _db.Ogloszenie_Kategoria.Remove(el);
            }
        }

        public void Dodaj(Ogloszenie ogloszenie)
        {
            _db.Ogloszenia.Add(ogloszenie);
        }

        public void Aktualizuj(Ogloszenie ogloszenie)
        {
            _db.Entry(ogloszenie).State = EntityState.Modified;
        }
    }
}