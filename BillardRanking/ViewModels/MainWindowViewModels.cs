using BillardRanking.FeService;
using BillardRanking.Models;
using BillardRanking.Views;
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
using System.Windows;
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
        public ICommand IncreaseBallDieCommand { get; }
        public ICommand ReduceBallDieCommand { get; }
        public ICommand AddPlayerCommand { get; }
        public ICommand RemovePlayerCommand { get; }

        #endregion

        public MainWindowViewModels()
        {
            SaveWinCommand = new RelayCommand<Player>(async (player) => await AddWin(player));
            ResetWinsCommand = new RelayCommand<string>(async (playerName) => await ResetPlayerWins(playerName));
            IncreaseBallDieCommand = new RelayCommand<Player>(async (player) => await IncreaseBallDie(player));
            ReduceBallDieCommand = new RelayCommand<Player>(async (player) => await ReduceBallDie(player));
            AddPlayerCommand = new RelayCommand<Player>(async (player) => await AddPlayer());
            RemovePlayerCommand = new RelayCommand<Player>(async (player) => await RemovePlayer());
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
        private async Task AddPlayer()
        {
            var inputDialog = new InputDialog("Nhập tên người chơi mới:");
            if (inputDialog.ShowDialog() == true)
            {
                string name = inputDialog.ResponseText;
                if (!string.IsNullOrWhiteSpace(name) && !Players.Any(p => p.Name == name))
                {
                    var newPlayer = new Player
                    {
                        Name = name,
                        CreatedDate = DateTime.Now
                    };
                    await _apiService.AddPlayer(newPlayer);
                    LoadPlayers();
                }
            }
        }

        private async Task RemovePlayer()
        {
            if (SelectedPlayer != null)
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa {SelectedPlayer.Name}?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    await _apiService.RemovePlayerAsync(SelectedPlayer.Name);
                    LoadPlayers();
                }
            }
        }
        private async Task IncreaseBallDie(Player player)
        {
            if (player == null) return;

            player.ballDie++;
            await _apiService.AddBallAsync(new Player
            {
                Name = player.Name,
                ballDie = 1
            });
            LoadPlayers();

        }
        private async Task ReduceBallDie(Player player)
        {
            if (player == null) return;

            player.ballDie--;
            await _apiService.AddBallAsync(new Player
            {
                Name = player.Name,
                ballDie = -1
            });
            LoadPlayers();
        }
        private async Task ResetPlayerWins(string playerName)
        {
            var player = Players.FirstOrDefault(p => p.Name == playerName);
            if (player != null)
            {
                var result = MessageBox.Show($"Bạn có chắc muốn reset điểm cho {player.Name}?", "Xác nhận",
                 MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (string.IsNullOrEmpty(playerName)) return;

                    await _apiService.ResetWinsAsync(playerName);
                    LoadPlayers();

                }
            }
        }
        private async Task AddWin(Player player)
        {
            if (player == null)
            {
                int count = 0;
                foreach (var p in Players)
                {
                    if (p.TemporaryWins > 0)
                    {
                        await _apiService.AddWinAsync(p.Name, p.TemporaryWins);
                        count++;
                    }
                }
                if (count == 0)
                {
                    return;
                }
                else
                {
                    LoadPlayers();
                    return;
                }
            }
        }
    }
}
