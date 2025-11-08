using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokTakip.API.DTOs;
using StokTakip.Data.Modeller;
using StokTakip.Servis.Interfaces;

namespace StokTakip.API.Controllers;

/// <summary>
/// Kategori işlemlerini yöneten API controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class KategorilerController : ControllerBase
{
    private readonly IKategoriServisi _kategoriServisi;
    private readonly IUrunServisi _urunServisi;
    private readonly ILogger<KategorilerController> _logger;

    /// <summary>
    /// Kategoriler controller constructor
    /// </summary>
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
    /// Tüm kategorileri getirir
    /// </summary>
    /// <returns>Kategori listesi</returns>
    /// <response code="200">Başarılı - Kategori listesi döner</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<KategoriDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<KategoriDTO>>> TumKategorileriGetir()
    {
        try
        {
            var kategoriler = await _kategoriServisi.TumKategorileriGetirAsync();
            var urunler = await _urunServisi.TumUrunleriGetirAsync();

            var kategoriDTOs = kategoriler.Select(k => new KategoriDTO
            {
                Id = k.Id,
                Ad = k.Ad,
                UrunSayisi = urunler.Count(u => u.KategoriId == k.Id)
            }).ToList();

            return Ok(kategoriDTOs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategoriler getirilirken hata oluştu.");
            return StatusCode(500, new { Hata = "Kategoriler getirilirken bir hata oluştu." });
        }
    }

    /// <summary>
    /// Belirli bir kategoriyi ID'ye göre getirir
    /// </summary>
    /// <param name="id">Kategori kimliği</param>
    /// <returns>Kategori bilgisi</returns>
    /// <response code="200">Başarılı - Kategori bilgisi döner</response>
    /// <response code="404">Kategori bulunamadı</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(KategoriDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<KategoriDTO>> KategoriGetir(int id)
    {
        try
        {
            var kategori = await _kategoriServisi.KategoriGetirAsync(id);
            if (kategori == null)
                return NotFound(new { Hata = $"ID'si {id} olan kategori bulunamadı." });

            var urunler = await _urunServisi.KategoriyeGoreUrunleriGetirAsync(id);
            var kategoriDTO = new KategoriDTO
            {
                Id = kategori.Id,
                Ad = kategori.Ad,
                UrunSayisi = urunler.Count
            };

            return Ok(kategoriDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategori getirilirken hata oluştu. ID: {Id}", id);
            return StatusCode(500, new { Hata = "Kategori getirilirken bir hata oluştu." });
        }
    }

    /// <summary>
    /// Yeni kategori ekler
    /// </summary>
    /// <param name="kategoriDTO">Kategori bilgileri</param>
    /// <returns>Oluşturulan kategori</returns>
    /// <response code="201">Başarılı - Kategori oluşturuldu</response>
    /// <response code="400">Geçersiz veri</response>
    [HttpPost]
    [ProducesResponseType(typeof(KategoriDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<KategoriDTO>> KategoriEkle([FromBody] KategoriOlusturDTO kategoriDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var kategori = new Kategori
            {
                Ad = kategoriDTO.Ad
            };

            var olusturulanKategori = await _kategoriServisi.KategoriEkleAsync(kategori);
            var responseDTO = new KategoriDTO
            {
                Id = olusturulanKategori.Id,
                Ad = olusturulanKategori.Ad,
                UrunSayisi = 0
            };

            return CreatedAtAction(nameof(KategoriGetir), new { id = olusturulanKategori.Id }, responseDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategori eklenirken hata oluştu.");
            return StatusCode(500, new { Hata = "Kategori eklenirken bir hata oluştu." });
        }
    }

    /// <summary>
    /// Mevcut kategoriyi günceller
    /// </summary>
    /// <param name="id">Kategori kimliği</param>
    /// <param name="kategoriDTO">Güncellenecek kategori bilgileri</param>
    /// <returns>Güncellenmiş kategori</returns>
    /// <response code="200">Başarılı - Kategori güncellendi</response>
    /// <response code="400">Geçersiz veri</response>
    /// <response code="404">Kategori bulunamadı</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(KategoriDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<KategoriDTO>> KategoriGuncelle(int id, [FromBody] KategoriOlusturDTO kategoriDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var kategori = new Kategori
            {
                Id = id,
                Ad = kategoriDTO.Ad
            };

            var guncellendi = await _kategoriServisi.KategoriGuncelleAsync(kategori);
            if (!guncellendi)
                return NotFound(new { Hata = $"ID'si {id} olan kategori bulunamadı." });

            var guncellenmisKategori = await _kategoriServisi.KategoriGetirAsync(id);
            var urunler = await _urunServisi.KategoriyeGoreUrunleriGetirAsync(id);
            var responseDTO = new KategoriDTO
            {
                Id = guncellenmisKategori!.Id,
                Ad = guncellenmisKategori.Ad,
                UrunSayisi = urunler.Count
            };

            return Ok(responseDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategori güncellenirken hata oluştu. ID: {Id}", id);
            return StatusCode(500, new { Hata = "Kategori güncellenirken bir hata oluştu." });
        }
    }

    /// <summary>
    /// Kategoriyi siler
    /// </summary>
    /// <param name="id">Kategori kimliği</param>
    /// <returns>Silme işlemi sonucu</returns>
    /// <response code="200">Başarılı - Kategori silindi</response>
    /// <response code="404">Kategori bulunamadı</response>
    /// <response code="400">Kategori silinemez (ürün var)</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> KategoriSil(int id)
    {
        try
        {
            var silindi = await _kategoriServisi.KategoriSilAsync(id);
            if (!silindi)
            {
                var kategoriVarMi = await _kategoriServisi.KategoriVarMiAsync(id);
                if (!kategoriVarMi)
                    return NotFound(new { Hata = $"ID'si {id} olan kategori bulunamadı." });
                
                return BadRequest(new { Hata = "Bu kategoriye ait ürün bulunduğu için silinemez." });
            }

            return Ok(new { Mesaj = "Kategori başarıyla silindi." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategori silinirken hata oluştu. ID: {Id}", id);
            return StatusCode(500, new { Hata = "Kategori silinirken bir hata oluştu." });
        }
    }
}

