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
    public class BirimlerController : Controller
    {
        MVC_StokProjeEntities br = new MVC_StokProjeEntities();
        // GET: Birimler
        public ActionResult Index()
        {
            var degerler = br.Birimler.ToList();
            return View(degerler);
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            return View("Kaydet");
        }

        [HttpPost]
        public ActionResult Kaydet(Birimler p)
        {
            if (p.ID == 0)
            {
                if (p.Birim == null || p.Aciklama == null)
                {
                    return View();
                }
                br.Entry(p).State = System.Data.Entity.EntityState.Added;
            }
            else if (p.ID > 0)
            {
                if (p.Birim == null || p.Aciklama == null)
                {
                    return View();
                }
                br.Entry(p).State = System.Data.Entity.EntityState.Modified;
            }
            br.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GuncelleBilgiGetir(int id)
        {
            var model = br.Birimler.Find(id);
            MyBirimler b = new MyBirimler();
            b.ID = model.ID;
            b.Birim = model.Birim;
            b.Aciklama = model.Aciklama;
            if (model == null) return HttpNotFound();
            return View("Kaydet", b);
        }

        public ActionResult SilBilgiGetir(Birimler p)
        {
            var model = br.Birimler.Find(p.ID);
            if (model == null) return HttpNotFound();
            return View(model);
        }

        public ActionResult Sil(Birimler p)
        {
            br.Entry(p).State = System.Data.Entity.EntityState.Deleted;
            br.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
