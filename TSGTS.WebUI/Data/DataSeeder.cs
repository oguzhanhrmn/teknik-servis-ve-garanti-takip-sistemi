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
                new Brand { BrandName = "Lenovo" });
            await context.SaveChangesAsync();
        }

        if (!context.Models.Any())
        {
            var telefonId = context.Categories.First(c => c.CategoryName == "Telefon").Id;
            var pcId = context.Categories.First(c => c.CategoryName == "Bilgisayar").Id;
            var appleId = context.Brands.First(b => b.BrandName == "Apple").Id;
            var samsungId = context.Brands.First(b => b.BrandName == "Samsung").Id;
            var lenovoId = context.Brands.First(b => b.BrandName == "Lenovo").Id;

            context.Models.AddRange(
                new Model { ModelName = "iPhone 13", BrandId = appleId, CategoryId = telefonId },
                new Model { ModelName = "Galaxy S23", BrandId = samsungId, CategoryId = telefonId },
                new Model { ModelName = "ThinkPad X1 Carbon", BrandId = lenovoId, CategoryId = pcId });
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

            context.Devices.AddRange(
                new Device { SerialNumber = "SN-IPH13-001", BrandId = context.Brands.First(b => b.BrandName == "Apple").Id, ModelId = iphoneModelId, PurchaseDate = DateTime.UtcNow.AddYears(-1), WarrantyEndDate = DateTime.UtcNow.AddYears(1) },
                new Device { SerialNumber = "SN-S23-002", BrandId = context.Brands.First(b => b.BrandName == "Samsung").Id, ModelId = galaxyModelId, PurchaseDate = DateTime.UtcNow.AddMonths(-8), WarrantyEndDate = DateTime.UtcNow.AddMonths(16) });
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
    }
}
