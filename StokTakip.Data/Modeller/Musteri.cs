using System.ComponentModel.DataAnnotations;

namespace StokTakip.Data.Modeller;

/// <summary>
/// Müşteri bilgilerini temsil eden model sınıfı
/// </summary>
public class Musteri
{
    /// <summary>
    /// Müşteri benzersiz kimliği
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Müşteri adı ve soyadı
    /// </summary>
    [Required(ErrorMessage = "Ad soyad zorunludur.")]
    [MinLength(3, ErrorMessage = "Ad soyad en az 3 karakter olmalıdır.")]
    [MaxLength(200, ErrorMessage = "Ad soyad en fazla 200 karakter olabilir.")]
    public string AdSoyad { get; set; } = string.Empty;
}

