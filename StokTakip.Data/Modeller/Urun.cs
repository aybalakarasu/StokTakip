using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StokTakip.Data.Modeller;

/// <summary>
/// Ürün bilgilerini temsil eden model sınıfı
/// </summary>
public class Urun
{
    /// <summary>
    /// Ürün benzersiz kimliği
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Ürün adı
    /// </summary>
    [Required(ErrorMessage = "Ürün adı zorunludur.")]
    [MinLength(2, ErrorMessage = "Ürün adı en az 2 karakter olmalıdır.")]
    [MaxLength(200, ErrorMessage = "Ürün adı en fazla 200 karakter olabilir.")]
    public string Ad { get; set; } = string.Empty;

    /// <summary>
    /// Ürün stok miktarı
    /// </summary>
    [Required(ErrorMessage = "Stok miktarı zorunludur.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya daha büyük olmalıdır.")]
    public int Stok { get; set; }

    /// <summary>
    /// Ürün fiyatı
    /// </summary>
    [Required(ErrorMessage = "Fiyat zorunludur.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0.01 veya daha büyük olmalıdır.")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Fiyat { get; set; }

    /// <summary>
    /// Ürünün ait olduğu kategori kimliği
    /// </summary>
    [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
    public int KategoriId { get; set; }

    /// <summary>
    /// Ürünün ait olduğu kategori (navigation property)
    /// </summary>
    public virtual Kategori Kategori { get; set; } = null!;

    /// <summary>
    /// Bu ürüne ait stok hareketleri
    /// </summary>
    public virtual ICollection<StokHareketi> StokHareketleri { get; set; } = new List<StokHareketi>();
}

