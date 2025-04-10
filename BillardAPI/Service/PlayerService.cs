using BillardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BillardAPI.Service
{
    public class PlayerService
    {
        private readonly AppDbContext _context;

        public PlayerService(AppDbContext context)
        {
            _context = context;
        }

        public List<Player> GetPlayers() => _context.Players.OrderByDescending(p => p.Wins).ToList();

        public void UpdateWin(string playerName, int win)
        {
            var player = _context.Players.FirstOrDefault(p => p.Name == playerName);
            if (player != null)
            {
                player.Wins += win;
                _context.SaveChanges();
            }
        }       
        public bool UpdateBall(string playerName, int ball)
        {
            var player = _context.Players.FirstOrDefault(p => p.Name == playerName);
            if (player == null)
                return false;

            player.ballDie = Math.Max(0, player.ballDie + ball);
            _context.SaveChanges();
            return true;
        }
        public async Task<Player> CreatePlayerAsync(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return player;
        }
        public async Task DeletePlayerAsync(string playerName)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Name == playerName);
            if (player != null)
            {
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
            }
        }
        public async Task ResetPlayerWinsAsync(string playerName)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var player = await _context.Players
                       .Where(p => p.Name == playerName && p.CreatedDate.Month == currentMonth && p.CreatedDate.Year == currentYear)
                       .FirstOrDefaultAsync();
            if (player != null)
            {
                player.Wins = 0; 
                await _context.SaveChangesAsync();
            }
        }
        public Player GetPlayerByName(string name)
        {
            return _context.Players
                           .Where(p => p.Name.ToLower() == name.ToLower())
                           .FirstOrDefault();
        }


    }
}
