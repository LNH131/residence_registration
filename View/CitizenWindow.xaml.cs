using Microsoft.Extensions.DependencyInjection;
using Project.DAO;
using Project.Enums;
using Project.Models;
using System.Collections.ObjectModel;
using System.Windows;
namespace Project.View
{
    public partial class CitizenWindow : Window
    {
        private readonly UserDAO _userDAO;
        private readonly User _currentUser;
        public CitizenWindow()
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

        private void UpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            var updateProfileWindow = new UpdateCitizenProfileWindow(_userDAO, _currentUser);
            updateProfileWindow.ShowDialog();
        }

        private void ManageHousehold_Click(object sender, RoutedEventArgs e)
        {
            var registerHouseholdWindow = new HouseHoldControlWindow(_currentUser, _userDAO);
            registerHouseholdWindow.ShowDialog();
        }

        public ObservableCollection<Role> Roles { get; } = new ObservableCollection<Role>(Enum.GetValues(typeof(Role)).Cast<Role>());
    }
}