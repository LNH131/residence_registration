using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Resident.Models;
using Resident.Service;
using Resident.View;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class CitizenViewModel : BaseViewModel
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IServiceProvider _serviceProvider;

        // Thuộc tính CurrentUser lấy từ ICurrentUserService
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

        public ICommand OpenNotificationsCommand { get; }
        public ICommand ManageHouseholdCommand { get; set; }
        public ICommand LoadNotificationsCommand { get; set; }
        public ICommand MarkAsReadCommand { get; set; }
        public ICommand OpenChatCommand { get; set; }

        public ICommand UpdateProfileCommand { get; set; }

        public CitizenViewModel(ICurrentUserService currentUserService, IServiceProvider serviceProvider)
        {
            _currentUserService = currentUserService;
            _serviceProvider = serviceProvider;

            // Debug current user info.
            Debug.WriteLine($"User: {CurrentUser?.FullName ?? "Chưa có thông tin"}");
            RegistrationStatus = "Chờ phê duyệt";

            // Khởi tạo danh sách thông báo
            Notifications = new ObservableCollection<Notification>();

            // Initialize commands.
            ManageHouseholdCommand = new LocalRelayCommand(o => ManageHousehold());
            MarkAsReadCommand = new LocalRelayCommand(o => MarkNotificationAsRead(), o => SelectedNotification != null);
            OpenChatCommand = new LocalRelayCommand(o => OpenChat());
            UpdateProfileCommand = new LocalRelayCommand(o => UpdateProfile());
            OpenNotificationsCommand = new RelayCommand(OpenNotifications);
        }

        private void UpdateProfile()
        {
            var updateCitizenProfileWindow = _serviceProvider.GetRequiredService<UpdateCitizenProfileWindow>();
            updateCitizenProfileWindow.ShowDialog();
        }

        private void OpenNotifications()
        {
            // Kiểm tra xem CurrentUser có được gán không.
            if (_currentUserService.CurrentUser == null)
            {
                MessageBox.Show("Chưa có thông tin người dùng, vui lòng đăng nhập lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var vm = new CitizenNotificationViewModel(_currentUserService);
            var window = new CitizenNotificationWindow(vm);
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

        private void OpenChat()
        {
            var selectionVM = new CitizenPoliceChatSelectionViewModel(_currentUserService);
            var selectionWindow = new CitizenPoliceChatSelectionWindow(selectionVM);
            selectionWindow.ShowDialog();
        }

        private void ManageHousehold()
        {
            var manageHouseholdWindow = _serviceProvider.GetRequiredService<HouseHoldControlWindow>();
            manageHouseholdWindow.ShowDialog();
        }
    }
}