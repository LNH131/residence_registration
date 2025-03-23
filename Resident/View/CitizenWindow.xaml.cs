using Microsoft.Extensions.DependencyInjection;
using Resident.DAO;
using Resident.Enums;
using Resident.Models;
using Resident.ViewModels;
using Resident.DAO;
using Resident.Enums;
using Resident.View;
using Resident;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
namespace Resident.View
{
    public partial class CitizenWindow : Window
    {
        private readonly User _currentUser;
        private readonly CitizenViewModel _viewModel;
        public CitizenWindow(CitizenViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _currentUser = _viewModel.CurrentUser;
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