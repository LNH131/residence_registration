using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Project.View
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