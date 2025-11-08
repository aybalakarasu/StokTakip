using StokTakip.Data.Modeller;

namespace StokTakip.Servis.Interfaces;

/// <summary>
/// Kategori işlemlerini yöneten servis arayüzü
/// </summary>
public interface IKategoriServisi
{
    /// <summary>
    /// Tüm kategorileri getirir
    /// </summary>
    Task<List<Kategori>> TumKategorileriGetirAsync();

    /// <summary>
    /// Belirli bir kategoriyi ID'ye göre getirir
    /// </summary>
    Task<Kategori?> KategoriGetirAsync(int id);

    /// <summary>
    /// Yeni kategori ekler
    /// </summary>
    Task<Kategori> KategoriEkleAsync(Kategori kategori);

    /// <summary>
    /// Mevcut kategoriyi günceller
    /// </summary>
    Task<bool> KategoriGuncelleAsync(Kategori kategori);

    /// <summary>
    /// Kategoriyi siler
    /// </summary>
    Task<bool> KategoriSilAsync(int id);

    /// <summary>
    /// Kategori var mı kontrol eder
    /// </summary>
    Task<bool> KategoriVarMiAsync(int id);
}

