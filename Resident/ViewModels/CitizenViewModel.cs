using Resident.Models;
using Resident.Service;
using Resident.View;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class CitizenViewModel : BaseViewModel
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IServiceProvider _serviceProvider;

        // Current user from ICurrentUserService.
        public User CurrentUser => _currentUserService.CurrentUser;

        private string _registrationStatus;
        public string RegistrationStatus
        {
            get => _registrationStatus;
            set { _registrationStatus = value; OnPropertyChanged(); }
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
            set { _selectedNotification = value; OnPropertyChanged(); }
        }

        public ICommand ManageHouseholdCommand { get; set; }
        public ICommand LoadNotificationsCommand { get; set; }
        public ICommand MarkAsReadCommand { get; set; }
        public ICommand OpenChatCommand { get; set; }
        public ICommand UpdateProfileCommand { get; set; }

        public CitizenViewModel(ICurrentUserService currentUserService, IServiceProvider serviceProvider)
        {
            _currentUserService = currentUserService;

            // Debug current user info.
            Debug.WriteLine($"User: {CurrentUser?.Sex ?? "Chưa có thông tin"}");
            RegistrationStatus = "Chờ phê duyệt";

            // Initialize notifications list.
            Notifications = new ObservableCollection<Notification>();

            // Initialize commands.
            ManageHouseholdCommand = new LocalRelayCommand(o => ManageHousehold());
            LoadNotificationsCommand = new LocalRelayCommand(o => LoadNotifications());
            MarkAsReadCommand = new LocalRelayCommand(o => MarkNotificationAsRead(), o => SelectedNotification != null);
            OpenChatCommand = new LocalRelayCommand(o => OpenChat());
            UpdateProfileCommand = new LocalRelayCommand(o => UpdateProfile());

            // Load initial notifications.
            LoadNotifications();
            _serviceProvider = serviceProvider;
        }

        private void UpdateProfile()
        {
        }

        private void LoadNotifications()
        {
            var notifVM = new CitizenNotificationViewModel(_currentUserService);

            // Show the window
            var window = new CitizenNotificationWindow(notifVM);
            window.ShowDialog();
        }

        private void MarkNotificationAsRead()
        {
            if (SelectedNotification != null)
            {
                SelectedNotification.IsRead = true;
                OnPropertyChanged(nameof(Notifications));
            }
        }

        // In your CitizenViewModel's OpenChat method:
        private void OpenChat()
        {
            // Open the citizen police chat selection window.
            var selectionVM = new CitizenPoliceChatSelectionViewModel(_currentUserService);
            var selectionWindow = new Resident.View.CitizenPoliceChatSelectionWindow(selectionVM);
            selectionWindow.ShowDialog();
        }


        private void ManageHousehold()
        {
        }
    }
}
