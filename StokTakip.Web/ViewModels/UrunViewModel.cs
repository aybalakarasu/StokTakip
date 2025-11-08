using System.ComponentModel.DataAnnotations;
using StokTakip.Data.Modeller;

namespace StokTakip.Web.ViewModels;

/// <summary>
/// Ürün görüntüleme için ViewModel
/// </summary>
public class UrunViewModel
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public int Stok { get; set; }
    public decimal Fiyat { get; set; }
    public int KategoriId { get; set; }
    public string KategoriAdi { get; set; } = string.Empty;
}

/// <summary>
/// Ürün oluşturma/düzenleme için ViewModel
/// </summary>
public class UrunOlusturViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Ürün adı zorunludur.")]
    [MinLength(2, ErrorMessage = "Ürün adı en az 2 karakter olmalıdır.")]
    [MaxLength(200, ErrorMessage = "Ürün adı en fazla 200 karakter olabilir.")]
    [Display(Name = "Ürün Adı")]
    public string Ad { get; set; } = string.Empty;

    [Required(ErrorMessage = "Stok miktarı zorunludur.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya daha büyük olmalıdır.")]
    [Display(Name = "Stok Miktarı")]
    public int Stok { get; set; }

    [Required(ErrorMessage = "Fiyat zorunludur.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0.01 veya daha büyük olmalıdır.")]
    [Display(Name = "Fiyat")]
    public decimal Fiyat { get; set; }

    [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
    [Display(Name = "Kategori")]
    public int KategoriId { get; set; }

    public List<Kategori>? Kategoriler { get; set; }
}

