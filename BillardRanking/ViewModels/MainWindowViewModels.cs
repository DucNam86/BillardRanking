using BillardRanking.FeService;
using BillardRanking.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BillardRanking.ViewModels
{
    public class MainWindowViewModels : ObservableObject
    {
        #region entities

        private readonly ApiService _apiService = new ApiService();
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        public ObservableCollection<Monthly> GroupedStatistics { get; set; } = new ObservableCollection<Monthly>();
        private Player _selectedPlayer;
        public Player SelectedPlayer
        {
            get => _selectedPlayer;
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        #region command
        public ICommand SaveWinCommand { get; }
        public ICommand ResetWinsCommand { get; }
        #endregion

        public MainWindowViewModels()
        {
            SaveWinCommand = new RelayCommand<Player>(async (player) => await AddWin(player));
            ResetWinsCommand = new RelayCommand<string>(async (playerName) => await ResetPlayerWins(playerName));
            LoadPlayers();
        }

        private async void LoadPlayers()
        {
            var players = await _apiService.GetPlayersAsync();
            var grouped = players
                   .Where(p => p.CreatedDate != DateTime.MinValue) 
                   .GroupBy(p => p.CreatedDate.ToString("yyyy-MM")) 
                   .OrderByDescending(g => g.Key); Players.Clear();
            GroupedStatistics.Clear();

            foreach (var group in grouped)
            {
                var sortedPlayers = group.OrderByDescending(p => p.Wins).ToList();
                int rank = 1;
                int previousWins = -1;
                int countSameRank = 0;

                foreach (var player in sortedPlayers)
                {
                    if (player.Wins != previousWins)
                    {
                        rank += countSameRank;
                        countSameRank = 1;
                    }
                    else
                    {
                        countSameRank++;
                    }
                    player.Rank = rank;
                    player.TemporaryWins = player.Wins;
                    Players.Add(player);

                    previousWins = player.Wins;
                    player.TemporaryWins = 0;
                }
                GroupedStatistics.Add(new Monthly
                {
                    Month = DateTime.ParseExact(group.Key, "yyyy-MM", null).ToString("MMMM yyyy"),
                    Players = new ObservableCollection<Player>(sortedPlayers)
                });
            }
        }
        private async Task ResetPlayerWins(string playerName)
        {
            if (string.IsNullOrEmpty(playerName)) return;

            await _apiService.ResetWinsAsync(playerName);
            LoadPlayers();
        }
        private async Task AddWin(Player player)
        {
            if (player == null)
            {
                if (_selectedPlayer != null)
                {
                    player = _selectedPlayer;
                }
                else
                {
                    return;
                }
            }
            player.Wins = player.TemporaryWins;
            await _apiService.AddWinAsync(player.Name, player.Wins);
            LoadPlayers();
        }
    }
}
