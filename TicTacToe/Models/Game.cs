namespace TicTacToe.Models
{
    public class Game
    {
        private const int FieldSize = 3;

        private readonly Mark[] _items;
        private readonly int _playerCross;
        private readonly int _playerCircle;

        private Mark _currentTurn;
        private Mark _nextTurn;
        private State _currentState;

        public enum State
        {
            InGame,
            WinCross,
            WinCircle,
            Tie
        }

        public Action<Game, State> Finished;

        public Game(int playerCross, int playerCircle)
        {
            _playerCross = playerCross;
            _playerCircle = playerCircle;
            _currentTurn = Mark.Cross;
            _nextTurn = Mark.Circle;
            _items = new Mark[FieldSize * FieldSize];
            _items.Initialize();
            _currentState = State.InGame;
            Guid = Guid.NewGuid();
        }

        public IReadOnlyCollection<Mark> Items => _items;

        public Guid Guid { get; }

        public int MarkToPlayerId(Mark mark)
        {
            if (mark == Mark.Cross)
                return _playerCross;
            else if (mark == Mark.Circle)
                return _playerCircle;
            else
                throw new ArgumentException($"Player cannot play with Mark.None");
        }

        public Mark PlayerIdToMark(int playerId)
        {
            if (playerId == _playerCross)
                return Mark.Cross;
            else if (playerId == _playerCircle)
                return Mark.Circle;
            else
                throw new ArgumentException($"User with id {playerId} does not participate in this game");
        }

        public void MakeMove(Mark mark, int row, int column)
        {
            if (row < 0 || column < 0 || row >= FieldSize || column >= FieldSize)
                throw new ArgumentOutOfRangeException($"Field size is {FieldSize}");
            if (_currentState != State.InGame)
                throw new InvalidOperationException($"Game is finished with result: {_currentState}");
            if (mark != _currentTurn)
                throw new InvalidOperationException("Wrong move");

            var index = ConvertToIndex(row, column);

            if (_items[index] != Mark.None)
                throw new InvalidOperationException("Cell already has a mark");

            _items[index] = mark;
            var state = GetState();

            if (_currentState != state)
            {
                Finished?.Invoke(this, state);
                _currentState = state;
            }
            else
            {
                _currentState = state;
                (_currentTurn, _nextTurn) = (_nextTurn, _currentTurn);
            }
        }

        private static int ConvertToIndex(int row, int column) => FieldSize * row + column;

        private State GetState()
        {
            var empty = 0;
            var crossRows = new int[] { 0, 0, 0 };
            var crossColumns = new int[] { 0, 0, 0 };
            var crossDiagonal = 0;
            var crossCounterDiagonal = 0;
            var circleRows = new int[] { 0, 0, 0 };
            var circleColumns = new int[] { 0, 0, 0 };
            var circleDiagonal = 0;
            var circleCounterDiagonal = 0;

            for (var i = 0; i < FieldSize * FieldSize; i++)
            {
                if (_items[i] == Mark.None)
                {
                    empty++;
                    continue;
                }

                if (_items[i] == Mark.Cross)
                {
                    crossRows[i / FieldSize]++;
                    crossColumns[i % FieldSize]++;
                }
                else
                {
                    circleRows[i / FieldSize]++;
                    circleColumns[i % FieldSize]++;
                }
            }

            for (var i = 0; i < FieldSize * FieldSize; i += FieldSize + 1)
            {
                if (_items[i] == Mark.None) continue;

                if (_items[i] == Mark.Cross)
                {
                    crossDiagonal++;
                }
                else
                {
                    circleDiagonal++;
                }
            }

            for (var i = FieldSize - 1; i <= FieldSize * (FieldSize - 1); i += FieldSize - 1)
            {
                if (_items[i] == Mark.None) continue;

                if (_items[i] == Mark.Cross)
                {
                    crossCounterDiagonal++;
                }
                else
                {
                    circleCounterDiagonal++;
                }
            }

            if (crossDiagonal == FieldSize ||
                crossCounterDiagonal == FieldSize ||
                crossRows.Any(r => r == FieldSize) ||
                crossColumns.Any(c => c == FieldSize))
            {
                return State.WinCross;
            }

            if (circleDiagonal == FieldSize ||
                circleCounterDiagonal == FieldSize ||
                circleRows.Any(r => r == FieldSize) ||
                circleColumns.Any(c => c == FieldSize))
            {
                return State.WinCircle;
            }

            if (empty == 0)
            {
                return State.Tie;
            }

            return State.InGame;
        }
    }
}