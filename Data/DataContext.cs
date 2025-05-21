
namespace dotnet_rpg.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<Character> Characters => Set<Character>();
        public DbSet<User> Users => Set<User>();

    }
}
