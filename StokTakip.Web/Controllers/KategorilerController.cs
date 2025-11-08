using Microsoft.AspNetCore.Mvc;
using StokTakip.Data.Modeller;
using StokTakip.Servis.Interfaces;
using StokTakip.Web.ViewModels;

namespace StokTakip.Web.Controllers;

/// <summary>
/// Kategori işlemlerini yöneten controller
/// </summary>
public class KategorilerController : Controller
{
    private readonly IKategoriServisi _kategoriServisi;
    private readonly IUrunServisi _urunServisi;
    private readonly ILogger<KategorilerController> _logger;

    public KategorilerController(
        IKategoriServisi kategoriServisi,
        IUrunServisi urunServisi,
        ILogger<KategorilerController> logger)
    {
        _kategoriServisi = kategoriServisi;
        _urunServisi = urunServisi;
        _logger = logger;
    }

    /// <summary>
    /// Kategori listesi
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var kategoriler = await _kategoriServisi.TumKategorileriGetirAsync();
        var urunler = await _urunServisi.TumUrunleriGetirAsync();

        var viewModels = kategoriler.Select(k => new KategoriViewModel
        {
            Id = k.Id,
            Ad = k.Ad,
            UrunSayisi = urunler.Count(u => u.KategoriId == k.Id)
        }).ToList();

        return View(viewModels);
    }

    /// <summary>
    /// Kategori ekleme sayfası (GET)
    /// </summary>
    public IActionResult Ekle()
    {
        return View(new KategoriOlusturViewModel());
    }

    /// <summary>
    /// Kategori ekleme işlemi (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(KategoriOlusturViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var kategori = new Kategori
        {
            Ad = viewModel.Ad
        };

        await _kategoriServisi.KategoriEkleAsync(kategori);
        TempData["Basarili"] = "Kategori başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Kategori düzenleme sayfası (GET)
    /// </summary>
    public async Task<IActionResult> Duzenle(int id)
    {
        var kategori = await _kategoriServisi.KategoriGetirAsync(id);
        if (kategori == null)
        {
            TempData["Hata"] = "Kategori bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = new KategoriOlusturViewModel
        {
            Id = kategori.Id,
            Ad = kategori.Ad
        };

        return View(viewModel);
    }

    /// <summary>
    /// Kategori düzenleme işlemi (POST)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(int id, KategoriOlusturViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData["Hata"] = "Geçersiz kategori kimliği.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var kategori = new Kategori
        {
            Id = id,
            Ad = viewModel.Ad
        };

        var guncellendi = await _kategoriServisi.KategoriGuncelleAsync(kategori);
        if (!guncellendi)
        {
            TempData["Hata"] = "Kategori güncellenemedi.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Basarili"] = "Kategori başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Kategori silme işlemi
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var silindi = await _kategoriServisi.KategoriSilAsync(id);
        if (!silindi)
        {
            TempData["Hata"] = "Kategori silinemedi. Bu kategoriye ait ürün bulunmaktadır.";
        }
        else
        {
            TempData["Basarili"] = "Kategori başarıyla silindi.";
        }

        return RedirectToAction(nameof(Index));
    }
}

