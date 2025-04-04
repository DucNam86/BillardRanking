using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillardRanking.Models
{
    public class Player
    {

        public int Id { get; set; }
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
    }
}
