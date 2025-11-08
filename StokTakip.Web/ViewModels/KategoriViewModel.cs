using System.ComponentModel.DataAnnotations;
using StokTakip.Data.Modeller;

namespace StokTakip.Web.ViewModels;

/// <summary>
/// Kategori görüntüleme için ViewModel
/// </summary>
public class KategoriViewModel
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public int UrunSayisi { get; set; }
}

/// <summary>
/// Kategori oluşturma/düzenleme için ViewModel
/// </summary>
public class KategoriOlusturViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Kategori adı zorunludur.")]
    [MinLength(2, ErrorMessage = "Kategori adı en az 2 karakter olmalıdır.")]
    [MaxLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
    [Display(Name = "Kategori Adı")]
    public string Ad { get; set; } = string.Empty;
}

