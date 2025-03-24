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

        // Updated Chat method: open chat with the citizen who submitted the selected registration.
        private void Chat()
        {
            // Instead of trying to chat with the selected registration’s user,
            // we open the selection window for area/police selection.
            var selectionVM = new AreaLeaderChatSelectionViewModel(_currentUserService);
            var selectionWindow = new Resident.View.AreaLeaderChatSelectionWindow(selectionVM);
            selectionWindow.ShowDialog();
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
        private void ViewAllRegistrations()
        {
            var overviewVM = new RegistrationOverviewViewModel();
            var overviewWindow = new Resident.View.RegistrationOverviewWindow(overviewVM);
            overviewWindow.Show();
        }
    }
}
