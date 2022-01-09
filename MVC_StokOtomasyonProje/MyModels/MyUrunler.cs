using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_StokOtomasyonProje.MyModels
{
    public class MyUrunler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MyUrunler()
        {
            //this.Satislar = new HashSet<Satislar>();
            //this.Sepet = new HashSet<Sepet>();
            this.MarkaListesi = new List<SelectListItem>();
            MarkaListesi.Insert(0, new SelectListItem { Text = "Önce kategori seçilmelidir", Value = "" });
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Kategori alanı boş geçilemez...")]
        public int KategoriID { get; set; }
        [Required(ErrorMessage = "Marka alanı boş geçilemez...")]
        public int MarkaID { get; set; }
        [Required(ErrorMessage = "Ürün alanı boş geçilemez...")]
        [Display(Name = "Ürün Adı")]
        public string UrunAdi { get; set; }
        [Required(ErrorMessage = "Barkod No alanı boş geçilemez...")]
        [Display(Name = "Barkod No")]
        public string BarkodNo { get; set; }
        [Required(ErrorMessage = "Alış Fiyatı alanı boş geçilemez...")]
        [Display(Name = "Alış Fiyatı")]
        public decimal? AlisFiyati { get; set; }
        [Required(ErrorMessage = "Satış Fiyatı alanı boş geçilemez...")]
        [Display(Name = "Satış Fiyatı")]
        public decimal? SatisFiyati { get; set; }
        [Required(ErrorMessage = "K.D.V. alanı boş geçilemez...")]
        [Range(0, 100, ErrorMessage = "0-100 arası rakam girilmelidir")]
        [Display(Name = "K.D.V.")]
        public int? KDV { get; set; }
        [Required(ErrorMessage = "Birim alanı boş geçilemez...")]
        public int BirimID { get; set; }
        [Required(ErrorMessage = "Tarih alanı boş geçilemez...")]
        public System.DateTime Tarih { get; set; }
        [Required(ErrorMessage = "Açıklama alanı boş geçilemez...")]
        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }
        [Required(ErrorMessage = "Miktar alanı boş geçilemez...")]
        [Display(Name = "Miktarı")]
        public decimal? Miktar { get; set; }

        public virtual MyBirimler Birimler { get; set; }
        public virtual MyKategoriler Kategoriler { get; set; }
        public virtual MyMarkalar Markalar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        // public virtual ICollection<Satislar> Satislar { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Sepet> Sepet { get; set; }

        public List<SelectListItem> KategoriListesi { get; set; }

        public List<SelectListItem> MarkaListesi { get; set; }

        public List<SelectListItem> BirimListesi { get; set; }

    }
}