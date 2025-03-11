using Microsoft.Extensions.DependencyInjection;
using Project.Enums;
using System.Collections.ObjectModel;
using System.Windows;
namespace Project.View
{
    public partial class CitizenWindow : Window
    {
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

        public ObservableCollection<Role> Roles { get; } = new ObservableCollection<Role>(Enum.GetValues(typeof(Role)).Cast<Role>());
    }
}