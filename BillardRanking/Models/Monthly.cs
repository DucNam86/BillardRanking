using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillardRanking.Models
{
    public class Monthly
    {
        public string Month { get; set; } 
        public ObservableCollection<Player> Players { get; set; }

    }
}
