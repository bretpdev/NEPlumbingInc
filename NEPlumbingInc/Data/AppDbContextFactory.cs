using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NEPlumbingInc.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // For migrations, use a SQL Server connection string
        // This is only used by 'dotnet ef' commands locally
        var connectionString = "Server=.;Database=NEPlumbingIncDB;Integrated Security=true;TrustServerCertificate=true;";
        optionsBuilder.UseSqlServer(connectionString);
        
        return new AppDbContext(optionsBuilder.Options);
    }
}
