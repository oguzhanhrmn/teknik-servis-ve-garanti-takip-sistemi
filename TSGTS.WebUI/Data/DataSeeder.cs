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
    }
}
