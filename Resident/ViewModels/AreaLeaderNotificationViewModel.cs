using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class AreaLeaderNotificationViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;

        public AreaLeaderNotificationViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
            SendCommand = new AsyncRelayCommand(SendNotificationAsync, CanSendNotification);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke());
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set
            {
                SetProperty(ref _message, value);
                ((AsyncRelayCommand)SendCommand).NotifyCanExecuteChanged();
            }
        }

        // Command gửi thông báo
        public ICommand SendCommand { get; }
        // Command hủy
        public ICommand CancelCommand { get; }

        // Action được gọi khi đóng cửa sổ
        public Action? CloseAction { get; set; }

        private bool CanSendNotification() => !string.IsNullOrWhiteSpace(Message);

        private async Task SendNotificationAsync()
        {
            try
            {
                // Tạo context để truy vấn danh sách Citizen.
                using var context = new PrnContext();
                var citizens = await context.Users
                    .Where(u => u.Role == "Citizen")
                    .ToListAsync();

                foreach (var citizen in citizens)
                {
                    await _notificationService.SendInAppNotificationAsync(citizen.UserId, Message);
                }
                MessageBox.Show("Thông báo đã được gửi đến tất cả cư dân.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi thông báo: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
