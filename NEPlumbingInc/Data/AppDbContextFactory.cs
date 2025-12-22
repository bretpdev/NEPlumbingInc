using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NEPlumbingInc.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Use SQL Server for migrations (change this to your actual connection string)
        optionsBuilder.UseSqlServer("Server=.;Database=NEPlumbingIncDB;Integrated Security=true;TrustServerCertificate=true;");
        
        return new AppDbContext(optionsBuilder.Options);
    }
}
