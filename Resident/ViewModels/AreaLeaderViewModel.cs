using Resident.Enums;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class AreaLeaderViewModel : BaseViewModel
    {
        private readonly ICurrentUserService _currentUserService;
        private RegistrationService service = new RegistrationService();

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

        public ICommand ApproveCommand { get; }
        public ICommand SendCommentCommand { get; }
        public ICommand ViewNotificationsCommand { get; }
        public ICommand ChatCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand ViewHouseholdCommand { get; }
        public ICommand ViewAllRegistrationsCommand { get; }  // New command

        // Constructor with ICurrentUserService injected.
        public AreaLeaderViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            LoadPendingRegistrations();

            ApproveCommand = new RelayCommand(o => ApproveRegistration());
            SendCommentCommand = new RelayCommand(o => SendComment());
            ViewNotificationsCommand = new RelayCommand(o => ViewNotifications());
            ChatCommand = new RelayCommand(o => Chat());
            GenerateReportCommand = new RelayCommand(o => GenerateReport());
            ViewDetailsCommand = new RelayCommand(ViewDetails);
            ViewHouseholdCommand = new RelayCommand(o => ViewHousehold());
            ViewAllRegistrationsCommand = new RelayCommand(o => ViewAllRegistrations());
        }

        private void LoadPendingRegistrations()
        {
            var regs = service.GetPendingRegistrations();
            PendingRegistrations = new ObservableCollection<Registration>(regs);
        }

        private void ApproveRegistration()
        {
            if (SelectedRegistration == null)
            {
                MessageBox.Show("Vui lòng chọn hồ sơ để duyệt!");
                return;
            }
            SelectedRegistration.Status = Status.Approved.ToString();
            service.UpdateRegistration(SelectedRegistration);
            MessageBox.Show($"Đã duyệt hồ sơ ID = {SelectedRegistration.RegistrationId}");
        }

        private void SendComment()
        {
            if (SelectedRegistration == null)
            {
                MessageBox.Show("Vui lòng chọn hồ sơ để gửi nhận xét!");
                return;
            }
            MessageBox.Show($"Gửi nhận xét cho Công an về hồ sơ ID = {SelectedRegistration.RegistrationId}");
        }

        private void ViewNotifications()
        {
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
            chatWindow.DataContext = new ChatViewModel(1009, citizenId);
            chatWindow.Show();
        }

        private void GenerateReport()
        {
            MessageBox.Show("Tạo báo cáo tổng hợp khu vực...");
        }

        private void ViewDetails(object parameter)
        {
            var registration = parameter as Registration;
            if (registration == null)
            {
                MessageBox.Show("Vui lòng chọn hồ sơ để xem chi tiết!");
                return;
            }
            var detailsWindow = new Resident.View.RegistrationDetailsWindow();
            detailsWindow.DataContext = new RegistrationDetailsViewModel(registration, _currentUserService);
            detailsWindow.Show();
        }

        private void ViewHousehold()
        {
            var householdDetailsWindow = new Resident.View.HouseholdDetailsWindow(new HouseholdDetailsViewModel());
            householdDetailsWindow.Show();
        }

        // New method to view all registrations.
        private void ViewAllRegistrations()
        {
            // Create the RegistrationOverviewViewModel, then create and show the window.
            var overviewVM = new RegistrationOverviewViewModel();
            var overviewWindow = new Resident.View.RegistrationOverviewWindow(overviewVM);
            overviewWindow.Show();
        }
    }
}
