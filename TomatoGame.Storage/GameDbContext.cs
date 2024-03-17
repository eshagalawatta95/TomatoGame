using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
