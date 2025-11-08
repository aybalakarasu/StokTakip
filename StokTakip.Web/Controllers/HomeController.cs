using Microsoft.AspNetCore.Mvc;
using StokTakip.Servis.Interfaces;
using StokTakip.Web.ViewModels;

namespace StokTakip.Web.Controllers;

/// <summary>
/// Ana sayfa controller'ı
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUrunServisi _urunServisi;
    private readonly IStokHareketServisi _stokHareketServisi;

    public HomeController(
        ILogger<HomeController> logger,
        IUrunServisi urunServisi,
        IStokHareketServisi stokHareketServisi)
    {
        _logger = logger;
        _urunServisi = urunServisi;
        _stokHareketServisi = stokHareketServisi;
    }

    /// <summary>
    /// Ana sayfa - Ürün listesi ve son stok hareketleri
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var urunler = await _urunServisi.TumUrunleriGetirAsync();
        var hareketler = await _stokHareketServisi.TumHareketleriGetirAsync();

        var urunViewModels = urunler.Select(u => new UrunViewModel
        {
            Id = u.Id,
            Ad = u.Ad,
            Stok = u.Stok,
            Fiyat = u.Fiyat,
            KategoriId = u.KategoriId,
            KategoriAdi = u.Kategori?.Ad ?? ""
        }).ToList();

        var hareketViewModels = hareketler.Take(10).Select(h => new StokHareketViewModel
        {
            Id = h.Id,
            UrunId = h.UrunId,
            UrunAdi = h.Urun?.Ad ?? "",
            Adet = h.Adet,
            HareketTuru = h.HareketTuru,
            Tarih = h.Tarih
        }).ToList();

        ViewBag.Urunler = urunViewModels;
        ViewBag.SonHareketler = hareketViewModels;

        return View();
    }

    /// <summary>
    /// Hata sayfası
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Hata()
    {
        return View();
    }
}
