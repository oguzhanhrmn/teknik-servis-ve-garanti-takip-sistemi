using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TSGTS.DataAccess;

public class TsgtsDbContextFactory : IDesignTimeDbContextFactory<TsgtsDbContext>
{
    public TsgtsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TsgtsDbContext>();

        var connectionString = Environment.GetEnvironmentVariable("TSGTS_ConnectionString") ??
                               "Server=localhost\\SQLEXPRESS01;Database=TSGTS;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

        optionsBuilder.UseSqlServer(connectionString);

        return new TsgtsDbContext(optionsBuilder.Options);
    }
}
