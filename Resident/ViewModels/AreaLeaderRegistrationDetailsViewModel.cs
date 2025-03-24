using Resident.Enums;
using Resident.Models;
using Resident.Service;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class AreaLeaderRegistrationDetailsViewModel : BaseViewModel
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

        // Constructor accepts ICurrentUserService via dependency injection.
        public AreaLeaderRegistrationDetailsViewModel(Registration registration, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            // Load full registration details including related data.
            Registration = service.GetRegistrationDetails(registration);

            ApproveCommand = new LocalRelayCommand(o => ApproveRegistration());
            RejectCommand = new LocalRelayCommand(o => RejectRegistration());
        }

        private void ApproveRegistration()
        {
            // Instead of performing full approval (adding household, etc.),
            // the area leader only sets the status to ApprovedByLeader.
            Registration.Status = Status.ApprovedByLeader.ToString();
            service.UpdateRegistration(Registration);
            MessageBox.Show($"Hồ sơ ID = {Registration.RegistrationId} đã được duyệt sơ bộ bởi Tổ trưởng khu phố. \n\nChờ xử lý cuối cùng từ phía Công an.");
        }

        private void RejectRegistration()
        {
            Registration.Status = Status.Rejected.ToString();
            service.UpdateRegistration(Registration);
            MessageBox.Show($"Hồ sơ ID = {Registration.RegistrationId} đã bị từ chối.");
        }
    }
}
