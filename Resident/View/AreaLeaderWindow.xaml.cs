using Microsoft.Extensions.DependencyInjection;
using Resident.View;
using Resident;
using System.Windows;

namespace Resident.View
{
    public partial class AreaLeaderWindow : Window
    {
        public AreaLeaderWindow()
        {
            InitializeComponent();
            this.btnLogout.Click += this.Logout_Click;
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