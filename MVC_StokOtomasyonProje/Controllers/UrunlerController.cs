
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokOtomasyonProje.Models.Entity;
using MVC_StokOtomasyonProje.MyModels;
using Excel = Microsoft.Office.Interop.Excel;

namespace MVC_StokTakipOtomasyonu.Controllers
{
    [Authorize]
   // [ValidateAntiForgeryToken]
    public class UrunlerController : Controller
    {
        // GET: Urunler
        MVC_StokProjeEntities ur = new MVC_StokProjeEntities();
        public ActionResult Index(string ara)
        {
            var model = ur.Urunler.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                model = model.Where(x => x.UrunAdi.Contains(ara) || x.BarkodNo.Contains(ara)).ToList();
            }
            return View("Index", model);
        }

        public ActionResult Index2(string ara)
        {
            var model = ur.Urunler.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                model = model.Where(x => x.UrunAdi.Contains(ara) || x.BarkodNo.Contains(ara)).ToList();
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Ekle()
        {
            var model = new MyUrunler();
            Yenile(model);
            return View(model);
        }

        private void Yenile(MyUrunler model)
        {
            List<Kategoriler> kategorilist = ur.Kategoriler.OrderBy(x => x.Kategori).ToList();
            model.KategoriListesi = (from x in kategorilist
                                     select new SelectListItem
                                     {
                                         Text = x.Kategori,
                                         Value = x.ID.ToString()

                                     }).ToList();
            List<Birimler> birimlist = ur.Birimler.OrderBy(x => x.Birim).ToList();
            model.BirimListesi = (from x in birimlist
                                  select new SelectListItem
                                  {
                                      Text = x.Birim,
                                      Value = x.ID.ToString()

                                  }).ToList();
        }

        [HttpPost]
        public ActionResult Ekle(Urunler p)
        {

            if (!ModelState.IsValid)
            {
                var model = new MyUrunler();
                Yenile(model);
                return View(model);
            }
            ur.Entry(p).State = System.Data.Entity.EntityState.Added;
            ur.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]

        public ActionResult MiktarEkle(int id)
        {
            var model = ur.Urunler.Find(id);
            return View();
        }
        [HttpPost]
        public ActionResult MiktarEkle(Urunler p)
        {
            var model = ur.Urunler.Find(p.ID);
            model.Miktar = model.Miktar + p.Miktar;
            ur.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult GetMarka(int id2)
        {
            var model = new MyUrunler();
            List<Markalar> markaliste = ur.Markalar.Where(x => x.KategoriID == id2).OrderBy(x => x.Marka).ToList();
            model.MarkaListesi = (from x in markaliste
                                  select new SelectListItem
                                  {
                                      Text = x.Marka,
                                      Value = x.ID.ToString()
                                  }).ToList();
            // model.MarkaListesi.Insert(0, new SelectListItem { Text = "---Seçiniz---", Value = "" });
            return Json(model.MarkaListesi, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GuncelleBilgiGetir(int id)
        {
            var model = ur.Urunler.Find(id);
            var urun = new MyUrunler();
            urun.ID = model.ID;
            urun.KategoriID = model.KategoriID;
            urun.MarkaID = model.MarkaID;
            urun.UrunAdi = model.UrunAdi;
            urun.BarkodNo = model.BarkodNo;
            urun.AlisFiyati = model.AlisFiyati;
            urun.SatisFiyati = model.SatisFiyati;
            urun.Miktar = model.Miktar;
            urun.KDV = model.KDV;
            urun.BirimID = model.BirimID;
            urun.Tarih = model.Tarih;
            urun.Aciklama = model.Aciklama;
            Yenile(urun);
            List<Markalar> markalist = ur.Markalar.Where(x => x.KategoriID == model.KategoriID).OrderBy(x => x.Marka).ToList();
            urun.MarkaListesi = (from x in markalist
                                 select new SelectListItem
                                 {
                                     Text = x.Marka,
                                     Value = x.ID.ToString()
                                 }).ToList();
            return View(urun);
        }
        [HttpPost]
        public ActionResult Guncelle(Urunler p)
        {
            if (!ModelState.IsValid)
            {
                var model = ur.Urunler.Find(p.ID);
                var urun = new MyUrunler();
                Yenile(urun);
                List<Markalar> markalist = ur.Markalar.Where(x => x.KategoriID == model.KategoriID).OrderBy(x => x.Marka).ToList();
                urun.MarkaListesi = (from x in markalist
                                     select new SelectListItem
                                     {
                                         Text = x.Marka,
                                         Value = x.ID.ToString()
                                     }).ToList();
                return View(urun);
            }
            ur.Entry(p).State = System.Data.Entity.EntityState.Modified;
            ur.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult ExcelExport()
        {
            try
            {
                var liste = ur.Urunler.ToList();
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet worksheet = workbook.ActiveSheet;
                worksheet.Cells[1, 1] = "ID";
                worksheet.Cells[1, 2] = "Ürün Adı";
                worksheet.Cells[1, 3] = "Barkod No";
                worksheet.Cells[1, 4] = "Fiyatı";
                worksheet.Cells[1, 5] = "Miktarı";
                worksheet.Cells[1, 6] = "Tarih";
                int row = 2;
                foreach (var item in liste)
                {
                    worksheet.Cells[row, 1] = item.ID;
                    worksheet.Cells[row, 2] = item.UrunAdi;
                    worksheet.Cells[row, 3] = item.BarkodNo;
                    worksheet.Cells[row, 4] = item.SatisFiyati;
                    worksheet.Cells[row, 5] = item.Miktar;
                    worksheet.Cells[row, 6] = item.Tarih;

                    worksheet.Cells[row, 2].ColumnWidth = 15;
                    worksheet.Cells[row, 4].ColumnWidth = 15;
                    worksheet.Cells[row, 6].ColumnWidth = 15;
                    row++;
                }
                var heading = worksheet.get_Range("A1", "F1");
                heading.Font.Bold = true;
                heading.Font.Size = 13;
                heading.Font.Color = System.Drawing.Color.Red;
                var sum = ur.Urunler.Sum(x=>x.SatisFiyati).ToString("#,###,###.00");
                var range_sum = worksheet.get_Range("D" + row);
                range_sum.Value2 = "Total: " + sum;
                range_sum.Font.Bold = true;

                var range_Fiyat = worksheet.get_Range("D2","D"+row);
                range_Fiyat.NumberFormat = "#,###,###.00";

                var range_Tarih = worksheet.get_Range("F2", "F" + row);
                range_Tarih.NumberFormat = "dd.MMM.yyyy";

                workbook.SaveAs("d\\test.xlsx");
                workbook.Close();
                application.Quit();

                ViewBag.mesaj = "İşlem Başarılı...";
            }
            catch (Exception ex)
            {

                ViewBag.mesaj=ex.Message;
            }
            return Json(ViewBag.mesaj, JsonRequestBehavior.AllowGet);
        }
    }
}