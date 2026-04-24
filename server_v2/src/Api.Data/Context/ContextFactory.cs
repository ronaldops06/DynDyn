using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<SomniaContext>
    {
        public SomniaContext CreateDbContext(string[] args)
        {
            var connectionString = "Host=aws-0-sa-east-1.pooler.supabase.com;Username=postgres.ldmjuumtcoaslaenmasw;Password=!4JVFrejvHKNZyz;Database=postgres";
            var optionsBuilder = new DbContextOptionsBuilder<SomniaContext>();
            optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            return new SomniaContext(optionsBuilder.Options);
        }
    }
}
