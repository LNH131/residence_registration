﻿using Resident.Enums;
using Resident.Models;
using Resident.Service;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class AreaLeaderViewModel : BaseViewModel
    {
        private ObservableCollection<Registration> _pendingRegistrations;
        public ObservableCollection<Registration> PendingRegistrations
        {
            get => _pendingRegistrations;
            set { _pendingRegistrations = value; OnPropertyChanged(); }
        }

        private Registration _selectedRegistration;
        public Registration SelectedRegistration
        {
            get => _selectedRegistration;
            set { _selectedRegistration = value; OnPropertyChanged(); }
        }

        // Các Command
        public ICommand ApproveCommand { get; }
        public ICommand SendCommentCommand { get; }
        public ICommand ViewNotificationsCommand { get; }
        public ICommand ChatCommand { get; }
        public ICommand GenerateReportCommand { get; }

        public AreaLeaderViewModel()
        {
            LoadPendingRegistrations();

            ApproveCommand = new RelayCommand(o => ApproveRegistration());
            SendCommentCommand = new RelayCommand(o => SendComment());
            ViewNotificationsCommand = new RelayCommand(o => ViewNotifications());
            ChatCommand = new RelayCommand(o => Chat());
            GenerateReportCommand = new RelayCommand(o => GenerateReport());
        }

        private void LoadPendingRegistrations()
        {
            // Dữ liệu cứng minh họa; thay thế bằng service lấy dữ liệu từ DB theo AreaLeader hiện tại
            PendingRegistrations = new ObservableCollection<Registration>
    {
        new Registration
        {
            RegistrationId = 1001,
            Status = Resident.Enums.Status.Pending.ToString(),
            User = new User { UserId = 2001, FullName = "Nguyễn Văn B" }
        },
        new Registration
        {
            RegistrationId = 1002,
            Status = Resident.Enums.Status.Pending.ToString(),
            User = new User { UserId = 2002, FullName = "Lê Thị C" }
        }
    };
        }

        private void ApproveRegistration()
        {
            if (SelectedRegistration == null)
            {
                MessageBox.Show("Vui lòng chọn hồ sơ để duyệt!");
                return;
            }
            SelectedRegistration.Status = Status.Approved.ToString();
            // Ở đây bạn có thể gọi service cập nhật DB
            MessageBox.Show($"Đã duyệt hồ sơ ID={SelectedRegistration.RegistrationId}");
        }

        private void SendComment()
        {
            if (SelectedRegistration == null)
            {
                MessageBox.Show("Vui lòng chọn hồ sơ để gửi nhận xét!");
                return;
            }
            // Mở cửa sổ nhập nhận xét hoặc thực hiện logic gửi thông báo đến công an
            MessageBox.Show($"Gửi nhận xét cho Công an về hồ sơ ID={SelectedRegistration.RegistrationId}");
        }

        private void ViewNotifications()
        {
            // Ở đây bạn có thể mở cửa sổ hoặc hiển thị panel quản lý thông báo của khu vực
            MessageBox.Show("Xem thông báo cho cư dân trong khu vực...");
        }

        private void Chat()
        {
            if (SelectedRegistration == null || SelectedRegistration.User == null)
            {
                MessageBox.Show("Vui lòng chọn hồ sơ để chat!");
                return;
            }
            int citizenId = SelectedRegistration.User.UserId;
            var chatWindow = new Resident.View.ChatWindow();
            chatWindow.DataContext = new Resident.ViewModels.ChatViewModel(1009, citizenId);
            chatWindow.Show();
        }

        private void GenerateReport()
        {
            // Triển khai logic thống kê và xuất báo cáo PDF/Excel
            MessageBox.Show("Tạo báo cáo tổng hợp khu vực...");
        }

    }
}
