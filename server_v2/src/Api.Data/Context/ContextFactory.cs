using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<SomniaContext>
    {
        public SomniaContext CreateDbContext(string[] args)
        {
            var connectionString = "";
            var optionsBuilder = new DbContextOptionsBuilder<SomniaContext>();
            optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            return new SomniaContext(optionsBuilder.Options);
        }
    }
}
