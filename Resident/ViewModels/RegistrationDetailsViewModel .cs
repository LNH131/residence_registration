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

        // Constructor now accepts ICurrentUserService via dependency injection.
        public RegistrationDetailsViewModel(Registration registration, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            // Load full registration details including related data.
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
