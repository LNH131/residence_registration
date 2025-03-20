using Project.Models;
using Project.Service;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
namespace Project.ViewModels
{
    public class CitizenViewModel : BaseViewModel
    {
        // Các thuộc tính thông tin cá nhân
        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        private string _registrationStatus;
        public string RegistrationStatus
        {
            get => _registrationStatus;
            set { _registrationStatus = value; OnPropertyChanged(); }
        }

        // Notification Center: Danh sách thông báo
        private ObservableCollection<Notification> _notifications;
        public ObservableCollection<Notification> Notifications
        {
            get => _notifications;
            set { _notifications = value; OnPropertyChanged(); }
        }

        // Thông báo đang chọn (nếu cần)
        private Notification _selectedNotification;
        public Notification SelectedNotification
        {
            get => _selectedNotification;
            set { _selectedNotification = value; OnPropertyChanged(); }
        }

        // Các Command
        public ICommand LoadNotificationsCommand { get; set; }
        public ICommand MarkAsReadCommand { get; set; }
        public ICommand OpenChatCommand { get; set; }

        // Constructor: khởi tạo giá trị và command
        public CitizenViewModel(User user)
        {
            // Giả lập thông tin cá nhân (thay bằng dữ liệu thật từ DB hoặc session)
            CurrentUser = user;
            Debug.WriteLine($"User: {CurrentUser.Sex}");
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

        // Phương thức lấy danh sách thông báo
        private void LoadNotifications()
        {
            // Ở đây bạn có thể gọi service thực sự thay vì dữ liệu mẫu
            Notifications.Clear();

            // Ví dụ: thêm vài thông báo mẫu
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

        // Phương thức đánh dấu thông báo đã đọc
        private void MarkNotificationAsRead()
        {
            if (SelectedNotification != null)
            {
                SelectedNotification.IsRead = true;
                // Nếu có service cập nhật DB, gọi ở đây
                OnPropertyChanged(nameof(Notifications));
            }
        }

        // Phương thức mở cửa sổ chat với tổ trưởng hoặc công an
        private void OpenChat()
        {
            var chatWindow = new Project.View.ChatWindow();
            chatWindow.DataContext = new Project.ViewModels.ChatViewModel(1008, 1009);
            chatWindow.Show();
        }
    }
}