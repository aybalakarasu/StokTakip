using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StokTakip.Data.Modeller;
using StokTakip.Servis.Interfaces;
using StokTakip.Web.ViewModels;

namespace StokTakip.Web.Controllers;

/// <summary>
/// Stok hareket işlemlerini yöneten controller
/// </summary>
public class StokHareketleriController : Controller
{
    private readonly IStokHareketServisi _stokHareketServisi;
    private readonly IUrunServisi _urunServisi;
    private readonly ILogger<StokHareketleriController> _logger;

    public StokHareketleriController(
        IStokHareketServisi stokHareketServisi,
        IUrunServisi urunServisi,
        ILogger<StokHareketleriController> logger)
    {
        _stokHareketServisi = stokHareketServisi;
        _urunServisi = urunServisi;
        _logger = logger;
    }

    /// <summary>
    /// Stok hareket listesi
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var hareketler = await _stokHareketServisi.TumHareketleriGetirAsync();
        var viewModels = hareketler.Select(h => new StokHareketViewModel
        {
            Id = h.Id,
            UrunId = h.UrunId,
            UrunAdi = h.Urun?.Ad ?? "",
            Adet = h.Adet,
            HareketTuru = h.HareketTuru,
            Tarih = h.Tarih
        }).ToList();

        return View(viewModels);
    }

    /// <summary>
    /// Stok hareket ekleme sayfası (GET)
    /// </summary>
    public async Task<IActionResult> Ekle()
    {
        var urunler = await _urunServisi.TumUrunleriGetirAsync();
        var urunViewModels = urunler.Select(u => new UrunViewModel
        {
            Id = u.Id,
            Ad = u.Ad,
            Stok = u.Stok,
            Fiyat = u.Fiyat,
            KategoriId = u.KategoriId,
            KategoriAdi = u.Kategori?.Ad ?? ""
        }).ToList();

        var viewModel = new StokHareketOlusturViewModel
        {
            Urunler = urunViewModels,
            Tarih = DateTime.Now
        };

        ViewBag.Urunler = new SelectList(urunler, "Id", "Ad");
        ViewBag.HareketTurleri = new SelectList(new[]
        {
            new { Value = HareketTuru.Giris, Text = "Giriş" },
            new { Value = HareketTuru.Cikis, Text = "Çıkış" }
        }, "Value", "Text");

        return View(viewModel);
    }

    /// <summary>
    /// Stok hareket ekleme işlemi (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(StokHareketOlusturViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var urunler = await _urunServisi.TumUrunleriGetirAsync();
            ViewBag.Urunler = new SelectList(urunler, "Id", "Ad", viewModel.UrunId);
            ViewBag.HareketTurleri = new SelectList(new[]
            {
                new { Value = HareketTuru.Giris, Text = "Giriş" },
                new { Value = HareketTuru.Cikis, Text = "Çıkış" }
            }, "Value", "Text", viewModel.HareketTuru);
            return View(viewModel);
        }

        try
        {
            var hareket = new StokHareketi
            {
                UrunId = viewModel.UrunId,
                Adet = viewModel.Adet,
                HareketTuru = viewModel.HareketTuru,
                Tarih = viewModel.Tarih ?? DateTime.Now
            };

            await _stokHareketServisi.HareketOlusturAsync(hareket);
            TempData["Basarili"] = "Stok hareketi başarıyla oluşturuldu ve ürün stok bilgisi güncellendi.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            TempData["Hata"] = ex.Message;
            var urunler = await _urunServisi.TumUrunleriGetirAsync();
            ViewBag.Urunler = new SelectList(urunler, "Id", "Ad", viewModel.UrunId);
            ViewBag.HareketTurleri = new SelectList(new[]
            {
                new { Value = HareketTuru.Giris, Text = "Giriş" },
                new { Value = HareketTuru.Cikis, Text = "Çıkış" }
            }, "Value", "Text", viewModel.HareketTuru);
            return View(viewModel);
        }
    }

    /// <summary>
    /// Belirli bir ürüne ait stok hareketlerini getirir
    /// </summary>
    public async Task<IActionResult> UruneGore(int urunId)
    {
        var hareketler = await _stokHareketServisi.UruneGoreHareketleriGetirAsync(urunId);
        var urun = await _urunServisi.UrunGetirAsync(urunId);

        var viewModels = hareketler.Select(h => new StokHareketViewModel
        {
            Id = h.Id,
            UrunId = h.UrunId,
            UrunAdi = h.Urun?.Ad ?? "",
            Adet = h.Adet,
            HareketTuru = h.HareketTuru,
            Tarih = h.Tarih
        }).ToList();

        ViewBag.UrunAdi = urun?.Ad ?? "Bilinmeyen Ürün";
        return View(viewModels);
    }
}

