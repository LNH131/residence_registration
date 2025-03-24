using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Resident.DAO;
using Resident.Enums;
using Resident.Service;
using Resident.View;
using System.Collections.ObjectModel;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Resident.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly UserDAO _userDAO;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrentUserService _currentUserService;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string selectedRole;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public ObservableCollection<Role> Roles { get; } =
            new ObservableCollection<Role>(System.Enum.GetValues(typeof(Role)).Cast<Role>());

        public IAsyncRelayCommand LoginCommand { get; }
        public IAsyncRelayCommand RegisterCommand { get; }

        public LoginViewModel(IServiceProvider serviceProvider, ICurrentUserService currentUserService)
        {
            _serviceProvider = serviceProvider;
            _currentUserService = currentUserService;
            _userDAO = _serviceProvider.GetRequiredService<UserDAO>();

            // Đặt mặc định là Citizen
            SelectedRole = Role.Citizen.ToString();

            LoginCommand = new AsyncRelayCommand(LoginAsync);
            RegisterCommand = new AsyncRelayCommand(RegisterAsync);
        }

        private async Task LoginAsync()
        {
            try
            {
                ErrorMessage = string.Empty;
                OnPropertyChanged(nameof(ErrorMessage));

                var user = await _userDAO.AuthenticateUser(Email, Password, SelectedRole);
                if (user != null)
                {
                    Window nextWindow = null;
                    switch (user.Role)
                    {
                        case "Admin":
                            _currentUserService.CurrentUser = user;
                            nextWindow = _serviceProvider.GetRequiredService<AdminWindow>();
                            break;
                        case "Police":
                            _currentUserService.CurrentUser = user;
                            nextWindow = _serviceProvider.GetRequiredService<PoliceWindow>();
                            break;
                        case "AreaLeader":
                            _currentUserService.CurrentUser = user;
                            nextWindow = _serviceProvider.GetRequiredService<AreaLeaderWindow>();
                            break;
                        case "Citizen":
                            _currentUserService.CurrentUser = user;
                            var citizenViewModel = _serviceProvider.GetRequiredService<CitizenViewModel>();
                            var citizenWindow = _serviceProvider.GetRequiredService<CitizenWindow>();
                            citizenWindow.DataContext = citizenViewModel;
                            nextWindow = citizenWindow;
                            break;
                    }

                    if (nextWindow != null)
                    {
                        nextWindow.Show();
                        var loginWindow = Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault();
                        loginWindow?.Close();
                    }
                }
                else
                {
                    // Nếu đăng nhập sai thì hiển thị MessageBox.
                    MessageBox.Show("Invalid email, password, or role.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ErrorMessage = "Invalid email, password, or role.";
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("An error occurred during login: " + ex.Message, "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ErrorMessage = "An error occurred during login: " + ex.Message;
                OnPropertyChanged(nameof(ErrorMessage));
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
