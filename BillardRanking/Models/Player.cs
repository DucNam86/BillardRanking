using BillardRanking.FeService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillardRanking.Models
{
    public class Player : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new ApiService();

        public int Id { get; set; }
        public int ballDie { get; set; } = 0;
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Rank { get; set; }
        public DateTime CreatedDate { get; set; }

        private int _temporaryWins;
        public int TemporaryWins
        {
            get => _temporaryWins;
            set
            {
                _temporaryWins = value;
                OnPropertyChanged(nameof(TemporaryWins));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool IsCurrentUser => Name == new ApiService().GetSavedUserName();
    }
}
