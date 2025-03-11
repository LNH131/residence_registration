using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Project.DAO;
using Project.Enums;
using Project.View;
using System.Collections.ObjectModel;
using System.Windows;

namespace Project.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly UserDAO _userDAO;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private Role selectedRole;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public ObservableCollection<Role> Roles { get; } =
            new ObservableCollection<Role>(System.Enum.GetValues(typeof(Role)).Cast<Role>());

        public IAsyncRelayCommand LoginCommand { get; }
        public IAsyncRelayCommand RegisterCommand { get; }

        public LoginViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _userDAO = _serviceProvider.GetRequiredService<UserDAO>();

            // Đặt mặc định là Citizen
            SelectedRole = Role.Citizen;

            LoginCommand = new AsyncRelayCommand(LoginAsync);
            RegisterCommand = new AsyncRelayCommand(RegisterAsync);
        }

        private async Task LoginAsync()
        {
            try
            {
                ErrorMessage = string.Empty;

                var user = await _userDAO.AuthenticateUser(Email, Password, SelectedRole);
                if (user != null)
                {
                    Window nextWindow = null;
                    switch (user.Role)
                    {
                        case Role.Admin:
                            nextWindow = _serviceProvider.GetRequiredService<AdminWindow>();
                            break;
                        case Role.Police:
                            nextWindow = _serviceProvider.GetRequiredService<PoliceWindow>();
                            break;
                        case Role.AreaLeader:
                            nextWindow = _serviceProvider.GetRequiredService<AreaLeaderWindow>();
                            break;
                        case Role.Citizen:
                            nextWindow = _serviceProvider.GetRequiredService<CitizenWindow>();
                            break;
                    }

                    if (nextWindow != null)
                    {
                        nextWindow.Show();
                        // Đóng LoginWindow hiện tại thông qua DI (tìm theo kiểu)
                        var loginWindow = Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault();
                        loginWindow?.Close();
                    }
                }
                else
                {
                    ErrorMessage = "Invalid email, password, or role.";
                }
            }
            catch (System.Exception ex)
            {
                ErrorMessage = "An error occurred during login: " + ex.Message;
            }
        }

        private async Task RegisterAsync()
        {
            var currentWindow = Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault();
            var registerWindow = _serviceProvider.GetRequiredService<Register>();
            registerWindow.Show();
            currentWindow?.Close();
            await Task.CompletedTask;
        }
    }
}
