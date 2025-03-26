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
    public class PoliceNotificationViewModel : ObservableObject
    {
        private readonly PrnContext _context;
        private readonly ICurrentUserService _currentUserService;

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
                // Let the MarkAsReadCommand re-check if it can execute
                (MarkAsReadCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public ICommand LoadNotificationsCommand { get; }
        public ICommand MarkAsReadCommand { get; }
        public ICommand CloseWindowCommand { get; }

        /// <summary>
        /// Action to close the window, assigned from code-behind if desired.
        /// </summary>
        public System.Action CloseAction { get; set; }

        public PoliceNotificationViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _context = new PrnContext(); // or inject via DI

            Notifications = new ObservableCollection<Notification>();

            LoadNotificationsCommand = new RelayCommand(async () => await LoadNotificationsAsync());
            MarkAsReadCommand = new RelayCommand(async () => await MarkSelectedNotificationAsReadAsync(),
                                                 () => SelectedNotification != null);
            CloseWindowCommand = new RelayCommand(() => CloseAction?.Invoke());

            // Optionally auto-load on creation
            _ = LoadNotificationsAsync();
        }

        private async Task LoadNotificationsAsync()
        {
            try
            {
                // Clear current list
                Notifications.Clear();

                int policeUserId = _currentUserService.CurrentUser.UserId;

                // Load all notifications for this police user
                var policeNotifications = await _context.Notifications
                    .Where(n => n.UserId == policeUserId)
                    .OrderByDescending(n => n.SentDate)
                    .ToListAsync();

                foreach (var notif in policeNotifications)
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
            if (SelectedNotification == null) return;

            try
            {
                SelectedNotification.IsRead = true;
                _context.Notifications.Update(SelectedNotification);
                await _context.SaveChangesAsync();

                // Optionally refresh the list
                // or simply update UI in-place
                MessageBox.Show("Notification marked as read.",
                                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error marking notification as read: " + ex.Message,
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
