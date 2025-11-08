using Microsoft.EntityFrameworkCore;
using StokTakip.Data;
using StokTakip.Data.Modeller;
using StokTakip.Servis.Interfaces;

namespace StokTakip.Servis.Servisler;

/// <summary>
/// Kategori işlemlerini yöneten servis implementasyonu
/// </summary>
public class KategoriServisi : IKategoriServisi
{
    private readonly StokTakipContext _context;

    /// <summary>
    /// Kategori servisi constructor
    /// </summary>
    public KategoriServisi(StokTakipContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tüm kategorileri getirir
    /// </summary>
    public async Task<List<Kategori>> TumKategorileriGetirAsync()
    {
        return await _context.Kategoriler
            .OrderBy(k => k.Ad)
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir kategoriyi ID'ye göre getirir
    /// </summary>
    public async Task<Kategori?> KategoriGetirAsync(int id)
    {
        return await _context.Kategoriler
            .Include(k => k.Urunler)
            .FirstOrDefaultAsync(k => k.Id == id);
    }

    /// <summary>
    /// Yeni kategori ekler
    /// </summary>
    public async Task<Kategori> KategoriEkleAsync(Kategori kategori)
    {
        _context.Kategoriler.Add(kategori);
        await _context.SaveChangesAsync();
        return kategori;
    }

    /// <summary>
    /// Mevcut kategoriyi günceller
    /// </summary>
    public async Task<bool> KategoriGuncelleAsync(Kategori kategori)
    {
        var mevcutKategori = await _context.Kategoriler.FindAsync(kategori.Id);
        if (mevcutKategori == null)
            return false;

        mevcutKategori.Ad = kategori.Ad;
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Kategoriyi siler
    /// </summary>
    public async Task<bool> KategoriSilAsync(int id)
    {
        var kategori = await _context.Kategoriler
            .Include(k => k.Urunler)
            .FirstOrDefaultAsync(k => k.Id == id);

        if (kategori == null)
            return false;

        // Eğer kategoriye ait ürün varsa silme işlemi yapılmaz
        if (kategori.Urunler.Any())
            return false;

        _context.Kategoriler.Remove(kategori);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Kategori var mı kontrol eder
    /// </summary>
    public async Task<bool> KategoriVarMiAsync(int id)
    {
        return await _context.Kategoriler.AnyAsync(k => k.Id == id);
    }
}

