using System.ComponentModel.DataAnnotations;
using StokTakip.Data.Modeller;

namespace StokTakip.API.DTOs;

/// <summary>
/// Stok hareket veri transfer nesnesi (DTO)
/// </summary>
public class StokHareketDTO
{
    /// <summary>
    /// Stok hareket kimliği
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Ürün kimliği
    /// </summary>
    public int UrunId { get; set; }

    /// <summary>
    /// Ürün adı (gösterim için)
    /// </summary>
    public string? UrunAdi { get; set; }

    /// <summary>
    /// Hareket adedi
    /// </summary>
    public int Adet { get; set; }

    /// <summary>
    /// Hareket türü (Giris veya Cikis)
    /// </summary>
    public HareketTuru HareketTuru { get; set; }

    /// <summary>
    /// Hareket türü açıklaması (gösterim için)
    /// </summary>
    public string HareketTuruAciklamasi => HareketTuru == HareketTuru.Giris ? "Giriş" : "Çıkış";

    /// <summary>
    /// Hareket tarihi
    /// </summary>
    public DateTime Tarih { get; set; }
}

/// <summary>
/// Stok hareket oluşturma için DTO
/// </summary>
public class StokHareketOlusturDTO
{
    /// <summary>
    /// Ürün kimliği
    /// </summary>
    [Required(ErrorMessage = "Ürün seçimi zorunludur.")]
    public int UrunId { get; set; }

    /// <summary>
    /// Hareket adedi
    /// </summary>
    [Required(ErrorMessage = "Adet zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Adet 1 veya daha büyük olmalıdır.")]
    public int Adet { get; set; }

    /// <summary>
    /// Hareket türü (1: Giriş, 2: Çıkış)
    /// </summary>
    [Required(ErrorMessage = "Hareket türü zorunludur.")]
    public HareketTuru HareketTuru { get; set; }

    /// <summary>
    /// Hareket tarihi (opsiyonel, belirtilmezse şu anki tarih kullanılır)
    /// </summary>
    public DateTime? Tarih { get; set; }
}

