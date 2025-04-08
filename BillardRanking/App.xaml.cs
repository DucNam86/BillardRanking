using BillardRanking.Views;
using System;
using System.Windows;

namespace BillardRanking
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //BillardRanking.Properties.Settings.Default.UserName = string.Empty;
            //BillardRanking.Properties.Settings.Default.Save();

            if (IsFirstTimeLogin())
            {
                var nameWindow = new NameInputDialog();
                nameWindow.ShowDialog();
            }

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        public bool IsFirstTimeLogin()
        {
            return string.IsNullOrEmpty(BillardRanking.Properties.Settings.Default.UserName);
        }
    }
}
