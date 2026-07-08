using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ClinicFlow.Infrastructure.Persistence;

// Used by dotnet-ef CLI at design time to create migrations
public class ClinicFlowDbContextFactory : IDesignTimeDbContextFactory<ClinicFlowDbContext>
{
    public ClinicFlowDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ClinicFlowDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ClinicFlowDb;Trusted_Connection=True;")
            .Options;

        return new ClinicFlowDbContext(options);
    }
}
