using TicTacToe.Entities;

namespace TicTacToe.Services
{
    public interface IGameResultRepository
    {
        void AddResult(GameResult result);
        IEnumerable<GameResult> GetResults(int count, int offset);

        GameResult FindGame(Guid guid);
    }
}
