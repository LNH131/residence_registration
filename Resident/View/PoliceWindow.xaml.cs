using Microsoft.Extensions.DependencyInjection;
using Resident.Enums;
using Resident.Service;
using Resident.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
namespace Resident.View
{
    public partial class PoliceWindow : Window
    {
        public PoliceWindow(ICurrentUserService currentUserService)
        {
            InitializeComponent();
            DataContext = new PoliceViewModel(currentUserService);

            this.btnLogout.Click += this.Logout_Click;

        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var loginWindow = serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
            this.Close();
        }
        public ObservableCollection<Role> Roles { get; } = new ObservableCollection<Role>(Enum.GetValues(typeof(Role)).Cast<Role>());

    }
}