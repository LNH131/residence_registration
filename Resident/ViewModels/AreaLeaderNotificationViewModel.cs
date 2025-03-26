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
    public class AreaLeaderNotificationViewModel : ObservableObject
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly PrnContext _context;

        public AreaLeaderNotificationViewModel(ICurrentUserService currentUserService, PrnContext context)
        {
            _currentUserService = currentUserService;
            _context = context;
            LoadNotificationsCommand = new AsyncRelayCommand(LoadNotificationsAsync);
            MarkAsReadCommand = new AsyncRelayCommand(MarkSelectedNotificationAsReadAsync, () => SelectedNotification != null);
            RefreshCommand = new AsyncRelayCommand(LoadNotificationsAsync);
        }

        private ObservableCollection<Notification> _notifications;
        public ObservableCollection<Notification> Notifications
        {
            get => _notifications;
            set => SetProperty(ref _notifications, value);
        }

        private Notification _selectedNotification;
        public Notification SelectedNotification
        {
            get => _selectedNotification;
            set
            {
                SetProperty(ref _selectedNotification, value);
                (MarkAsReadCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public ICommand LoadNotificationsCommand { get; }
        public ICommand MarkAsReadCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand CloseCommand { get; set; } // Set in code-behind.

        private async Task LoadNotificationsAsync()
        {
            try
            {
                // Clear current list
                Notifications.Clear();

                int arealeader = _currentUserService.CurrentUser.UserId;

                var arealeaderNotifications = await _context.Notifications
                    .Where(n => n.UserId == arealeader)
                    .OrderByDescending(n => n.SentDate)
                    .ToListAsync();

                foreach (var notif in arealeaderNotifications)
                {
                    Notifications.Add(notif);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error loading notifications: " + ex.Message,
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task MarkSelectedNotificationAsReadAsync()
        {
            if (SelectedNotification == null)
                return;

            try
            {
                SelectedNotification.IsRead = true;
                _context.Notifications.Update(SelectedNotification);
                await _context.SaveChangesAsync();
                await LoadNotificationsAsync();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error marking notification as read: " + ex.Message, "Error");
            }
        }
    }
}
