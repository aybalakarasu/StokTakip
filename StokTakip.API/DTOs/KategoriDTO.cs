using System.ComponentModel.DataAnnotations;

namespace StokTakip.API.DTOs;

/// <summary>
/// Kategori veri transfer nesnesi (DTO)
/// </summary>
public class KategoriDTO
{
    /// <summary>
    /// Kategori kimliği
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
    /// Bu kategoriye ait ürün sayısı
    /// </summary>
    public int UrunSayisi { get; set; }
}

/// <summary>
/// Kategori oluşturma/güncelleme için DTO
/// </summary>
public class KategoriOlusturDTO
{
    /// <summary>
    /// Kategori adı
    /// </summary>
    [Required(ErrorMessage = "Kategori adı zorunludur.")]
    [MinLength(2, ErrorMessage = "Kategori adı en az 2 karakter olmalıdır.")]
    [MaxLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
    public string Ad { get; set; } = string.Empty;
}

