# Postman / API Kullanımı

Geliştirme modunda Swagger UI aktif: `http://localhost:5250/swagger`

Postman için temel adımlar:
1. Yeni Collection açın (ör. “TSGTS”).
2. Ortam değişkenleri (Environment) tanımlayın:
   - `base_url` = `http://localhost:5250`
3. Örnek istekler:
   - GET `{{base_url}}/api/dashboard/stats`
   - GET `{{base_url}}/api/customers`
   - POST `{{base_url}}/api/customers` (Body: form-data veya JSON; alanlar: `firstName`, `lastName`, `phone`, `email`, `address`, `taxNo`)
   - GET `{{base_url}}/api/devices`
   - POST `{{base_url}}/api/devices` (Body: `serialNumber`, `brandId`, `modelId`, `purchaseDate`, `warrantyEndDate`)
   - GET `{{base_url}}/api/warranty/check/{serialNumber}`
   - GET `{{base_url}}/api/parts/search?q=xyz`
   - POST `{{base_url}}/api/tickets/{id}/status` (Body: `statusId`, `actionLog`, `userId`)
4. Auth: UI girişleri cookie tabanlıdır; Postman için dev modda anonime izin verilmez. Cookie’yi aktarmak için önce UI’da oturum açın, tarayıcıdan alınan `.AspNetCore.Cookies` değerini Postman’de Cookie olarak ekleyin.
5. JSON örneği (müşteri):
```json
{
  "firstName": "Deneme",
  "lastName": "Musteri",
  "phone": "5551234567",
  "email": "deneme@example.com",
  "address": "Adres",
  "taxNo": "12345678901"
}
```

Not: Swagger’daki “Authorize” (padlock) devrede değil; cookie-auth kullanıldığı için UI oturumu ile istekleri test etmek en kolay yöntemdir. Prod senaryosunda API auth açılacaksa JWT/benzeri mekanizma eklenmelidir.
