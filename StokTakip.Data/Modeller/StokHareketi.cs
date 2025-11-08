using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StokTakip.Data.Modeller;

/// <summary>
/// Stok hareket türleri
/// </summary>
public enum HareketTuru
{
    /// <summary>
    /// Stok girişi
    /// </summary>
    Giris = 1,

    /// <summary>
    /// Stok çıkışı
    /// </summary>
    Cikis = 2
}

/// <summary>
/// Stok hareketlerini temsil eden model sınıfı
/// </summary>
public class StokHareketi
{
    /// <summary>
    /// Stok hareketi benzersiz kimliği
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Hareketin yapıldığı ürün kimliği
    /// </summary>
    [Required(ErrorMessage = "Ürün seçimi zorunludur.")]
    public int UrunId { get; set; }

    /// <summary>
    /// Hareketin yapıldığı ürün (navigation property)
    /// </summary>
    public virtual Urun Urun { get; set; } = null!;

    /// <summary>
    /// Hareket adedi
    /// </summary>
    [Required(ErrorMessage = "Adet zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Adet 1 veya daha büyük olmalıdır.")]
    public int Adet { get; set; }

    /// <summary>
    /// Hareket türü (Giriş veya Çıkış)
    /// </summary>
    [Required(ErrorMessage = "Hareket türü zorunludur.")]
    public HareketTuru HareketTuru { get; set; }

    /// <summary>
    /// Hareket tarihi
    /// </summary>
    [Required(ErrorMessage = "Tarih zorunludur.")]
    public DateTime Tarih { get; set; } = DateTime.Now;
}

