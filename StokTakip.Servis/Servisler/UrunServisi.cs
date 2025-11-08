using Microsoft.EntityFrameworkCore;
using StokTakip.Data;
using StokTakip.Data.Modeller;
using StokTakip.Servis.Interfaces;

namespace StokTakip.Servis.Servisler;

/// <summary>
/// Ürün işlemlerini yöneten servis implementasyonu
/// </summary>
public class UrunServisi : IUrunServisi
{
    private readonly StokTakipContext _context;

    /// <summary>
    /// Ürün servisi constructor
    /// </summary>
    public UrunServisi(StokTakipContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tüm ürünleri getirir (kategorileri ile birlikte)
    /// </summary>
    public async Task<List<Urun>> TumUrunleriGetirAsync()
    {
        return await _context.Urunler
            .Include(u => u.Kategori)
            .OrderBy(u => u.Ad)
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir ürünü ID'ye göre getirir
    /// </summary>
    public async Task<Urun?> UrunGetirAsync(int id)
    {
        return await _context.Urunler
            .Include(u => u.Kategori)
            .Include(u => u.StokHareketleri)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <summary>
    /// Yeni ürün ekler
    /// </summary>
    public async Task<Urun> UrunEkleAsync(Urun urun)
    {
        _context.Urunler.Add(urun);
        await _context.SaveChangesAsync();
        return urun;
    }

    /// <summary>
    /// Mevcut ürünü günceller
    /// </summary>
    public async Task<bool> UrunGuncelleAsync(Urun urun)
    {
        var mevcutUrun = await _context.Urunler.FindAsync(urun.Id);
        if (mevcutUrun == null)
            return false;

        mevcutUrun.Ad = urun.Ad;
        mevcutUrun.Stok = urun.Stok;
        mevcutUrun.Fiyat = urun.Fiyat;
        mevcutUrun.KategoriId = urun.KategoriId;
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Ürünü siler
    /// </summary>
    public async Task<bool> UrunSilAsync(int id)
    {
        var urun = await _context.Urunler
            .Include(u => u.StokHareketleri)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (urun == null)
            return false;

        // Eğer ürüne ait stok hareketi varsa silme işlemi yapılmaz
        if (urun.StokHareketleri.Any())
            return false;

        _context.Urunler.Remove(urun);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Ürün var mı kontrol eder
    /// </summary>
    public async Task<bool> UrunVarMiAsync(int id)
    {
        return await _context.Urunler.AnyAsync(u => u.Id == id);
    }

    /// <summary>
    /// Kategoriye göre ürünleri getirir
    /// </summary>
    public async Task<List<Urun>> KategoriyeGoreUrunleriGetirAsync(int kategoriId)
    {
        return await _context.Urunler
            .Include(u => u.Kategori)
            .Where(u => u.KategoriId == kategoriId)
            .OrderBy(u => u.Ad)
            .ToListAsync();
    }
}

