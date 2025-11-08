using Microsoft.EntityFrameworkCore;
using StokTakip.Data;
using StokTakip.Servis.Interfaces;
using StokTakip.Servis.Servisler;

var builder = WebApplication.CreateBuilder(args);

// Servisleri ekle
builder.Services.AddControllersWithViews();

// SQLite veritabanı bağlantısını yapılandır
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=StokTakip.db";

builder.Services.AddDbContext<StokTakipContext>(options =>
    options.UseSqlite(connectionString));

// Servisleri Dependency Injection container'a ekle
builder.Services.AddScoped<IKategoriServisi, KategoriServisi>();
builder.Services.AddScoped<IUrunServisi, UrunServisi>();
builder.Services.AddScoped<IStokHareketServisi, StokHareketServisi>();

var app = builder.Build();

// Veritabanını oluştur
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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Hata");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
