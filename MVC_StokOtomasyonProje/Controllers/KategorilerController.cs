using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokOtomasyonProje.Models.Entity;
using MVC_StokOtomasyonProje.MyModels;

namespace MVC_StokOtomasyonProje.Controllers
{
   [Authorize(Roles = "A,U")]
 //  [ValidateAntiForgeryToken]
    public class KategorilerController : Controller
    {
        MVC_StokProjeEntities db = new MVC_StokProjeEntities();
        // GET: Kategoriler
        public ActionResult Index()
        {
            var degerler = db.Kategoriler.ToList();
            return View(degerler);
        }
        public ActionResult Ekle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Ekle2(Kategoriler p)
        {
            if (ModelState.IsValid == false) return View("Ekle");
            db.Kategoriler.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GuncelleBilgiGetir(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var model = db.Kategoriler.Find(id);
            MyKategoriler k = new MyKategoriler();
            k.ID = model.ID;
            k.Kategori = model.Kategori;
            k.Aciklama = model.Aciklama;
            if (model == null) return HttpNotFound();// ID si olmayan verileri NotFound Sayfasınna Yönlndirme işlemi
            return View(k);
        }
        public ActionResult Guncelle(Kategoriler p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SilBilgiGetir(Kategoriler p)
        {
            var model = db.Kategoriler.Find(p.ID);
           if (model == null) return HttpNotFound();
            return View(model);
        }

        public ActionResult Sil(Kategoriler p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        } 
        public ActionResult Markalar(int id)
        {
            var model = db.Markalar.Where(x => x.Kategoriler.ID == id).ToList();
            var kategori = db.Kategoriler.Where(x => x.ID == id).Select(x => x.Kategori).FirstOrDefault();
            ViewBag.viewkategori = kategori;
            return View(model);
        }
        public ActionResult Urunler(int id)
        {
            var model = db.Urunler.Where(x => x.Kategoriler.ID == id).ToList();
            var kategori = db.Kategoriler.Where(x => x.ID == id).Select(x => x.Kategori).FirstOrDefault();
            ViewBag.viewkategori = kategori;
            return View(model);
        }
    }
}
