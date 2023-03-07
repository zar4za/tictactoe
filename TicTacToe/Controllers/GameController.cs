using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("create")]
        public IActionResult CreateGame(int hostId, int contenderId)
        {
            try
            {
                var guid = _gameService.CreateGame(hostId, contenderId);
                return Ok(new
                {
                    success = true,
                    response = new
                    {
                        guid
                    }
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    success = false,
                    message = e.Message
                });
            }
        }

        [HttpPost("move")]
        public IActionResult MakeMove(Guid guid, int playerId, int row, int column)
        {
            try
            {
                _gameService.MakeMove(guid, playerId, row, column);
                return Ok(new
                {
                    success = true
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    success = false,
                    message = e.Message
                });
            }
        }

        [HttpGet("field")]
        public IActionResult GetField(Guid guid)
        {
            try
            {
                var field = _gameService.GetField(guid);
                var winner = _gameService.GetWinner(guid);

                var response = new
                {
                    field,
                    winner
                };

                return Ok(new
                {
                    success = true,
                    response
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    success = false,
                    message = e.Message
                });
            }
        }
    }
}
