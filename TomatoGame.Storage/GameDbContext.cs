using System.Data.Entity;

namespace TomatoGame.Storage
{
    public class GameDbContext : DbContext
    {
        public DbSet<Score> Scores { get; set; }

        public DbSet<GameMode> GameModes { get; set; }

        public DbSet<User> Users { get; set; }

        public GameDbContext() : base("name=GameDbConnection")
        {
        }
    }
}
