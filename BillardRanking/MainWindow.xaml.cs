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
        private readonly ApiService _userService = new ApiService();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModels();
            if (_userService.IsFirstTimeLogin())
            {
                var nameDialog = new NameInputDialog();
                if (nameDialog.ShowDialog() == true)
                {
                    _userService.SaveUserName(nameDialog.Name);
                }
                else
                {
                    Close();
                }
            }

            LoadUserData();
        }
        private async void LoadUserData()
        {
            string playerName = _userService.GetSavedUserName();
            var userData = await _userService.GetPlayerAsync(playerName);
            DataContext = userData;
        }
    }
}
