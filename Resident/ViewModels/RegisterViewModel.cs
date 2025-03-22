using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Resident.Enums;
using Resident.Models;
using Resident.View;
using System.Collections.ObjectModel;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Resident.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly PrnContext _context;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private string fullName = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

        [ObservableProperty]
        private Role selectedRole;

        [ObservableProperty]
        private Area selectedArea;

        [ObservableProperty]
        private Address selectedAddress;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public ObservableCollection<Role> Roles { get; }
        public ObservableCollection<Area> Areas { get; }
        public ObservableCollection<Address> Addresses { get; }

        public IAsyncRelayCommand RegisterCommand { get; }
        public IRelayCommand BackCommand { get; }

        public RegisterViewModel(PrnContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;

            Roles = new ObservableCollection<Role>(Enum.GetValues(typeof(Role)).Cast<Role>());
            SelectedRole = Roles.FirstOrDefault();

            Areas = new ObservableCollection<Area>(_context.Areas.ToList());
            SelectedArea = Areas.FirstOrDefault();

            Addresses = new ObservableCollection<Address>(_context.Addresses.ToList());
            SelectedAddress = Addresses.FirstOrDefault();

            RegisterCommand = new AsyncRelayCommand(RegisterAsync);
            BackCommand = new RelayCommand(GoBack);
        }

        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ErrorMessage = "Please fill in all required fields.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            if (await _context.Users.AnyAsync(u => u.Email == Email))
            {
                ErrorMessage = "Email is already registered.";
                return;
            }

            if (SelectedAddress == null)
            {
                ErrorMessage = "Please select a current address.";
                return;
            }

            var newUser = new User
            {
                FullName = FullName,
                Email = Email,
                Password = BCrypt.Net.BCrypt.HashPassword(Password),
                Role = SelectedRole,
                AreaId = SelectedArea?.AreaId,
                CurrentAddressId = SelectedAddress.AddressId,
                CurrentAddress = SelectedAddress
            };

            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
                loginWindow.Show();
                Application.Current.Windows.OfType<Resident.View.Register>().FirstOrDefault()?.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Registration failed: " + (ex.InnerException?.Message ?? ex.Message);
            }
        }

        private void GoBack()
        {
            var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
            Application.Current.Windows.OfType<Resident.View.Register>().FirstOrDefault()?.Close();
        }
    }
}
