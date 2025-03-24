using Microsoft.Extensions.DependencyInjection;
using Resident.Enums;
using Resident.Service;
using Resident.Services; // For IPoliceProcessingService
using System.Collections.ObjectModel;
using System.Windows;

namespace Resident.View
{
    public partial class PoliceWindow : Window
    {
        public PoliceWindow()
        {
            InitializeComponent();

            // Resolve any additional services you need from the DI container
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var policeProcessingService = serviceProvider.GetRequiredService<IPoliceProcessingService>();

            // Now create the PoliceViewModel using both services
            DataContext = new PoliceViewModel(currentUserService, policeProcessingService);

            this.btnLogout.Click += Logout_Click;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var loginWindow = serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
            this.Close();
        }

        public ObservableCollection<Role> Roles { get; }
            = new ObservableCollection<Role>(Enum.GetValues(typeof(Role)).Cast<Role>());
    }
}
