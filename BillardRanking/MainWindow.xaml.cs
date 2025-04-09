using BillardRanking.FeService;
using BillardRanking.ViewModels;
using BillardRanking.Views;
using System.Windows;
using System.Windows.Controls;

namespace BillardRanking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModels();

        }
  
    }
}
