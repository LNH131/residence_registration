using Project.Models;
using Project.Service;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Project.ViewModels
{
    public class CitizenViewModel : BaseViewModel
    {
        private readonly ICurrentUserService _currentUserService;

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

        public ICommand LoadNotificationsCommand { get; set; }
        public ICommand MarkAsReadCommand { get; set; }
        public ICommand OpenChatCommand { get; set; }

        public CitizenViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;

            // Debug thông tin người dùng
            Debug.WriteLine($"User: {CurrentUser?.Sex ?? "Chưa có thông tin"}");
            RegistrationStatus = "Chờ phê duyệt";

            // Khởi tạo danh sách thông báo
            Notifications = new ObservableCollection<Notification>();

            // Khởi tạo các command
            LoadNotificationsCommand = new RelayCommand(o => LoadNotifications());
            MarkAsReadCommand = new RelayCommand(o => MarkNotificationAsRead(), o => SelectedNotification != null);
            OpenChatCommand = new RelayCommand(o => OpenChat());

            // Load dữ liệu thông báo ban đầu
            LoadNotifications();
        }

        private void LoadNotifications()
        {
            Notifications.Clear();
            Notifications.Add(new Notification
            {
                NotificationId = 1,
                UserId = 0,
                Message = "Hồ sơ của bạn đã được phê duyệt.",
                SentDate = DateTime.Now,
                IsRead = false
            });
            Notifications.Add(new Notification
            {
                NotificationId = 2,
                UserId = 0,
                Message = "Cập nhật thông tin của bạn để đảm bảo tính chính xác.",
                SentDate = DateTime.Now.AddMinutes(-30),
                IsRead = false
            });
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
            var chatWindow = new Project.View.ChatWindow();
            chatWindow.DataContext = new Project.ViewModels.ChatViewModel(1008, 1009);
            chatWindow.Show();
        }
    }
}