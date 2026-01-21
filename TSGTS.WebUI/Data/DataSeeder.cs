using Microsoft.EntityFrameworkCore;
using TSGTS.Business.Security;
using TSGTS.Core.Entities;
using TSGTS.DataAccess;

namespace TSGTS.WebUI.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(TsgtsDbContext context)
    {
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { RoleName = "Admin", Description = "Yönetici" },
                new Role { RoleName = "Teknisyen", Description = "Teknik kullanıcı" });
            await context.SaveChangesAsync();
        }

        if (!context.PaymentTypes.Any())
        {
            context.PaymentTypes.AddRange(
                new PaymentType { TypeName = "Nakit" },
                new PaymentType { TypeName = "Kredi Kartı" },
                new PaymentType { TypeName = "Havale/EFT" });
            await context.SaveChangesAsync();
        }

        if (!context.TicketStatuses.Any())
        {
            context.TicketStatuses.AddRange(
                new TicketStatus { StatusName = "Beklemede", ColorCode = "#f1c40f" },
                new TicketStatus { StatusName = "İşlemde", ColorCode = "#3498db" },
                new TicketStatus { StatusName = "Test", ColorCode = "#9b59b6" },
                new TicketStatus { StatusName = "Hazır", ColorCode = "#2ecc71" },
                new TicketStatus { StatusName = "Kapatıldı", ColorCode = "#95a5a6" });
            await context.SaveChangesAsync();
        }

        if (!context.Users.Any())
        {
            var adminRoleId = context.Roles.First(r => r.RoleName == "Admin").Id;
            var admin = new User
            {
                Username = "admin",
                PasswordHash = PasswordHasher.Hash("Admin123!"),
                Email = "admin@example.com",
                RoleId = adminRoleId,
                IsActive = true
            };
            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }

        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { CategoryName = "Telefon" },
                new Category { CategoryName = "Bilgisayar" },
                new Category { CategoryName = "Tablet" });
            await context.SaveChangesAsync();
        }

        if (!context.Brands.Any())
        {
            context.Brands.AddRange(
                new Brand { BrandName = "Apple" },
                new Brand { BrandName = "Samsung" },
                new Brand { BrandName = "Lenovo" },
                new Brand { BrandName = "Dell" });
            await context.SaveChangesAsync();
        }

        if (!context.Models.Any())
        {
            var telefonId = context.Categories.First(c => c.CategoryName == "Telefon").Id;
            var pcId = context.Categories.First(c => c.CategoryName == "Bilgisayar").Id;
            var tabletId = context.Categories.First(c => c.CategoryName == "Tablet").Id;
            var appleId = context.Brands.First(b => b.BrandName == "Apple").Id;
            var samsungId = context.Brands.First(b => b.BrandName == "Samsung").Id;
            var lenovoId = context.Brands.First(b => b.BrandName == "Lenovo").Id;
            var dellId = context.Brands.First(b => b.BrandName == "Dell").Id;

            context.Models.AddRange(
                new Model { ModelName = "iPhone 13", BrandId = appleId, CategoryId = telefonId },
                new Model { ModelName = "Galaxy S23", BrandId = samsungId, CategoryId = telefonId },
                new Model { ModelName = "ThinkPad X1 Carbon", BrandId = lenovoId, CategoryId = pcId },
                new Model { ModelName = "MacBook Air M2", BrandId = appleId, CategoryId = pcId },
                new Model { ModelName = "Dell XPS 13", BrandId = dellId, CategoryId = pcId },
                new Model { ModelName = "iPad Pro 11", BrandId = appleId, CategoryId = tabletId },
                new Model { ModelName = "Galaxy Tab S9", BrandId = samsungId, CategoryId = tabletId });
            await context.SaveChangesAsync();
        }

        if (!context.Customers.Any())
        {
            context.Customers.AddRange(
                new Customer { FirstName = "Ahmet", LastName = "Yılmaz", Phone = "5551112233", Email = "ahmet@example.com", Address = "İstanbul", TaxNo = "12345678901" },
                new Customer { FirstName = "Ayşe", LastName = "Demir", Phone = "5554445566", Email = "ayse@example.com", Address = "Ankara", TaxNo = "98765432109" });
            await context.SaveChangesAsync();
        }

        if (!context.Devices.Any())
        {
            var iphoneModelId = context.Models.First(m => m.ModelName == "iPhone 13").Id;
            var galaxyModelId = context.Models.First(m => m.ModelName == "Galaxy S23").Id;
            var macAirId = context.Models.First(m => m.ModelName == "MacBook Air M2").Id;
            var xps13Id = context.Models.First(m => m.ModelName == "Dell XPS 13").Id;
            var ipadProId = context.Models.First(m => m.ModelName == "iPad Pro 11").Id;
            var tabS9Id = context.Models.First(m => m.ModelName == "Galaxy Tab S9").Id;

            context.Devices.AddRange(
                new Device { SerialNumber = "SN-IPH13-001", BrandId = context.Brands.First(b => b.BrandName == "Apple").Id, ModelId = iphoneModelId, PurchaseDate = DateTime.UtcNow.AddYears(-1), WarrantyEndDate = DateTime.UtcNow.AddYears(1) },
                new Device { SerialNumber = "SN-S23-002", BrandId = context.Brands.First(b => b.BrandName == "Samsung").Id, ModelId = galaxyModelId, PurchaseDate = DateTime.UtcNow.AddMonths(-8), WarrantyEndDate = DateTime.UtcNow.AddMonths(16) },
                new Device { SerialNumber = "SN-MACAIR-005", BrandId = context.Brands.First(b => b.BrandName == "Apple").Id, ModelId = macAirId, PurchaseDate = DateTime.UtcNow.AddMonths(-10), WarrantyEndDate = DateTime.UtcNow.AddMonths(14) },
                new Device { SerialNumber = "SN-XPS13-006", BrandId = context.Brands.First(b => b.BrandName == "Dell").Id, ModelId = xps13Id, PurchaseDate = DateTime.UtcNow.AddMonths(-7), WarrantyEndDate = DateTime.UtcNow.AddMonths(17) },
                new Device { SerialNumber = "SN-IPAD-007", BrandId = context.Brands.First(b => b.BrandName == "Apple").Id, ModelId = ipadProId, PurchaseDate = DateTime.UtcNow.AddMonths(-5), WarrantyEndDate = DateTime.UtcNow.AddMonths(19) },
                new Device { SerialNumber = "SN-TABS9-008", BrandId = context.Brands.First(b => b.BrandName == "Samsung").Id, ModelId = tabS9Id, PurchaseDate = DateTime.UtcNow.AddMonths(-4), WarrantyEndDate = DateTime.UtcNow.AddMonths(20) });
            await context.SaveChangesAsync();
        }

        if (!context.ServiceTickets.Any())
        {
            var musteri1 = context.Customers.First().Id;
            var musteri2 = context.Customers.Skip(1).First().Id;
            var cihaz1 = context.Devices.First().Id;
            var cihaz2 = context.Devices.Skip(1).First().Id;
            var statusBeklemede = context.TicketStatuses.First(s => s.StatusName == "Beklemede").Id;
            var statusIslemde = context.TicketStatuses.First(s => s.StatusName == "İşlemde").Id;
            var adminUserId = context.Users.First().Id;

            context.ServiceTickets.AddRange(
                new ServiceTicket
                {
                    CustomerId = musteri1,
                    DeviceId = cihaz1,
                    OpenedByUserId = adminUserId,
                    StatusId = statusBeklemede,
                    CreatedDate = DateTime.UtcNow.AddDays(-3),
                    Description = "Ekran çatlağı ve batarya hızlı bitiyor"
                },
                new ServiceTicket
                {
                    CustomerId = musteri2,
                    DeviceId = cihaz2,
                    OpenedByUserId = adminUserId,
                    StatusId = statusIslemde,
                    CreatedDate = DateTime.UtcNow.AddDays(-1),
                    Description = "Isınma problemi"
                });
            await context.SaveChangesAsync();
        }

        if (!context.SpareParts.Any())
        {
            context.SpareParts.AddRange(
                new SparePart { PartName = "iPhone 13 Batarya", PartCode = "BAT-IPH13", StockQuantity = 15, UnitPrice = 1200, CriticalLevel = 5 },
                new SparePart { PartName = "Galaxy S23 Ekran", PartCode = "SCR-S23", StockQuantity = 8, UnitPrice = 3500, CriticalLevel = 3 });
            await context.SaveChangesAsync();
        }

        if (!context.Invoices.Any())
        {
            var ticket1 = context.ServiceTickets.First().Id;
            context.Invoices.Add(new Invoice
            {
                TicketId = ticket1,
                TotalAmount = 5000,
                Discount = 0,
                TaxAmount = 900,
                FinalAmount = 5900,
                InvoiceDate = DateTime.UtcNow.Date
            });
            await context.SaveChangesAsync();
        }

        if (!context.Payments.Any())
        {
            var invoiceId = context.Invoices.First().Id;
            var nakitId = context.PaymentTypes.First(pt => pt.TypeName.Contains("Nakit")).Id;
            context.Payments.Add(new Payment
            {
                InvoiceId = invoiceId,
                PaymentTypeId = nakitId,
                Amount = 5900,
                Date = DateTime.UtcNow.Date
            });
            await context.SaveChangesAsync();
        }

        // Ek örnek kayıtlar (toplam 4 servis kaydı ve ilgili müşteri/cihaz)
        await AddSampleIfMissing(context);
    }

    private static async Task AddSampleIfMissing(TsgtsDbContext context)
    {
        // Yardımcı: müşteri bul/ekle
        async Task<Customer> EnsureCustomer(string email, string first, string last, string phone, string address, string? taxNo)
        {
            var existing = await context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (existing is not null) return existing;

            var c = new Customer
            {
                FirstName = first,
                LastName = last,
                Phone = phone,
                Email = email,
                Address = address,
                TaxNo = taxNo
            };
            await context.Customers.AddAsync(c);
            await context.SaveChangesAsync();
            return c;
        }

        async Task<Device> EnsureDevice(string serial, int brandId, int modelId, DateTime purchase, DateTime warrantyEnd)
        {
            var existing = await context.Devices.FirstOrDefaultAsync(d => d.SerialNumber == serial);
            if (existing is not null) return existing;

            var d = new Device
            {
                SerialNumber = serial,
                BrandId = brandId,
                ModelId = modelId,
                PurchaseDate = purchase,
                WarrantyEndDate = warrantyEnd
            };
            await context.Devices.AddAsync(d);
            await context.SaveChangesAsync();
            return d;
        }

        // Yardımcı: kategori/marka/model sağlama
        async Task<int> EnsureCategory(string name)
        {
            var existing = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName == name);
            if (existing != null) return existing.Id;
            var c = new Category { CategoryName = name };
            await context.Categories.AddAsync(c);
            await context.SaveChangesAsync();
            return c.Id;
        }

        async Task<int> EnsureBrand(string name)
        {
            var existing = await context.Brands.FirstOrDefaultAsync(b => b.BrandName == name);
            if (existing != null) return existing.Id;
            var b = new Brand { BrandName = name };
            await context.Brands.AddAsync(b);
            await context.SaveChangesAsync();
            return b.Id;
        }

        async Task<int> EnsureModel(string modelName, string brandName, string categoryName)
        {
            var existing = await context.Models.FirstOrDefaultAsync(m => m.ModelName == modelName);
            if (existing != null) return existing.Id;
            var brandId = await EnsureBrand(brandName);
            var categoryId = await EnsureCategory(categoryName);
            var m = new Model { ModelName = modelName, BrandId = brandId, CategoryId = categoryId };
            await context.Models.AddAsync(m);
            await context.SaveChangesAsync();
            return m.Id;
        }

        // Çekirdek lookup'lar
        var statuses = await context.TicketStatuses.ToListAsync();
        var adminUserId = context.Users.First().Id;
        var statusFallback = statuses.First().Id;
        int statusTest = statuses.FirstOrDefault(s => s.StatusName.Contains("Test", StringComparison.OrdinalIgnoreCase))?.Id ?? statusFallback;
        int statusReady = statuses.FirstOrDefault(s => s.StatusName.Contains("Haz", StringComparison.OrdinalIgnoreCase))?.Id ?? statusFallback;

        // Müşteriler
        var c3 = await EnsureCustomer("bengisu@example.com", "Bengisu", "Kaya", "5556667788", "İzmir", "11122233344");
        var c4 = await EnsureCustomer("murat@example.com", "Murat", "Celik", "5559990001", "Bursa", null);

        // Cihazlar (mevcut veya eklenen modelleri kullan)
        var modelIphone13 = await EnsureModel("iPhone 13", "Apple", "Telefon");
        var modelThinkpad = await EnsureModel("ThinkPad X1 Carbon", "Lenovo", "Bilgisayar");
        var d3 = await EnsureDevice("SN-TPX1-003", await EnsureBrand("Lenovo"), modelThinkpad, DateTime.UtcNow.AddMonths(-10), DateTime.UtcNow.AddMonths(14));
        var d4 = await EnsureDevice("SN-IPH13-004", await EnsureBrand("Apple"), modelIphone13, DateTime.UtcNow.AddMonths(-6), DateTime.UtcNow.AddMonths(18));

        // Servis kayıtları (2 yeni)
        if (!context.ServiceTickets.Any(t => t.CustomerId == c3.Id && t.DeviceId == d3.Id))
        {
            var t3 = new ServiceTicket
            {
                CustomerId = c3.Id,
                DeviceId = d3.Id,
                OpenedByUserId = adminUserId,
                StatusId = statusTest,
                CreatedDate = DateTime.UtcNow.AddDays(-2),
                Description = "Klavye bazı tuşlar çalışmıyor, fan gürültülü, aşırı ısınma ve rastgele kapanma; termal macun değişimi ihtiyacı",
                ServiceCode = "SRV-DEMO-003"
            };
            await context.ServiceTickets.AddAsync(t3);
            await context.SaveChangesAsync();

            context.Invoices.Add(new Invoice
            {
                TicketId = t3.Id,
                TotalAmount = 2600,
                Discount = 0,
                TaxAmount = 468,
                FinalAmount = 3068,
                InvoiceDate = DateTime.UtcNow.AddDays(-1)
            });
            await context.SaveChangesAsync();
        }

        if (!context.ServiceTickets.Any(t => t.CustomerId == c4.Id && t.DeviceId == d4.Id))
        {
            var t4 = new ServiceTicket
            {
                CustomerId = c4.Id,
                DeviceId = d4.Id,
                OpenedByUserId = adminUserId,
                StatusId = statusReady,
                CreatedDate = DateTime.UtcNow.AddDays(-5),
                Description = "Ön cam kırığı, arka kamera bulanık, FaceID kalibrasyonu ve ana kart temas sorunu giderildi",
                ServiceCode = "SRV-DEMO-004"
            };
            await context.ServiceTickets.AddAsync(t4);
            await context.SaveChangesAsync();

            context.Invoices.Add(new Invoice
            {
                TicketId = t4.Id,
                TotalAmount = 4100,
                Discount = 0,
                TaxAmount = 738,
                FinalAmount = 4838,
                InvoiceDate = DateTime.UtcNow.AddDays(-4)
            });
            await context.SaveChangesAsync();
        }
    }
}
