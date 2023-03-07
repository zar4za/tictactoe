using Microsoft.EntityFrameworkCore;

namespace TicTacToe.Entities
{
    public class GameResultContext : DbContext
    {
        public DbSet<GameResult> GameResults { get; init; } = null!;

        public GameResultContext(DbContextOptions<GameResultContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
