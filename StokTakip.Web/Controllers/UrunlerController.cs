using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StokTakip.Data.Modeller;
using StokTakip.Servis.Interfaces;
using StokTakip.Web.ViewModels;

namespace StokTakip.Web.Controllers;

/// <summary>
/// Ürün işlemlerini yöneten controller
/// </summary>
public class UrunlerController : Controller
{
    private readonly IUrunServisi _urunServisi;
    private readonly IKategoriServisi _kategoriServisi;
    private readonly ILogger<UrunlerController> _logger;

    public UrunlerController(
        IUrunServisi urunServisi,
        IKategoriServisi kategoriServisi,
        ILogger<UrunlerController> logger)
    {
        _urunServisi = urunServisi;
        _kategoriServisi = kategoriServisi;
        _logger = logger;
    }

    /// <summary>
    /// Ürün listesi
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var urunler = await _urunServisi.TumUrunleriGetirAsync();
        var viewModels = urunler.Select(u => new UrunViewModel
        {
            Id = u.Id,
            Ad = u.Ad,
            Stok = u.Stok,
            Fiyat = u.Fiyat,
            KategoriId = u.KategoriId,
            KategoriAdi = u.Kategori?.Ad ?? ""
        }).ToList();

        return View(viewModels);
    }

    /// <summary>
    /// Ürün detay sayfası
    /// </summary>
    public async Task<IActionResult> Detay(int id)
    {
        var urun = await _urunServisi.UrunGetirAsync(id);
        if (urun == null)
        {
            TempData["Hata"] = "Ürün bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = new UrunViewModel
        {
            Id = urun.Id,
            Ad = urun.Ad,
            Stok = urun.Stok,
            Fiyat = urun.Fiyat,
            KategoriId = urun.KategoriId,
            KategoriAdi = urun.Kategori?.Ad ?? ""
        };

        return View(viewModel);
    }

    /// <summary>
    /// Ürün ekleme sayfası (GET)
    /// </summary>
    public async Task<IActionResult> Ekle()
    {
        var kategoriler = await _kategoriServisi.TumKategorileriGetirAsync();
        var viewModel = new UrunOlusturViewModel
        {
            Kategoriler = kategoriler
        };

        ViewBag.Kategoriler = new SelectList(kategoriler, "Id", "Ad");
        return View(viewModel);
    }

    /// <summary>
    /// Ürün ekleme işlemi (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(UrunOlusturViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var kategoriler = await _kategoriServisi.TumKategorileriGetirAsync();
            ViewBag.Kategoriler = new SelectList(kategoriler, "Id", "Ad");
            return View(viewModel);
        }

        var urun = new Urun
        {
            Ad = viewModel.Ad,
            Stok = viewModel.Stok,
            Fiyat = viewModel.Fiyat,
            KategoriId = viewModel.KategoriId
        };

        await _urunServisi.UrunEkleAsync(urun);
        TempData["Basarili"] = "Ürün başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Ürün düzenleme sayfası (GET)
    /// </summary>
    public async Task<IActionResult> Duzenle(int id)
    {
        var urun = await _urunServisi.UrunGetirAsync(id);
        if (urun == null)
        {
            TempData["Hata"] = "Ürün bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        var kategoriler = await _kategoriServisi.TumKategorileriGetirAsync();
        var viewModel = new UrunOlusturViewModel
        {
            Id = urun.Id,
            Ad = urun.Ad,
            Stok = urun.Stok,
            Fiyat = urun.Fiyat,
            KategoriId = urun.KategoriId,
            Kategoriler = kategoriler
        };

        ViewBag.Kategoriler = new SelectList(kategoriler, "Id", "Ad", urun.KategoriId);
        return View(viewModel);
    }

    /// <summary>
    /// Ürün düzenleme işlemi (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(int id, UrunOlusturViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData["Hata"] = "Geçersiz ürün kimliği.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            var kategoriler = await _kategoriServisi.TumKategorileriGetirAsync();
            ViewBag.Kategoriler = new SelectList(kategoriler, "Id", "Ad", viewModel.KategoriId);
            return View(viewModel);
        }

        var urun = new Urun
        {
            Id = id,
            Ad = viewModel.Ad,
            Stok = viewModel.Stok,
            Fiyat = viewModel.Fiyat,
            KategoriId = viewModel.KategoriId
        };

        var guncellendi = await _urunServisi.UrunGuncelleAsync(urun);
        if (!guncellendi)
        {
            TempData["Hata"] = "Ürün güncellenemedi.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Basarili"] = "Ürün başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Ürün silme işlemi
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var silindi = await _urunServisi.UrunSilAsync(id);
        if (!silindi)
        {
            TempData["Hata"] = "Ürün silinemedi. Bu ürüne ait stok hareketi bulunmaktadır.";
        }
        else
        {
            TempData["Basarili"] = "Ürün başarıyla silindi.";
        }

        return RedirectToAction(nameof(Index));
    }
}

