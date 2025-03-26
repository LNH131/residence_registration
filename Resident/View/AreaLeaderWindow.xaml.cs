using Microsoft.Extensions.DependencyInjection;
using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class AreaLeaderWindow : Window
    {
        public AreaLeaderWindow(AreaLeaderViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var loginWindow = serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
            this.Close();
        }
    }
}
