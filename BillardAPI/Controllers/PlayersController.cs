using BillardAPI.Models;
using BillardAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;

namespace BillardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerService _playerService;

        public PlayersController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public ActionResult<List<Player>> GetPlayers()
        {
            return _playerService.GetPlayers();
        }
        [HttpGet("{playerName}")]
        public ActionResult<Player> GetUserData(string playerName)
        {
            var player = _playerService.GetPlayerByName(playerName);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }
        [HttpPost("AddWins")]
        public IActionResult AddWin([FromBody] Player request)
        {
            _playerService.UpdateWin(request.Name, request.Wins);
            return Ok();
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePlayer([FromBody] Player player)
        {
            if (string.IsNullOrEmpty(player.Name))
            {
                return BadRequest("Tên người chơi không được để trống.");
            }

            var createdPlayer = await _playerService.CreatePlayerAsync(player);
            return Ok(new { message = "Player created successfully!", createdPlayer });
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeletePlayer([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Invalid player name.");
            }

            await _playerService.DeletePlayerAsync(name);
            return NoContent();
        }

        [HttpPost("resetWins")]
        public async Task<IActionResult> ResetWins([FromBody] string playerName)
        {
            if (string.IsNullOrEmpty(playerName))
            {
                return BadRequest("Invalid player name.");
            }

            await _playerService.ResetPlayerWinsAsync(playerName);
            return Ok();
        }
 
    }
}
