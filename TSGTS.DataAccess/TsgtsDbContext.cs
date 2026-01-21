using Microsoft.EntityFrameworkCore;
using TSGTS.Core.Entities;

namespace TSGTS.DataAccess;

public class TsgtsDbContext : DbContext
{
    public TsgtsDbContext(DbContextOptions<TsgtsDbContext> options) : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Model> Models => Set<Model>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ServiceTicket> ServiceTickets => Set<ServiceTicket>();
    public DbSet<TicketStatus> TicketStatuses => Set<TicketStatus>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<SparePart> SpareParts => Set<SparePart>();
    public DbSet<ServicePartUsage> ServicePartUsages => Set<ServicePartUsage>();
    public DbSet<LaborRate> LaborRates => Set<LaborRate>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentType> PaymentTypes => Set<PaymentType>();
    public DbSet<ActionLog> ActionLogs => Set<ActionLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>()
            .ToTable("Roller")
            .Property(r => r.RoleName)
            .HasColumnName("RolAdi")
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<Role>()
            .Property(r => r.Description)
            .HasColumnName("Aciklama");

        modelBuilder.Entity<User>()
            .ToTable("Kullanicilar")
            .HasIndex(u => u.Username)
            .IsUnique();
        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .HasColumnName("KullaniciAdi")
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .HasColumnName("SifreOzeti")
            .HasMaxLength(256)
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasColumnName("Eposta")
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property(u => u.RoleId)
            .HasColumnName("RolId");
        modelBuilder.Entity<User>()
            .Property(u => u.IsActive)
            .HasColumnName("Aktif");
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Customer>()
            .ToTable("Musteriler")
            .Property(c => c.FirstName)
            .HasColumnName("Ad")
            .HasMaxLength(75)
            .IsRequired();
        modelBuilder.Entity<Customer>()
            .Property(c => c.LastName)
            .HasColumnName("Soyad")
            .HasMaxLength(75)
            .IsRequired();
        modelBuilder.Entity<Customer>()
            .Property(c => c.Phone)
            .HasColumnName("Telefon")
            .HasMaxLength(20)
            .IsRequired();
        modelBuilder.Entity<Customer>()
            .Property(c => c.Email)
            .HasColumnName("Eposta")
            .HasMaxLength(100);
        modelBuilder.Entity<Customer>()
            .Property(c => c.TaxNo)
            .HasColumnName("VergiNo")
            .HasMaxLength(20);
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.TaxNo)
            .IsUnique()
            .HasFilter("[TaxNo] IS NOT NULL");

        modelBuilder.Entity<Brand>()
            .ToTable("Markalar")
            .Property(b => b.BrandName)
            .HasColumnName("MarkaAdi")
            .HasMaxLength(75)
            .IsRequired();

        modelBuilder.Entity<Category>()
            .ToTable("Kategoriler")
            .Property(c => c.CategoryName)
            .HasColumnName("KategoriAdi")
            .HasMaxLength(75)
            .IsRequired();

        modelBuilder.Entity<Model>()
            .ToTable("Modeller")
            .Property(m => m.ModelName)
            .HasColumnName("ModelAdi")
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<Model>()
            .Property(m => m.BrandId)
            .HasColumnName("MarkaId");
        modelBuilder.Entity<Model>()
            .Property(m => m.CategoryId)
            .HasColumnName("KategoriId");
        modelBuilder.Entity<Model>()
            .HasOne(m => m.Brand)
            .WithMany(b => b.Models)
            .HasForeignKey(m => m.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Model>()
            .HasOne(m => m.Category)
            .WithMany(c => c.Models)
            .HasForeignKey(m => m.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Device>()
            .ToTable("Cihazlar")
            .Property(d => d.SerialNumber)
            .HasColumnName("SeriNo")
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<Device>()
            .HasIndex(d => d.SerialNumber)
            .IsUnique();
        modelBuilder.Entity<Device>()
            .Property(d => d.BrandId)
            .HasColumnName("MarkaId");
        modelBuilder.Entity<Device>()
            .Property(d => d.ModelId)
            .HasColumnName("ModelId");
        modelBuilder.Entity<Device>()
            .Property(d => d.PurchaseDate)
            .HasColumnName("SatinAlmaTarihi");
        modelBuilder.Entity<Device>()
            .Property(d => d.WarrantyEndDate)
            .HasColumnName("GarantiBitisTarihi");
        modelBuilder.Entity<Device>()
            .HasOne(d => d.Brand)
            .WithMany(b => b.Devices)
            .HasForeignKey(d => d.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Device>()
            .HasOne(d => d.Model)
            .WithMany(m => m.Devices)
            .HasForeignKey(d => d.ModelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TicketStatus>()
            .ToTable("ServisDurumlari")
            .Property(s => s.StatusName)
            .HasColumnName("DurumAdi")
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<TicketStatus>()
            .Property(s => s.ColorCode)
            .HasColumnName("RenkKodu")
            .HasMaxLength(7);

        modelBuilder.Entity<ServiceTicket>()
            .ToTable("ServisKayitlari")
            .Property(t => t.ServiceCode)
            .HasColumnName("ServisKodu")
            .HasMaxLength(50);
        modelBuilder.Entity<ServiceTicket>()
            .Property(t => t.Description)
            .HasColumnName("Aciklama")
            .HasMaxLength(1000);
        modelBuilder.Entity<ServiceTicket>()
            .Property(t => t.CustomerId)
            .HasColumnName("MusteriId");
        modelBuilder.Entity<ServiceTicket>()
            .Property(t => t.DeviceId)
            .HasColumnName("CihazId");
        modelBuilder.Entity<ServiceTicket>()
            .Property(t => t.OpenedByUserId)
            .HasColumnName("AcanKullaniciId");
        modelBuilder.Entity<ServiceTicket>()
            .Property(t => t.StatusId)
            .HasColumnName("DurumId");
        modelBuilder.Entity<ServiceTicket>()
            .Property(t => t.CreatedDate)
            .HasColumnName("OlusturmaTarihi");
        modelBuilder.Entity<ServiceTicket>()
            .HasOne(t => t.Customer)
            .WithMany(c => c.ServiceTickets)
            .HasForeignKey(t => t.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ServiceTicket>()
            .HasOne(t => t.Device)
            .WithMany(d => d.ServiceTickets)
            .HasForeignKey(t => t.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ServiceTicket>()
            .HasOne(t => t.OpenedByUser)
            .WithMany(u => u.OpenedTickets)
            .HasForeignKey(t => t.OpenedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ServiceTicket>()
            .HasOne(t => t.Status)
            .WithMany(s => s.ServiceTickets)
            .HasForeignKey(t => t.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Assignment>()
            .ToTable("Atamalar")
            .Property(a => a.TicketId)
            .HasColumnName("KayitId");
        modelBuilder.Entity<Assignment>()
            .Property(a => a.TechnicianId)
            .HasColumnName("TeknisyenId");
        modelBuilder.Entity<Assignment>()
            .Property(a => a.AssignedDate)
            .HasColumnName("AtamaTarihi");
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Ticket)
            .WithMany(t => t.Assignments)
            .HasForeignKey(a => a.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Technician)
            .WithMany(u => u.Assignments)
            .HasForeignKey(a => a.TechnicianId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SparePart>()
            .ToTable("YedekParcalar")
            .Property(p => p.PartName)
            .HasColumnName("ParcaAdi")
            .HasMaxLength(150)
            .IsRequired();
        modelBuilder.Entity<SparePart>()
            .Property(p => p.PartCode)
            .HasColumnName("ParcaKodu")
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<SparePart>()
            .HasIndex(p => p.PartCode)
            .IsUnique();
        modelBuilder.Entity<SparePart>()
            .Property(p => p.UnitPrice)
            .HasColumnName("BirimFiyat")
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<SparePart>()
            .Property(p => p.StockQuantity)
            .HasColumnName("StokMiktari");
        modelBuilder.Entity<SparePart>()
            .Property(p => p.CriticalLevel)
            .HasColumnName("KritikSeviye");

        modelBuilder.Entity<ServicePartUsage>()
            .ToTable("ServisParcaKullanimlari")
            .Property(u => u.TicketId)
            .HasColumnName("KayitId");
        modelBuilder.Entity<ServicePartUsage>()
            .Property(u => u.PartId)
            .HasColumnName("ParcaId");
        modelBuilder.Entity<ServicePartUsage>()
            .Property(u => u.Quantity)
            .HasColumnName("Miktar");
        modelBuilder.Entity<ServicePartUsage>()
            .Property(u => u.UsedDate)
            .HasColumnName("KullanimTarihi");
        modelBuilder.Entity<ServicePartUsage>()
            .HasOne(u => u.Ticket)
            .WithMany(t => t.PartUsages)
            .HasForeignKey(u => u.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ServicePartUsage>()
            .HasOne(u => u.Part)
            .WithMany(p => p.PartUsages)
            .HasForeignKey(u => u.PartId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LaborRate>()
            .ToTable("IscilikUcretleri")
            .Property(l => l.ServiceType)
            .HasColumnName("HizmetTuru")
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<LaborRate>()
            .Property(l => l.HourlyRate)
            .HasColumnName("SaatlikUcret")
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Invoice>()
            .ToTable("Faturalar")
            .Property(i => i.TotalAmount)
            .HasColumnName("ToplamTutar")
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Invoice>()
            .Property(i => i.Discount)
            .HasColumnName("Indirim")
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Invoice>()
            .Property(i => i.TaxAmount)
            .HasColumnName("KDV")
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Invoice>()
            .Property(i => i.FinalAmount)
            .HasColumnName("NetTutar")
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Invoice>()
            .Property(i => i.TicketId)
            .HasColumnName("KayitId");
        modelBuilder.Entity<Invoice>()
            .Property(i => i.InvoiceDate)
            .HasColumnName("FaturaTarihi");
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Ticket)
            .WithMany(t => t.Invoices)
            .HasForeignKey(i => i.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PaymentType>()
            .ToTable("OdemeTipleri")
            .Property(p => p.TypeName)
            .HasColumnName("TipAdi")
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Payment>()
            .ToTable("Odemeler")
            .Property(p => p.Amount)
            .HasColumnName("Tutar")
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Payment>()
            .Property(p => p.InvoiceId)
            .HasColumnName("FaturaId");
        modelBuilder.Entity<Payment>()
            .Property(p => p.PaymentTypeId)
            .HasColumnName("OdemeTipiId");
        modelBuilder.Entity<Payment>()
            .Property(p => p.Date)
            .HasColumnName("Tarih");
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Invoice)
            .WithMany(i => i.Payments)
            .HasForeignKey(p => p.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.PaymentType)
            .WithMany(t => t.Payments)
            .HasForeignKey(p => p.PaymentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ActionLog>()
            .ToTable("IslemLoglari")
            .Property(a => a.ActionDescription)
            .HasColumnName("IslemAciklamasi")
            .HasMaxLength(1000)
            .IsRequired();
        modelBuilder.Entity<ActionLog>()
            .Property(a => a.TicketId)
            .HasColumnName("KayitId");
        modelBuilder.Entity<ActionLog>()
            .Property(a => a.UserId)
            .HasColumnName("KullaniciId");
        modelBuilder.Entity<ActionLog>()
            .Property(a => a.Timestamp)
            .HasColumnName("ZamanDamgasi");
        modelBuilder.Entity<ActionLog>()
            .HasOne(a => a.Ticket)
            .WithMany(t => t.ActionLogs)
            .HasForeignKey(a => a.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ActionLog>()
            .HasOne(a => a.User)
            .WithMany(u => u.ActionLogs)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
