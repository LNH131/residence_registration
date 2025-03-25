using Resident.Enums;
using Resident.Models;
using Resident.Service;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class RegistrationDetailsViewModel : BaseViewModel
    {
        private readonly ICurrentUserService _currentUserService;
        private Registration _registration;
        public Registration Registration
        {
            get => _registration;
            set { _registration = value; OnPropertyChanged(); }
        }

        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        private RegistrationService service = new RegistrationService();

        // Constructor: nhận Registration và ICurrentUserService
        public RegistrationDetailsViewModel(Registration registration, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            // Tải đầy đủ thông tin registration từ DB (nếu cần).
            Registration = service.GetRegistrationDetails(registration);

            ApproveCommand = new LocalRelayCommand(o => ApproveRegistration());
            RejectCommand = new LocalRelayCommand(o => RejectRegistration());
        }

        private void ApproveRegistration()
        {
            var currentUser = _currentUserService.CurrentUser;
            service.ApproveRegistration(Registration, currentUser);
            MessageBox.Show($"Đã duyệt hồ sơ ID = {Registration.RegistrationId} và tạo hộ khẩu.");
        }

        private void RejectRegistration()
        {
            Registration.Status = Status.Rejected.ToString();
            service.UpdateRegistration(Registration);
            MessageBox.Show($"Đã từ chối hồ sơ ID = {Registration.RegistrationId}.");
        }
    }
}
