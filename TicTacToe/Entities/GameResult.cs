using System.ComponentModel.DataAnnotations;
using TicTacToe.Models;

namespace TicTacToe.Entities
{
    public class GameResult
    {
        [Key]
        public Guid GameId { get; init; }

        public int? WinnerId { get; init; }
    }
}
