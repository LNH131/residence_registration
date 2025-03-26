using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Resident.View
{
    public partial class PoliceWindow : Window
    {
        public PoliceWindow(PoliceViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var loginWindow = serviceProvider.GetRequiredService<LoginWindow>();
            Application.Current.MainWindow.Close();
            loginWindow.Show();
            this.Close();
        }
    }
}
