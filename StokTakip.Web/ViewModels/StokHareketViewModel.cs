using System.ComponentModel.DataAnnotations;
using StokTakip.Data.Modeller;

namespace StokTakip.Web.ViewModels;

/// <summary>
/// Stok hareket görüntüleme için ViewModel
/// </summary>
public class StokHareketViewModel
{
    public int Id { get; set; }
    public int UrunId { get; set; }
    public string UrunAdi { get; set; } = string.Empty;
    public int Adet { get; set; }
    public HareketTuru HareketTuru { get; set; }
    public string HareketTuruAciklamasi => HareketTuru == HareketTuru.Giris ? "Giriş" : "Çıkış";
    public DateTime Tarih { get; set; }
}

/// <summary>
/// Stok hareket oluşturma için ViewModel
/// </summary>
public class StokHareketOlusturViewModel
{
    [Required(ErrorMessage = "Ürün seçimi zorunludur.")]
    [Display(Name = "Ürün")]
    public int UrunId { get; set; }

    [Required(ErrorMessage = "Adet zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Adet 1 veya daha büyük olmalıdır.")]
    [Display(Name = "Adet")]
    public int Adet { get; set; }

    [Required(ErrorMessage = "Hareket türü zorunludur.")]
    [Display(Name = "Hareket Türü")]
    public HareketTuru HareketTuru { get; set; }

    [Display(Name = "Tarih")]
    public DateTime? Tarih { get; set; }

    public List<UrunViewModel>? Urunler { get; set; }
}

