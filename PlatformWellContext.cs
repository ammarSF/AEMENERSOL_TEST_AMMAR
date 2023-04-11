using AEMENERSOL;
using System.Data.Entity;

public class PlatformWellContext : DbContext
{
    public DbSet<PlatformTable> PlatformTable { get; set; }
    public DbSet<WellTable> WellTable { get; set; }

    public PlatformWellContext() : base("name=PlatformWellDB_")
    {
        Database.SetInitializer(new CreateDatabaseIfNotExists<PlatformWellContext>());
    }
}
