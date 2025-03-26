using CommunityToolkit.Mvvm.Input;
using Resident.Models;
using Resident.Service;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class CreateNotificationForCitizensViewModel : BaseViewModel
    {
        private readonly INotificationService _notificationService;
        private readonly PrnContext _context;

        public CreateNotificationForCitizensViewModel(INotificationService notificationService, PrnContext context)
        {
            _notificationService = notificationService;
            _context = context;

            SendNotificationCommand = new AsyncRelayCommand(SendNotificationAsync, CanSendNotification);
        }

        private string _notificationMessage;
        public string NotificationMessage
        {
            get => _notificationMessage;
            set
            {
                _notificationMessage = value;
                OnPropertyChanged();
                (SendNotificationCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public ICommand SendNotificationCommand { get; }

        private bool CanSendNotification()
        {
            // Only enable the command if there's non-empty text.
            return !string.IsNullOrWhiteSpace(NotificationMessage);
        }

        private async Task SendNotificationAsync()
        {
            // Retrieve all users with role "Citizen"
            var citizens = _context.Users.Where(u => u.Role == "Citizen").ToList();
            foreach (var citizen in citizens)
            {
                await _notificationService.SendNotificationAsync(citizen.UserId, NotificationMessage);
            }

            MessageBox.Show("Thông báo đã được gửi đến tất cả Citizens.",
                            "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
