using Microsoft.EntityFrameworkCore;
using TicTacToe.Entities;

namespace TicTacToe.Services
{
    public class GameResultRepository : IGameResultRepository
    {
        private readonly GameResultContext _context;

        public GameResultRepository(GameResultContext context)
        {
            _context = context;
        }   

        public void AddResult(GameResult result)
        {
            _context.GameResults.Add(result);
            _context.SaveChanges();
        }

        public GameResult FindGame(Guid guid)
        {
            return _context.GameResults
                .AsNoTracking()
                .Where(x => x.GameId == guid)
                .Single();
        }

        public IEnumerable<GameResult> GetResults(int count, int offset)
        {
            return _context.GameResults
                .AsNoTracking()
                .Skip(offset)
                .Take(count);
        }
    }
}
