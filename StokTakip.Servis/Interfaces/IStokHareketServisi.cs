using StokTakip.Data.Modeller;

namespace StokTakip.Servis.Interfaces;

/// <summary>
/// Stok hareket işlemlerini yöneten servis arayüzü
/// </summary>
public interface IStokHareketServisi
{
    /// <summary>
    /// Tüm stok hareketlerini getirir (ürün bilgileri ile birlikte)
    /// </summary>
    Task<List<StokHareketi>> TumHareketleriGetirAsync();

    /// <summary>
    /// Belirli bir stok hareketini ID'ye göre getirir
    /// </summary>
    Task<StokHareketi?> HareketGetirAsync(int id);

    /// <summary>
    /// Yeni stok hareketi oluşturur ve ürün stok bilgisini otomatik günceller
    /// </summary>
    Task<StokHareketi> HareketOlusturAsync(StokHareketi hareket);

    /// <summary>
    /// Belirli bir ürüne ait stok hareketlerini getirir
    /// </summary>
    Task<List<StokHareketi>> UruneGoreHareketleriGetirAsync(int urunId);

    /// <summary>
    /// Tarih aralığına göre stok hareketlerini getirir
    /// </summary>
    Task<List<StokHareketi>> TarihAraliginaGoreHareketleriGetirAsync(DateTime baslangic, DateTime bitis);
}

