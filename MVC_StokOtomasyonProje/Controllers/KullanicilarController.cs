using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_StokOtomasyonProje.Models.Entity;
using System.Web.Security;
using System.Net.Mail;
using System.Net;

namespace MVC_StokTakipOtomasyonu.Controllers
{
    [AllowAnonymous] 
    public class KullanicilarController : Controller
    {

        MVC_StokProjeEntities ku = new MVC_StokProjeEntities();
        // GET: Kullanicilar
        [HttpGet]

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Kullanicilar k)
        {
            var kullanici = ku.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == k.KullaniciAdi && x.Sifre == k.Sifre);
            if (kullanici != null)
            {
                FormsAuthentication.SetAuthCookie(k.KullaniciAdi, false);
                return RedirectToAction("Index", "Urunler");
            }
            ViewBag.hata = "Kullanıcı adı veya Şifre yanlış!";
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ResetPassword(Kullanicilar k)
        {
            var model = ku.Kullanicilar.Where(x => x.Email == k.Email).FirstOrDefault();
            if (model != null)
            {
                Guid rastgele = Guid.NewGuid();
                model.Sifre = rastgele.ToString().Substring(0, 8);
                ku.SaveChanges();
                SmtpClient client = new SmtpClient("smtp.yandex.ru", 587);
                client.EnableSsl = true;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("testceyhungunsar1997@yandex.com", "Şifre sıfırlama");
                mail.To.Add(model.Email);
                mail.IsBodyHtml = true;
                mail.Subject = "Şifre değiştirme isteği";
                mail.Body += "Merhaba " + model.AdiSoyadi + "<br/> Kullanıcı Adınız: " + model.KullaniciAdi + "<br/> Şifreniz: " + model.Sifre;
                NetworkCredential net = new NetworkCredential("testasaf@yandex.com", "Asaf123");
                client.Credentials = net;
                client.Send(mail);
                return RedirectToAction("Login");

            }
            ViewBag.hata = "Böyle bir E-mail adresi bulunamadı.";
            return View();
        }

        [HttpGet]
        public ActionResult Kaydol()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Kaydol(Kullanicilar k)
        {
            if (!ModelState.IsValid) return View();
            ku.Entry(k).State = System.Data.Entity.EntityState.Added;
            ku.SaveChanges();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Guncelle()
        {
            if(User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = ku.Kullanicilar.FirstOrDefault(x=>x.KullaniciAdi==kullaniciadi);
                if(model != null)
                {
                    return View(model);
                }
                else
                {
                    return View(new Kullanicilar());
                }
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Guncelle(Kullanicilar k)
        {
            ku.Entry(k).State = System.Data.Entity.EntityState.Modified;
            ku.SaveChanges();
            FormsAuthentication.SignOut();
            //FormsAuthentication.SetAuthCookie(k.KullaniciAdi, false);
            return RedirectToAction("Login", "Kullanicilar");
        }
    }

}