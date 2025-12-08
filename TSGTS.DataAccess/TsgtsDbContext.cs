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
            .Property(r => r.RoleName)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .HasMaxLength(256)
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Customer>()
            .Property(c => c.FirstName)
            .HasMaxLength(75)
            .IsRequired();
        modelBuilder.Entity<Customer>()
            .Property(c => c.LastName)
            .HasMaxLength(75)
            .IsRequired();
        modelBuilder.Entity<Customer>()
            .Property(c => c.Phone)
            .HasMaxLength(20)
            .IsRequired();
        modelBuilder.Entity<Customer>()
            .Property(c => c.Email)
            .HasMaxLength(100);
        modelBuilder.Entity<Customer>()
            .Property(c => c.TaxNo)
            .HasMaxLength(20);
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.TaxNo)
            .IsUnique()
            .HasFilter("[TaxNo] IS NOT NULL");

        modelBuilder.Entity<Brand>()
            .Property(b => b.BrandName)
            .HasMaxLength(75)
            .IsRequired();

        modelBuilder.Entity<Category>()
            .Property(c => c.CategoryName)
            .HasMaxLength(75)
            .IsRequired();

        modelBuilder.Entity<Model>()
            .Property(m => m.ModelName)
            .HasMaxLength(100)
            .IsRequired();
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
            .Property(d => d.SerialNumber)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<Device>()
            .HasIndex(d => d.SerialNumber)
            .IsUnique();
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
            .Property(s => s.StatusName)
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<TicketStatus>()
            .Property(s => s.ColorCode)
            .HasMaxLength(7);

        modelBuilder.Entity<ServiceTicket>()
            .Property(t => t.Description)
            .HasMaxLength(1000);
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
            .Property(p => p.PartName)
            .HasMaxLength(150)
            .IsRequired();
        modelBuilder.Entity<SparePart>()
            .Property(p => p.PartCode)
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<SparePart>()
            .HasIndex(p => p.PartCode)
            .IsUnique();
        modelBuilder.Entity<SparePart>()
            .Property(p => p.UnitPrice)
            .HasColumnType("decimal(18,2)");

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
            .Property(l => l.ServiceType)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<LaborRate>()
            .Property(l => l.HourlyRate)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Invoice>()
            .Property(i => i.TotalAmount)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Invoice>()
            .Property(i => i.Discount)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Invoice>()
            .Property(i => i.TaxAmount)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Invoice>()
            .Property(i => i.FinalAmount)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Ticket)
            .WithMany(t => t.Invoices)
            .HasForeignKey(i => i.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PaymentType>()
            .Property(p => p.TypeName)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasColumnType("decimal(18,2)");
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
            .Property(a => a.ActionDescription)
            .HasMaxLength(1000)
            .IsRequired();
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
