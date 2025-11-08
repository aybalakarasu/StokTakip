using System.ComponentModel.DataAnnotations;

namespace StokTakip.API.DTOs;

/// <summary>
/// Ürün veri transfer nesnesi (DTO)
/// </summary>
public class UrunDTO
{
    /// <summary>
    /// Ürün kimliği
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
    /// Stok miktarı
    /// </summary>
    [Required(ErrorMessage = "Stok miktarı zorunludur.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya daha büyük olmalıdır.")]
    public int Stok { get; set; }

    /// <summary>
    /// Ürün fiyatı
    /// </summary>
    [Required(ErrorMessage = "Fiyat zorunludur.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0.01 veya daha büyük olmalıdır.")]
    public decimal Fiyat { get; set; }

    /// <summary>
    /// Kategori kimliği
    /// </summary>
    [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
    public int KategoriId { get; set; }

    /// <summary>
    /// Kategori adı (gösterim için)
    /// </summary>
    public string? KategoriAdi { get; set; }
}

/// <summary>
/// Ürün oluşturma/güncelleme için DTO
/// </summary>
public class UrunOlusturDTO
{
    /// <summary>
    /// Ürün adı
    /// </summary>
    [Required(ErrorMessage = "Ürün adı zorunludur.")]
    [MinLength(2, ErrorMessage = "Ürün adı en az 2 karakter olmalıdır.")]
    [MaxLength(200, ErrorMessage = "Ürün adı en fazla 200 karakter olabilir.")]
    public string Ad { get; set; } = string.Empty;

    /// <summary>
    /// Stok miktarı
    /// </summary>
    [Required(ErrorMessage = "Stok miktarı zorunludur.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya daha büyük olmalıdır.")]
    public int Stok { get; set; }

    /// <summary>
    /// Ürün fiyatı
    /// </summary>
    [Required(ErrorMessage = "Fiyat zorunludur.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0.01 veya daha büyük olmalıdır.")]
    public decimal Fiyat { get; set; }

    /// <summary>
    /// Kategori kimliği
    /// </summary>
    [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
    public int KategoriId { get; set; }
}

