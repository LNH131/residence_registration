using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class CitizenNotificationViewModel : ObservableObject
    {
        private readonly PrnContext _context;
        private readonly ICurrentUserService _currentUserService;

        public ObservableCollection<Notification> Notifications { get; set; } = new ObservableCollection<Notification>();

        private Notification? _selectedNotification;
        public Notification? SelectedNotification
        {
            get => _selectedNotification;
            set { SetProperty(ref _selectedNotification, value); }
        }

        public ICommand MarkAsReadCommand { get; }
        public ICommand CloseCommand { get; }

        // Action để đóng cửa sổ, được thiết lập từ code-behind.
        public Action? CloseAction { get; set; }

        public CitizenNotificationViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _context = new PrnContext();

            MarkAsReadCommand = new AsyncRelayCommand(MarkAsReadAsync, CanMarkAsRead);
            CloseCommand = new RelayCommand(() => CloseAction?.Invoke());

            LoadNotificationsAsync();
        }

        private bool CanMarkAsRead() => SelectedNotification != null && SelectedNotification.IsRead != true;

        private async Task LoadNotificationsAsync()
        {
            try
            {
                if (_currentUserService.CurrentUser == null)
                {
                    MessageBox.Show("Chưa có thông tin người dùng. Vui lòng đăng nhập lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int currentUserId = _currentUserService.CurrentUser.UserId;
                var notifications = await _context.Notifications
                    .Where(n => n.UserId == currentUserId)
                    .OrderByDescending(n => n.SentDate)
                    .ToListAsync();

                Notifications = new ObservableCollection<Notification>(notifications);
                OnPropertyChanged(nameof(Notifications));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông báo: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task MarkAsReadAsync()
        {
            if (SelectedNotification == null)
                return;

            try
            {
                SelectedNotification.IsRead = true;
                _context.Notifications.Update(SelectedNotification);
                await _context.SaveChangesAsync();
                MessageBox.Show("Thông báo đã được đánh dấu đã đọc.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadNotificationsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
