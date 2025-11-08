using Microsoft.AspNetCore.Mvc;
using StokTakip.API.DTOs;
using StokTakip.Data.Modeller;
using StokTakip.Servis.Interfaces;

namespace StokTakip.API.Controllers;

/// <summary>
/// Stok hareket işlemlerini yöneten API controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class StokController : ControllerBase
{
    private readonly IStokHareketServisi _stokHareketServisi;
    private readonly ILogger<StokController> _logger;

    /// <summary>
    /// Stok controller constructor
    /// </summary>
    public StokController(IStokHareketServisi stokHareketServisi, ILogger<StokController> logger)
    {
        _stokHareketServisi = stokHareketServisi;
        _logger = logger;
    }

    /// <summary>
    /// Tüm stok hareketlerini getirir
    /// </summary>
    /// <returns>Stok hareket listesi</returns>
    /// <response code="200">Başarılı - Stok hareket listesi döner</response>
    [HttpGet("hareketler")]
    [ProducesResponseType(typeof(List<StokHareketDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<StokHareketDTO>>> TumHareketleriGetir()
    {
        try
        {
            var hareketler = await _stokHareketServisi.TumHareketleriGetirAsync();
            var hareketDTOs = hareketler.Select(h => new StokHareketDTO
            {
                Id = h.Id,
                UrunId = h.UrunId,
                UrunAdi = h.Urun?.Ad,
                Adet = h.Adet,
                HareketTuru = h.HareketTuru,
                Tarih = h.Tarih
            }).ToList();

            return Ok(hareketDTOs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok hareketleri getirilirken hata oluştu.");
            return StatusCode(500, new { Hata = "Stok hareketleri getirilirken bir hata oluştu." });
        }
    }

    /// <summary>
    /// Belirli bir stok hareketini ID'ye göre getirir
    /// </summary>
    /// <param name="id">Stok hareket kimliği</param>
    /// <returns>Stok hareket bilgisi</returns>
    /// <response code="200">Başarılı - Stok hareket bilgisi döner</response>
    /// <response code="404">Stok hareket bulunamadı</response>
    [HttpGet("hareketler/{id}")]
    [ProducesResponseType(typeof(StokHareketDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StokHareketDTO>> HareketGetir(int id)
    {
        try
        {
            var hareket = await _stokHareketServisi.HareketGetirAsync(id);
            if (hareket == null)
                return NotFound(new { Hata = $"ID'si {id} olan stok hareket bulunamadı." });

            var hareketDTO = new StokHareketDTO
            {
                Id = hareket.Id,
                UrunId = hareket.UrunId,
                UrunAdi = hareket.Urun?.Ad,
                Adet = hareket.Adet,
                HareketTuru = hareket.HareketTuru,
                Tarih = hareket.Tarih
            };

            return Ok(hareketDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok hareket getirilirken hata oluştu. ID: {Id}", id);
            return StatusCode(500, new { Hata = "Stok hareket getirilirken bir hata oluştu." });
        }
    }

    /// <summary>
    /// Yeni stok hareketi oluşturur ve ürün stok bilgisini otomatik günceller
    /// </summary>
    /// <param name="hareketDTO">Stok hareket bilgileri</param>
    /// <returns>Oluşturulan stok hareket</returns>
    /// <response code="201">Başarılı - Stok hareket oluşturuldu</response>
    /// <response code="400">Geçersiz veri veya yetersiz stok</response>
    [HttpPost("hareket")]
    [ProducesResponseType(typeof(StokHareketDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StokHareketDTO>> HareketOlustur([FromBody] StokHareketOlusturDTO hareketDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hareket = new StokHareketi
            {
                UrunId = hareketDTO.UrunId,
                Adet = hareketDTO.Adet,
                HareketTuru = hareketDTO.HareketTuru,
                Tarih = hareketDTO.Tarih ?? DateTime.Now
            };

            var olusturulanHareket = await _stokHareketServisi.HareketOlusturAsync(hareket);
            var responseDTO = new StokHareketDTO
            {
                Id = olusturulanHareket.Id,
                UrunId = olusturulanHareket.UrunId,
                UrunAdi = olusturulanHareket.Urun?.Ad,
                Adet = olusturulanHareket.Adet,
                HareketTuru = olusturulanHareket.HareketTuru,
                Tarih = olusturulanHareket.Tarih
            };

            return CreatedAtAction(nameof(HareketGetir), new { id = olusturulanHareket.Id }, responseDTO);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Stok hareket oluşturulurken iş kuralı hatası.");
            return BadRequest(new { Hata = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stok hareket oluşturulurken hata oluştu.");
            return StatusCode(500, new { Hata = "Stok hareket oluşturulurken bir hata oluştu." });
        }
    }

    /// <summary>
    /// Belirli bir ürüne ait stok hareketlerini getirir
    /// </summary>
    /// <param name="urunId">Ürün kimliği</param>
    /// <returns>Ürüne ait stok hareket listesi</returns>
    /// <response code="200">Başarılı - Stok hareket listesi döner</response>
    [HttpGet("hareketler/urun/{urunId}")]
    [ProducesResponseType(typeof(List<StokHareketDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<StokHareketDTO>>> UruneGoreHareketleriGetir(int urunId)
    {
        try
        {
            var hareketler = await _stokHareketServisi.UruneGoreHareketleriGetirAsync(urunId);
            var hareketDTOs = hareketler.Select(h => new StokHareketDTO
            {
                Id = h.Id,
                UrunId = h.UrunId,
                UrunAdi = h.Urun?.Ad,
                Adet = h.Adet,
                HareketTuru = h.HareketTuru,
                Tarih = h.Tarih
            }).ToList();

            return Ok(hareketDTOs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürüne göre stok hareketleri getirilirken hata oluştu. Ürün ID: {UrunId}", urunId);
            return StatusCode(500, new { Hata = "Stok hareketleri getirilirken bir hata oluştu." });
        }
    }
}

