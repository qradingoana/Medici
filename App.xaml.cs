using Medici.Data;
using Medici.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Medici
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Create an instance of DatabaseHelper
            DatabaseHelper dbHelper = DatabaseHelper.GetInstance();
            dbHelper.InitializeDatabase(); // Call the InitializeDatabase method
        }
    }

}
