using Microsoft.EntityFrameworkCore;
using StokTakip.Data.Modeller;

namespace StokTakip.Data;

/// <summary>
/// Entity Framework Core veritabanı bağlam sınıfı
/// </summary>
public class StokTakipContext : DbContext
{
    /// <summary>
    /// Kategoriler veritabanı seti
    /// </summary>
    public DbSet<Kategori> Kategoriler { get; set; }

    /// <summary>
    /// Ürünler veritabanı seti
    /// </summary>
    public DbSet<Urun> Urunler { get; set; }

    /// <summary>
    /// Müşteriler veritabanı seti
    /// </summary>
    public DbSet<Musteri> Musteriler { get; set; }

    /// <summary>
    /// Stok hareketleri veritabanı seti
    /// </summary>
    public DbSet<StokHareketi> StokHareketleri { get; set; }

    /// <summary>
    /// Veritabanı bağlantı dizisi
    /// </summary>
    private readonly string? _connectionString;

    /// <summary>
    /// Varsayılan constructor - connection string parametresi ile
    /// </summary>
    public StokTakipContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// DbContextOptions ile constructor (Dependency Injection için)
    /// </summary>
    public StokTakipContext(DbContextOptions<StokTakipContext> options) : base(options)
    {
    }

    /// <summary>
    /// Veritabanı yapılandırması
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(_connectionString ?? "Data Source=StokTakip.db");
        }
    }

    /// <summary>
    /// Model yapılandırması ve Fluent API ayarları
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Kategori yapılandırması
        modelBuilder.Entity<Kategori>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Ad)
                .IsRequired()
                .HasMaxLength(100);
            entity.HasIndex(e => e.Ad).IsUnique();
        });

        // Ürün yapılandırması
        modelBuilder.Entity<Urun>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Ad)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Stok)
                .IsRequired()
                .HasDefaultValue(0);
            entity.Property(e => e.Fiyat)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            entity.Property(e => e.KategoriId)
                .IsRequired();

            // Ürün -> Kategori ilişkisi
            entity.HasOne(e => e.Kategori)
                .WithMany(k => k.Urunler)
                .HasForeignKey(e => e.KategoriId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Müşteri yapılandırması
        modelBuilder.Entity<Musteri>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AdSoyad)
                .IsRequired()
                .HasMaxLength(200);
        });

        // StokHareketi yapılandırması
        modelBuilder.Entity<StokHareketi>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UrunId)
                .IsRequired();
            entity.Property(e => e.Adet)
                .IsRequired();
            entity.Property(e => e.HareketTuru)
                .IsRequired()
                .HasConversion<int>();
            entity.Property(e => e.Tarih)
                .IsRequired()
                .HasDefaultValueSql("datetime('now')");

            // StokHareketi -> Ürün ilişkisi
            entity.HasOne(e => e.Urun)
                .WithMany(u => u.StokHareketleri)
                .HasForeignKey(e => e.UrunId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tarih için index
            entity.HasIndex(e => e.Tarih);
        });

        // Seed verileri
        SeedVerileri(modelBuilder);
    }

    /// <summary>
    /// Veritabanına başlangıç verilerini ekler
    /// </summary>
    private void SeedVerileri(ModelBuilder modelBuilder)
    {
        // Kategoriler
        modelBuilder.Entity<Kategori>().HasData(
            new Kategori { Id = 1, Ad = "Elektronik" },
            new Kategori { Id = 2, Ad = "Giyim" },
            new Kategori { Id = 3, Ad = "Gıda" },
            new Kategori { Id = 4, Ad = "Ev Eşyası" },
            new Kategori { Id = 5, Ad = "Kitap" }
        );

        // Müşteriler
        modelBuilder.Entity<Musteri>().HasData(
            new Musteri { Id = 1, AdSoyad = "Ahmet Yılmaz" },
            new Musteri { Id = 2, AdSoyad = "Ayşe Demir" },
            new Musteri { Id = 3, AdSoyad = "Mehmet Kaya" }
        );

        // Ürünler
        modelBuilder.Entity<Urun>().HasData(
            new Urun { Id = 1, Ad = "Laptop", Stok = 10, Fiyat = 15000.00m, KategoriId = 1 },
            new Urun { Id = 2, Ad = "Tişört", Stok = 50, Fiyat = 150.00m, KategoriId = 2 },
            new Urun { Id = 3, Ad = "Ekmek", Stok = 100, Fiyat = 5.00m, KategoriId = 3 },
            new Urun { Id = 4, Ad = "Masa", Stok = 20, Fiyat = 2500.00m, KategoriId = 4 },
            new Urun { Id = 5, Ad = "Roman", Stok = 75, Fiyat = 50.00m, KategoriId = 5 }
        );
    }
}

