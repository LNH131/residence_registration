using CommunityToolkit.Mvvm.Input;
using Resident.Models;
using Resident.Service;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class CreateNotificationForPoliceViewModel : BaseViewModel
    {
        private readonly INotificationService _notificationService;
        private readonly PrnContext _context;

        public CreateNotificationForPoliceViewModel(INotificationService notificationService, PrnContext context)
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
            // Retrieve all users with role "Police"
            var policeList = _context.Users.Where(u => u.Role == "Police").ToList();
            foreach (var police in policeList)
            {
                await _notificationService.SendNotificationAsync(police.UserId, NotificationMessage);
            }

            MessageBox.Show("Thông báo đã được gửi đến tất cả Police.",
                            "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
