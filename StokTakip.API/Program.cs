using Microsoft.EntityFrameworkCore;
using StokTakip.Data;
using StokTakip.Servis.Interfaces;
using StokTakip.Servis.Servisler;

var builder = WebApplication.CreateBuilder(args);

// Servisleri ekle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Stok Takip API",
        Version = "v1",
        Description = "Stok Takip Web Uygulaması için RESTful API",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Stok Takip API Desteği"
        }
    });
});

// SQLite veritabanı bağlantısını yapılandır
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=StokTakip.db";

builder.Services.AddDbContext<StokTakipContext>(options =>
    options.UseSqlite(connectionString));

// Servisleri Dependency Injection container'a ekle
builder.Services.AddScoped<IKategoriServisi, KategoriServisi>();
builder.Services.AddScoped<IUrunServisi, UrunServisi>();
builder.Services.AddScoped<IStokHareketServisi, StokHareketServisi>();

// CORS ayarları (gerekirse)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Veritabanını oluştur ve migrasyonları uygula
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<StokTakipContext>();
        context.Database.EnsureCreated(); // SQLite için otomatik tablo oluşturma
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı oluşturulurken bir hata oluştu.");
    }
}

// HTTP istek pipeline'ını yapılandır
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stok Takip API v1");
        c.RoutePrefix = string.Empty; // Swagger'ı root'ta göster
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
