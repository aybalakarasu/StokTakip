# 📦 Stok Takip Web Uygulaması

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4?logo=dotnet)](https://docs.microsoft.com/ef/core/)
[![SQLite](https://img.shields.io/badge/SQLite-3-blue?logo=sqlite)](https://www.sqlite.org/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

.NET 8 kullanılarak geliştirilmiş, katmanlı mimariye sahip, SQLite veritabanı kullanan profesyonel bir Stok Takip Web Uygulaması.

## 📋 İçindekiler

- [Proje Özellikleri](#-proje-özellikleri)
- [Proje Yapısı](#-proje-yapısı)
- [Kurulum ve Çalıştırma](#-kurulum-ve-çalıştırma)
- [API Dokümantasyonu](#-api-endpoints)
- [Teknolojiler](#-teknolojiler)
- [Katkıda Bulunma](#-katkıda-bulunma)
- [Lisans](#-lisans)

## 📋 Proje Özellikleri

- **Katmanlı Mimari**: Data, Servis, API ve Web katmanları ile ayrılmış yapı
- **Entity Framework Core 8**: SQLite veritabanı ile ORM desteği
- **ASP.NET Core MVC**: Modern web arayüzü
- **RESTful API**: Tam CRUD işlemleri için API desteği
- **Dependency Injection**: Profesyonel bağımlılık yönetimi
- **Asenkron Programlama**: Tüm veritabanı işlemleri async/await ile
- **Validasyon**: Türkçe hata mesajları ile kapsamlı validasyon
- **Bootstrap**: Modern ve responsive arayüz

## 🏗️ Proje Yapısı

```
StokTakip/
├── StokTakip.sln                    # Solution dosyası
├── StokTakip.Data/                   # Veritabanı katmanı
│   ├── Modeller/                     # Entity sınıfları
│   │   ├── Kategori.cs
│   │   ├── Urun.cs
│   │   ├── Musteri.cs
│   │   └── StokHareketi.cs
│   └── StokTakipContext.cs          # DbContext
├── StokTakip.Servis/                 # İş mantığı katmanı
│   ├── Interfaces/                  # Servis arayüzleri
│   └── Servisler/                   # Servis implementasyonları
├── StokTakip.API/                    # Web API katmanı
│   ├── Controllers/                  # API Controller'ları
│   └── DTOs/                         # Veri transfer nesneleri
└── StokTakip.Web/                    # MVC Web uygulaması
    ├── Controllers/                  # MVC Controller'ları
    ├── Views/                        # Razor View'ları
    └── ViewModels/                   # View modelleri
```

## 📦 Modeller

### Kategori
- **Id**: Kategori benzersiz kimliği
- **Ad**: Kategori adı

### Ürün
- **Id**: Ürün benzersiz kimliği
- **Ad**: Ürün adı
- **Stok**: Stok miktarı
- **Fiyat**: Ürün fiyatı
- **KategoriId**: Bağlı kategori kimliği
- **Kategori**: Kategori navigation property

### Müşteri
- **Id**: Müşteri benzersiz kimliği
- **AdSoyad**: Müşteri adı ve soyadı

### StokHareketi
- **Id**: Stok hareketi benzersiz kimliği
- **UrunId**: İlgili ürün kimliği
- **Adet**: Hareket adedi
- **HareketTuru**: Giriş veya Çıkış
- **Tarih**: Hareket tarihi
- **Urun**: Ürün navigation property

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler

- .NET 8 SDK
- Visual Studio 2022 veya Visual Studio Code (veya herhangi bir IDE)

### Adımlar

1. **Projeyi klonlayın**
   ```bash
   git clone https://github.com/kullaniciadi/StokTakipYeni.git
   cd StokTakipYeni
   ```
   
   Veya ZIP olarak indirip açabilirsiniz.

2. **Paketleri geri yükleyin**
   ```bash
   dotnet restore
   ```

3. **Veritabanını oluşturun**
   
   Veritabanı otomatik olarak oluşturulacaktır. İlk çalıştırmada `StokTakip.db` dosyası proje kök dizininde oluşturulur ve seed verileri eklenir.

4. **Web uygulamasını çalıştırın**
   ```bash
   cd StokTakip.Web
   dotnet run
   ```
   
   Veya Visual Studio'da `StokTakip.Web` projesini başlangıç projesi olarak ayarlayıp F5 ile çalıştırın.

5. **Tarayıcıda açın**
   
   Uygulama genellikle `https://localhost:5001` veya `http://localhost:5000` adresinde çalışır.

### API'yi Çalıştırma

API'yi ayrı olarak çalıştırmak için:

```bash
cd StokTakip.API
dotnet run
```

API genellikle `https://localhost:7001` veya `http://localhost:5000` adresinde çalışır. Swagger UI için root URL'e gidin (`/`).

## 📡 API Endpoints

### Ürünler (`/api/urunler`)

- `GET /api/urunler` - Tüm ürünleri getir
- `GET /api/urunler/{id}` - Belirli bir ürünü getir
- `POST /api/urunler` - Yeni ürün ekle
- `PUT /api/urunler/{id}` - Ürün güncelle
- `DELETE /api/urunler/{id}` - Ürün sil

### Kategoriler (`/api/kategoriler`)

- `GET /api/kategoriler` - Tüm kategorileri getir
- `GET /api/kategoriler/{id}` - Belirli bir kategoriyi getir
- `POST /api/kategoriler` - Yeni kategori ekle
- `PUT /api/kategoriler/{id}` - Kategori güncelle
- `DELETE /api/kategoriler/{id}` - Kategori sil

### Stok Hareketleri (`/api/stok`)

- `GET /api/stok/hareketler` - Tüm stok hareketlerini getir
- `GET /api/stok/hareketler/{id}` - Belirli bir stok hareketini getir
- `POST /api/stok/hareket` - Yeni stok hareketi oluştur (stok otomatik güncellenir)
- `GET /api/stok/hareketler/urun/{urunId}` - Belirli bir ürüne ait hareketleri getir

## 📝 API Kullanım Örnekleri

### Ürün Ekleme

```http
POST /api/urunler
Content-Type: application/json

{
  "ad": "Laptop",
  "stok": 10,
  "fiyat": 15000.00,
  "kategoriId": 1
}
```

### Stok Hareketi Oluşturma

```http
POST /api/stok/hareket
Content-Type: application/json

{
  "urunId": 1,
  "adet": 5,
  "hareketTuru": 1,
  "tarih": "2025-01-08T10:00:00"
}
```

**Hareket Türleri:**
- `1` = Giriş
- `2` = Çıkış

## 🗄️ Veritabanı

- **Veritabanı Türü**: SQLite
- **Dosya Konumu**: Proje kök dizininde `StokTakip.db`
- **Seed Verileri**: İlk çalıştırmada otomatik olarak örnek veriler eklenir:
  - 5 kategori (Elektronik, Giyim, Gıda, Ev Eşyası, Kitap)
  - 5 ürün
  - 3 müşteri

## 🎨 Web Arayüzü Özellikleri

- **Ana Sayfa**: Genel bakış ve son stok hareketleri
- **Ürünler**: Ürün listesi, ekleme, düzenleme, silme
- **Kategoriler**: Kategori yönetimi
- **Stok Hareketleri**: Stok giriş/çıkış işlemleri
- **Responsive Tasarım**: Bootstrap ile mobil uyumlu arayüz

## 🔧 Teknolojiler

- **.NET 8**
- **ASP.NET Core MVC**
- **Entity Framework Core 8**
- **SQLite**
- **Bootstrap 5**
- **jQuery**
- **Swagger/OpenAPI** (API için)

## 📂 SQLite Dosyası Konumu

SQLite veritabanı dosyası (`StokTakip.db`) proje kök dizininde oluşturulur. Bu dosya:

- İlk çalıştırmada otomatik oluşturulur
- Seed verileri ile doldurulur
- Tüm veriler bu dosyada saklanır

**Not**: Veritabanı dosyasını silerseniz, uygulama tekrar çalıştırıldığında otomatik olarak yeniden oluşturulur.

## 🛠️ Geliştirme

### Projeyi Derleme

```bash
dotnet build
```

### Tüm Projeleri Test Etme

```bash
dotnet build StokTakip.sln
```

## 🤝 Katkıda Bulunma

Katkılarınızı bekliyoruz! Lütfen şu adımları izleyin:

1. Bu projeyi fork edin
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some AmazingFeature'`)
4. Branch'inizi push edin (`git push origin feature/AmazingFeature`)
5. Pull Request açın

## 📄 Lisans

Bu proje [MIT Lisansı](LICENSE) altında lisanslanmıştır.

## 👨‍💻 Geliştirici Notları

- Tüm kodlar, açıklamalar ve dokümantasyon Türkçe'dir
- Katmanlı mimari prensiplerine uygun olarak geliştirilmiştir
- Dependency Injection kullanılmıştır
- Asenkron programlama best practice'leri uygulanmıştır
- Validasyon kuralları Türkçe hata mesajları ile tanımlanmıştır

## 🐛 Bilinen Sorunlar

Şu anda bilinen bir sorun bulunmamaktadır.

## 📞 Destek

Sorularınız için issue açabilirsiniz.

---

**Not**: Bu proje tamamen Türkçe olarak geliştirilmiştir. Tüm kodlar, açıklamalar, yorum satırları ve dokümantasyon Türkçe'dir.

