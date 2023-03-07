using TicTacToe.Entities;
using TicTacToe.Models;

namespace TicTacToe.Services
{
    public class GameService : IGameService
    {
        private readonly List<Game> _activeGames;
        private readonly IServiceScopeFactory _scopeFactory;

        public GameService(IServiceScopeFactory factory)
        {
            _activeGames = new List<Game>();
            _scopeFactory = factory;
        }

        public int? GetWinner(Guid guid)
        {
            using var scope = _scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IGameResultRepository>().FindGame(guid).WinnerId;
        }

        public Guid CreateGame(int crossPlayerId, int circlePlayerId)
        {
            var game = new Game(crossPlayerId, circlePlayerId);
            game.Finished += OnGameFinished;
            _activeGames.Add(game);
            return game.Guid;
        }

        public IReadOnlyCollection<Mark>? GetField(Guid guid)
        {
            try
            {
                return _activeGames.FirstOrDefault(x => x.Guid == guid)?.Items;
            }
            catch
            {
                return null;
            }
        }

        public void MakeMove(Guid guid, int playerid, int row, int column)
        {
            var game = _activeGames.First(x => x.Guid == guid);
            var mark = game.PlayerIdToMark(playerid);
            game.MakeMove(mark, row, column);
        }

        private void OnGameFinished(Game game, Game.State state)
        {
            _activeGames.Remove(game);
            int? winner = null;

            if (state == Game.State.WinCross)
                winner = game.MarkToPlayerId(Mark.Cross);
            else if (state == Game.State.WinCircle)
                winner = game.MarkToPlayerId(Mark.Circle);

            var result = new GameResult()
            {
                GameId = game.Guid,
                WinnerId = winner
            };

            using var scope = _scopeFactory.CreateScope();
            scope.ServiceProvider.GetRequiredService<IGameResultRepository>().AddResult(result);
        }
    }
}
