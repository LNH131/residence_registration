using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class CitizenNotificationViewModel : BaseViewModel
    {
        private readonly PrnContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CitizenNotificationViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _context = new PrnContext();

            Notifications = new ObservableCollection<Notification>();
            LoadNotificationsCommand = new LocalRelayCommand(_ => LoadNotifications());
            MarkAsReadCommand = new LocalRelayCommand(_ => MarkAsRead(), _ => SelectedNotification != null);

            LoadNotifications(); // Initial load
        }

        private ObservableCollection<Notification> _notifications;
        public ObservableCollection<Notification> Notifications
        {
            get => _notifications;
            set { _notifications = value; OnPropertyChanged(); }
        }

        private Notification _selectedNotification;
        public Notification SelectedNotification
        {
            get => _selectedNotification;
            set
            {
                _selectedNotification = value;
                OnPropertyChanged();
                // Re-check can-execute for MarkAsRead
                (MarkAsReadCommand as LocalRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand LoadNotificationsCommand { get; }
        public ICommand MarkAsReadCommand { get; }

        private void LoadNotifications()
        {
            Notifications.Clear();

            try
            {
                int currentUserId = _currentUserService.CurrentUser.UserId;

                // Query all notifications for the current user, sorted by date descending
                var userNotifications = _context.Notifications
                    .Where(n => n.UserId == currentUserId)
                    .OrderByDescending(n => n.SentDate)
                    .ToList();

                foreach (var notif in userNotifications)
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

        private void MarkAsRead()
        {
            if (SelectedNotification == null) return;

            try
            {
                // Mark in DB
                var dbNotif = _context.Notifications
                    .FirstOrDefault(n => n.NotificationId == SelectedNotification.NotificationId);

                if (dbNotif != null)
                {
                    dbNotif.IsRead = true;
                    _context.SaveChanges();
                }

                // Also mark in local collection
                SelectedNotification.IsRead = true;
                OnPropertyChanged(nameof(Notifications));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error marking as read: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
