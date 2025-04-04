using BillardRanking.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace BillardRanking.FeService
{
    public class ApiService
    {
        private const string UserNameKey = "UserName";
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new System.Uri("https://localhost:7162/api/Players/") };

        public async Task<List<Player>> GetPlayersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Player>>("");
        }
        public async Task <Player> GetPlayerAsync(string playerName)
        {
            return await _httpClient.GetFromJsonAsync<Player>("name");
        }

        public async Task AddWinAsync(string name, int win)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { Name = name, Wins = win }), Encoding.UTF8, "application/json");

            await _httpClient.PostAsync($"AddWins", content);
        }
        public async Task AddPlayer(Player player)
        {
            await _httpClient.PostAsync($"Create", null);
        }
        public async Task ResetWinsAsync(string playerName)
        {
            var response = await _httpClient.PostAsJsonAsync($"resetWins", playerName);
            response.EnsureSuccessStatusCode();
        }


        public string GetSavedUserName()
        {
            return Properties.Settings.Default[UserNameKey]?.ToString();
        }

        public void SaveUserName(string userName)
        {
            Properties.Settings.Default[UserNameKey] = userName;
            Properties.Settings.Default.Save();
        }

        public bool IsFirstTimeLogin()
        {
            return string.IsNullOrEmpty(GetSavedUserName());
        }
    }
}
