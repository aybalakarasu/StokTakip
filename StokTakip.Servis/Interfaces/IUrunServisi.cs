using StokTakip.Data.Modeller;

namespace StokTakip.Servis.Interfaces;

/// <summary>
/// Ürün işlemlerini yöneten servis arayüzü
/// </summary>
public interface IUrunServisi
{
    /// <summary>
    /// Tüm ürünleri getirir (kategorileri ile birlikte)
    /// </summary>
    Task<List<Urun>> TumUrunleriGetirAsync();

    /// <summary>
    /// Belirli bir ürünü ID'ye göre getirir
    /// </summary>
    Task<Urun?> UrunGetirAsync(int id);

    /// <summary>
    /// Yeni ürün ekler
    /// </summary>
    Task<Urun> UrunEkleAsync(Urun urun);

    /// <summary>
    /// Mevcut ürünü günceller
    /// </summary>
    Task<bool> UrunGuncelleAsync(Urun urun);

    /// <summary>
    /// Ürünü siler
    /// </summary>
    Task<bool> UrunSilAsync(int id);

    /// <summary>
    /// Ürün var mı kontrol eder
    /// </summary>
    Task<bool> UrunVarMiAsync(int id);

    /// <summary>
    /// Kategoriye göre ürünleri getirir
    /// </summary>
    Task<List<Urun>> KategoriyeGoreUrunleriGetirAsync(int kategoriId);
}

