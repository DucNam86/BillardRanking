using System.Windows;

namespace BillardRanking.Views
{
    /// <summary>
    /// Interaction logic for NameInputDialog.xaml
    /// </summary>
    public partial class NameInputDialog : Window
    {
        public string Name { get; private set; }

        public NameInputDialog()
        {
            InitializeComponent();
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            var userName = txtUserName.Text;
            Properties.Settings.Default["UserName"] = userName;
            Properties.Settings.Default.Save();
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
           
        }
    }
}
