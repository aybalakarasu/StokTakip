using System.ComponentModel.DataAnnotations;

namespace StokTakip.Data.Modeller;

/// <summary>
/// Ürün kategorilerini temsil eden model sınıfı
/// </summary>
public class Kategori
{
    /// <summary>
    /// Kategori benzersiz kimliği
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Kategori adı
    /// </summary>
    [Required(ErrorMessage = "Kategori adı zorunludur.")]
    [MinLength(2, ErrorMessage = "Kategori adı en az 2 karakter olmalıdır.")]
    [MaxLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
    public string Ad { get; set; } = string.Empty;

    /// <summary>
    /// Bu kategoriye ait ürünler
    /// </summary>
    public virtual ICollection<Urun> Urunler { get; set; } = new List<Urun>();
}

