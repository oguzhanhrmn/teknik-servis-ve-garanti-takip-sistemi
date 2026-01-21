# Teknik Servis ve Garanti Takip Sistemi (TSGTS)

## Çalıştırma
1) Gerekirse açık örneği kapat: `Get-Process -Name TSGTS.WebUI -ErrorAction SilentlyContinue | Stop-Process -Force`
2) Veri tabanı bağlantısı `appsettings*.json` içinde `DefaultConnection` (örn. `localhost\SQLEXPRESS01`, DB: `TSGTS`).
3) Migration uygulamak için (gerekirse): `dotnet ef database update --project TSGTS.DataAccess --startup-project TSGTS.WebUI`
4) Uygulamayı başlat: `dotnet run --project TSGTS.WebUI`
5) Giriş: kullanıcı adı `admin`, şifre `Admin123!`
6) API dokümantasyonu: Geliştirme modunda Swagger UI: `http://localhost:5250/swagger`
7) Testler (örnek unit): `dotnet test TSGTS.Tests/TSGTS.Tests.csproj`

## Özellikler
- Çok katmanlı mimari (Core/DataAccess/Business/WebUI)
- EF Core Code-First, repository + servis katmanı
- Roller: Admin, Teknisyen (cookie auth + yetkilendirme)
- CRUD: Müşteriler, Cihazlar, Servis Kayıtları, Yedek Parçalar, Fatura, Ödeme
- Dashboard: durum dağılımı, son 7 gün trendi, son servis kayıtları, kritik stok listesi
- Ajax arama: Müşteriler listesinde canlı arama/filtre

## Örnek seed verileri
- Kullanıcı: `admin / Admin123!`
- Roller, ödeme tipleri, durumlar, örnek müşteri/cihaz/kayıt/fatura/ödeme, kritik stok örnekleri uygulama açılışında otomatik eklenir.

## Postman / API
- Geliştirme modunda Swagger ile test edebilirsiniz (`/swagger`).
- Postman için `docs/POSTMAN.md` içindeki uç noktaları kullanın; UI’dan aldığınız cookie ile çağrı yapabilirsiniz.
- Public API (lookup, opsiyonel): `/api/public/tickets/lookup?code={SRV-...}&contact={tel/eposta}`, `/api/public/tickets/{id}/timeline?contact=...`

## Test Planı
- Unit test çalıştırma: `dotnet test TSGTS.Tests/TSGTS.Tests.csproj`
- Detaylı test planı ve kabul adımları için: `docs/TESTPLAN.md`
