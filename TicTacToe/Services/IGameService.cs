using TicTacToe.Models;

namespace TicTacToe.Services
{
    public interface IGameService
    {
        Guid CreateGame(int crossPlayerId, int circlePlayerId);

        IReadOnlyCollection<Mark>? GetField(Guid guid);

        void MakeMove(Guid guid, int playerid, int row, int column);
        
        int? GetWinner(Guid guid);
    }
}
