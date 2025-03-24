using Microsoft.Extensions.DependencyInjection;
using Resident.Service;
using System.Windows;

namespace Resident.View
{
    public partial class AreaLeaderWindow : Window
    {
        public AreaLeaderWindow(ICurrentUserService currentUserService, INotificationService notificationService)
        {
            InitializeComponent();
            DataContext = new AreaLeaderViewModel(currentUserService, notificationService);
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