# Test Planı (TSGTS)

## 1. Unit Testler
- `dotnet test TSGTS.Tests/TSGTS.Tests.csproj`
- Mevcut örnekler:
  - `CustomerManagerTests`: Create çağrısı müşteri ekliyor mu.
  - `TicketManagerTests`: Durum güncellemesi log oluşturuyor mu.

## 2. Integration Test (öneri)
- InMemory/SQLite provider ile uçtan uca CRUD (Müşteri, Cihaz, Ticket) senaryosu eklenmeli.
- Garanti kontrolü, durum değişimi ve fatura/ödeme akışı için “happy path” testi yazılmalı.

## 3. Manuel Kabul (Happy Path)
1) Giriş: `admin / Admin123!`
2) Müşteri ekle (UI) → cihaz ekle → ticket oluştur.
3) Ticket durumunu güncelle (Beklemede → İşlemde → Hazır).
4) Dashboard’da durum grafiği ve son kayıtlar güncelleniyor mu kontrol et.
5) Yedek parça kritik stok uyarıları listeleniyor mu kontrol et.
6) Fatura/ödeme kaydı ekle; listelerde göründüğünü doğrula.
7) API testleri: Swagger veya Postman ile `/api/dashboard/stats`, `/api/customers`, `/api/devices`, `/api/warranty/check/{serialNumber}`, `/api/parts/search`.
