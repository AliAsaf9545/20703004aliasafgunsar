using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokOtomasyonProje.Models.Entity;
using MVC_StokOtomasyonProje.MyModels;

namespace MVC_StokTakipOtomasyonu.Controllers
{
   [Authorize(Roles = "A,U2")] 
    public class MarkalarController : Controller
    {
        // GET: Markalar
        MVC_StokProjeEntities mr = new MVC_StokProjeEntities();
        public ActionResult Index()
        {
            var model = mr.Markalar.ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            SelecteBilgiGetir();
            return View();
        }

        private void SelecteBilgiGetir()
        {
            var model = new MyMarkalar();
            List<SelectListItem> liste = new List<SelectListItem>(from x in mr.Kategoriler
                                                                  select new SelectListItem
                                                                  {
                                                                      Value = x.ID.ToString(),
                                                                      Text = x.Kategori
                                                                  }
                                                                  ).ToList();
            ViewBag.l = liste;
        }

        [HttpPost]
        public ActionResult Ekle(Markalar p)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.KategoriID = new SelectList(mr.Kategoriler, "ID", "Kategori", p.KategoriID);

                return View();
            }

            mr.Entry(p).State = System.Data.Entity.EntityState.Added;
            mr.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult GuncelleBilgiGetir(int id)
        {
            MyMarkalar model = new MyMarkalar();
            SelecteBilgiGetir();
            var ara = mr.Markalar.Find(id);
            model.ID = ara.ID;
            model.KategoriID = ara.KategoriID;
            model.Marka = ara.Marka;
            model.Aciklama = ara.Aciklama;
            return View(model);
        }

        public ActionResult Guncelle(Markalar p)
        {
            if (!ModelState.IsValid)
            {
                SelecteBilgiGetir();
                return View("GuncelleBilgiGetir");
            }
            mr.Entry(p).State = System.Data.Entity.EntityState.Modified;
            mr.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SilBilgiGetir(Markalar p)
        {
            var getir = mr.Markalar.Find(p.ID);
            return View(getir);
        }

        public ActionResult Sil(Markalar p)
        {
            mr.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            mr.SaveChanges();
            return RedirectToAction("Index");
        }   
        public ActionResult Urunler(int id)
        {
            var model = mr.Urunler.Where(x => x.Markalar.ID == id).ToList();
            var kategori = mr.Kategoriler.Where(x => x.ID == id).Select(x => x.Kategori).FirstOrDefault();
            var marka = mr.Markalar.Where(x => x.ID == id).Select(x => x.Marka).FirstOrDefault();
            ViewBag.k = kategori;
            ViewBag.m = marka;
            return View(model);
        }
    }  
}

