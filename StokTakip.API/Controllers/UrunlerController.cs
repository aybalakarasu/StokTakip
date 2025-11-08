using Microsoft.AspNetCore.Mvc;
using StokTakip.API.DTOs;
using StokTakip.Data.Modeller;
using StokTakip.Servis.Interfaces;

namespace StokTakip.API.Controllers;

/// <summary>
/// Ürün işlemlerini yöneten API controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UrunlerController : ControllerBase
{
    private readonly IUrunServisi _urunServisi;
    private readonly ILogger<UrunlerController> _logger;

    /// <summary>
    /// Ürünler controller constructor
    /// </summary>
    public UrunlerController(IUrunServisi urunServisi, ILogger<UrunlerController> logger)
    {
        _urunServisi = urunServisi;
        _logger = logger;
    }

    /// <summary>
    /// Tüm ürünleri getirir
    /// </summary>
    /// <returns>Ürün listesi</returns>
    /// <response code="200">Başarılı - Ürün listesi döner</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<UrunDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UrunDTO>>> TumUrunleriGetir()
    {
        try
        {
            var urunler = await _urunServisi.TumUrunleriGetirAsync();
            var urunDTOs = urunler.Select(u => new UrunDTO
            {
                Id = u.Id,
                Ad = u.Ad,
                Stok = u.Stok,
                Fiyat = u.Fiyat,
                KategoriId = u.KategoriId,
                KategoriAdi = u.Kategori?.Ad
            }).ToList();

            return Ok(urunDTOs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürünler getirilirken hata oluştu.");
            return StatusCode(500, new { Hata = "Ürünler getirilirken bir hata oluştu." });
        }
    }

    /// <summary>
    /// Belirli bir ürünü ID'ye göre getirir
    /// </summary>
    /// <param name="id">Ürün kimliği</param>
    /// <returns>Ürün bilgisi</returns>
    /// <response code="200">Başarılı - Ürün bilgisi döner</response>
    /// <response code="404">Ürün bulunamadı</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UrunDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UrunDTO>> UrunGetir(int id)
    {
        try
        {
            var urun = await _urunServisi.UrunGetirAsync(id);
            if (urun == null)
                return NotFound(new { Hata = $"ID'si {id} olan ürün bulunamadı." });

            var urunDTO = new UrunDTO
            {
                Id = urun.Id,
                Ad = urun.Ad,
                Stok = urun.Stok,
                Fiyat = urun.Fiyat,
                KategoriId = urun.KategoriId,
                KategoriAdi = urun.Kategori?.Ad
            };

            return Ok(urunDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün getirilirken hata oluştu. ID: {Id}", id);
            return StatusCode(500, new { Hata = "Ürün getirilirken bir hata oluştu." });
        }
    }

    /// <summary>
    /// Yeni ürün ekler
    /// </summary>
    /// <param name="urunDTO">Ürün bilgileri</param>
    /// <returns>Oluşturulan ürün</returns>
    /// <response code="201">Başarılı - Ürün oluşturuldu</response>
    /// <response code="400">Geçersiz veri</response>
    [HttpPost]
    [ProducesResponseType(typeof(UrunDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UrunDTO>> UrunEkle([FromBody] UrunOlusturDTO urunDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var urun = new Urun
            {
                Ad = urunDTO.Ad,
                Stok = urunDTO.Stok,
                Fiyat = urunDTO.Fiyat,
                KategoriId = urunDTO.KategoriId
            };

            var olusturulanUrun = await _urunServisi.UrunEkleAsync(urun);
            await _urunServisi.UrunGetirAsync(olusturulanUrun.Id); // Kategori bilgisini yükle

            var responseDTO = new UrunDTO
            {
                Id = olusturulanUrun.Id,
                Ad = olusturulanUrun.Ad,
                Stok = olusturulanUrun.Stok,
                Fiyat = olusturulanUrun.Fiyat,
                KategoriId = olusturulanUrun.KategoriId
            };

            return CreatedAtAction(nameof(UrunGetir), new { id = olusturulanUrun.Id }, responseDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün eklenirken hata oluştu.");
            return StatusCode(500, new { Hata = "Ürün eklenirken bir hata oluştu." });
        }
    }

    /// <summary>
    /// Mevcut ürünü günceller
    /// </summary>
    /// <param name="id">Ürün kimliği</param>
    /// <param name="urunDTO">Güncellenecek ürün bilgileri</param>
    /// <returns>Güncellenmiş ürün</returns>
    /// <response code="200">Başarılı - Ürün güncellendi</response>
    /// <response code="400">Geçersiz veri</response>
    /// <response code="404">Ürün bulunamadı</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UrunDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UrunDTO>> UrunGuncelle(int id, [FromBody] UrunOlusturDTO urunDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mevcutUrun = await _urunServisi.UrunGetirAsync(id);
            if (mevcutUrun == null)
                return NotFound(new { Hata = $"ID'si {id} olan ürün bulunamadı." });

            var urun = new Urun
            {
                Id = id,
                Ad = urunDTO.Ad,
                Stok = urunDTO.Stok,
                Fiyat = urunDTO.Fiyat,
                KategoriId = urunDTO.KategoriId
            };

            var guncellendi = await _urunServisi.UrunGuncelleAsync(urun);
            if (!guncellendi)
                return NotFound(new { Hata = $"ID'si {id} olan ürün güncellenemedi." });

            var guncellenmisUrun = await _urunServisi.UrunGetirAsync(id);
            var responseDTO = new UrunDTO
            {
                Id = guncellenmisUrun!.Id,
                Ad = guncellenmisUrun.Ad,
                Stok = guncellenmisUrun.Stok,
                Fiyat = guncellenmisUrun.Fiyat,
                KategoriId = guncellenmisUrun.KategoriId,
                KategoriAdi = guncellenmisUrun.Kategori?.Ad
            };

            return Ok(responseDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün güncellenirken hata oluştu. ID: {Id}", id);
            return StatusCode(500, new { Hata = "Ürün güncellenirken bir hata oluştu." });
        }
    }

    /// <summary>
    /// Ürünü siler
    /// </summary>
    /// <param name="id">Ürün kimliği</param>
    /// <returns>Silme işlemi sonucu</returns>
    /// <response code="200">Başarılı - Ürün silindi</response>
    /// <response code="404">Ürün bulunamadı</response>
    /// <response code="400">Ürün silinemez (stok hareketi var)</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UrunSil(int id)
    {
        try
        {
            var silindi = await _urunServisi.UrunSilAsync(id);
            if (!silindi)
            {
                var urunVarMi = await _urunServisi.UrunVarMiAsync(id);
                if (!urunVarMi)
                    return NotFound(new { Hata = $"ID'si {id} olan ürün bulunamadı." });
                
                return BadRequest(new { Hata = "Bu ürüne ait stok hareketi bulunduğu için silinemez." });
            }

            return Ok(new { Mesaj = "Ürün başarıyla silindi." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün silinirken hata oluştu. ID: {Id}", id);
            return StatusCode(500, new { Hata = "Ürün silinirken bir hata oluştu." });
        }
    }
}

