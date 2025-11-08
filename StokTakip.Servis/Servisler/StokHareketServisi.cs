using Microsoft.EntityFrameworkCore;
using StokTakip.Data;
using StokTakip.Data.Modeller;
using StokTakip.Servis.Interfaces;

namespace StokTakip.Servis.Servisler;

/// <summary>
/// Stok hareket işlemlerini yöneten servis implementasyonu
/// </summary>
public class StokHareketServisi : IStokHareketServisi
{
    private readonly StokTakipContext _context;
    private readonly IUrunServisi _urunServisi;

    /// <summary>
    /// Stok hareket servisi constructor
    /// </summary>
    public StokHareketServisi(StokTakipContext context, IUrunServisi urunServisi)
    {
        _context = context;
        _urunServisi = urunServisi;
    }

    /// <summary>
    /// Tüm stok hareketlerini getirir (ürün bilgileri ile birlikte)
    /// </summary>
    public async Task<List<StokHareketi>> TumHareketleriGetirAsync()
    {
        return await _context.StokHareketleri
            .Include(s => s.Urun)
                .ThenInclude(u => u.Kategori)
            .OrderByDescending(s => s.Tarih)
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir stok hareketini ID'ye göre getirir
    /// </summary>
    public async Task<StokHareketi?> HareketGetirAsync(int id)
    {
        return await _context.StokHareketleri
            .Include(s => s.Urun)
                .ThenInclude(u => u.Kategori)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <summary>
    /// Yeni stok hareketi oluşturur ve ürün stok bilgisini otomatik günceller
    /// </summary>
    public async Task<StokHareketi> HareketOlusturAsync(StokHareketi hareket)
    {
        // Ürünü kontrol et
        var urun = await _urunServisi.UrunGetirAsync(hareket.UrunId);
        if (urun == null)
            throw new InvalidOperationException("Ürün bulunamadı.");

        // Çıkış işlemi için stok kontrolü
        if (hareket.HareketTuru == HareketTuru.Cikis && urun.Stok < hareket.Adet)
            throw new InvalidOperationException($"Yetersiz stok! Mevcut stok: {urun.Stok}, İstenen: {hareket.Adet}");

        // Stok hareketini ekle
        _context.StokHareketleri.Add(hareket);
        await _context.SaveChangesAsync();

        // Ürün stok bilgisini güncelle
        if (hareket.HareketTuru == HareketTuru.Giris)
        {
            urun.Stok += hareket.Adet;
        }
        else // Cikis
        {
            urun.Stok -= hareket.Adet;
        }

        await _context.SaveChangesAsync();

        // İlişkili verileri yükle
        await _context.Entry(hareket)
            .Reference(s => s.Urun)
            .LoadAsync();

        await _context.Entry(hareket.Urun)
            .Reference(u => u.Kategori)
            .LoadAsync();

        return hareket;
    }

    /// <summary>
    /// Belirli bir ürüne ait stok hareketlerini getirir
    /// </summary>
    public async Task<List<StokHareketi>> UruneGoreHareketleriGetirAsync(int urunId)
    {
        return await _context.StokHareketleri
            .Include(s => s.Urun)
                .ThenInclude(u => u.Kategori)
            .Where(s => s.UrunId == urunId)
            .OrderByDescending(s => s.Tarih)
            .ToListAsync();
    }

    /// <summary>
    /// Tarih aralığına göre stok hareketlerini getirir
    /// </summary>
    public async Task<List<StokHareketi>> TarihAraliginaGoreHareketleriGetirAsync(DateTime baslangic, DateTime bitis)
    {
        return await _context.StokHareketleri
            .Include(s => s.Urun)
                .ThenInclude(u => u.Kategori)
            .Where(s => s.Tarih >= baslangic && s.Tarih <= bitis)
            .OrderByDescending(s => s.Tarih)
            .ToListAsync();
    }
}

