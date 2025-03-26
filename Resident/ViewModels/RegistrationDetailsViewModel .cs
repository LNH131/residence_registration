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
        private readonly RegistrationService _registrationService;

        private Registration _registration;
        public Registration Registration
        {
            get => _registration;
            set
            {
                _registration = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanModify));
            }
        }

        // Only allow Approve/Reject if not yet finalized (e.g., not Approved or Rejected).
        public bool CanModify =>
            Registration.Status != Status.Approved.ToString() &&
            Registration.Status != Status.Rejected.ToString();

        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        public RegistrationDetailsViewModel(Registration registration, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _registrationService = new RegistrationService();

            // Refresh from DB if needed.
            Registration = _registrationService.GetRegistrationDetails(registration);

            ApproveCommand = new LocalRelayCommand(_ => ApproveRegistration(), _ => CanModify);
            RejectCommand = new LocalRelayCommand(_ => RejectRegistration(), _ => CanModify);
        }

        private void ApproveRegistration()
        {
            var currentUser = _currentUserService.CurrentUser;
            _registrationService.ApproveRegistration(Registration, currentUser);

            MessageBox.Show($"Registration ID {Registration.RegistrationId} approved.", "Success");
            OnPropertyChanged(nameof(CanModify)); // Re-check after status changes
        }

        private void RejectRegistration()
        {
            Registration.Status = Status.Rejected.ToString();
            _registrationService.UpdateRegistration(Registration);

            MessageBox.Show($"Registration ID {Registration.RegistrationId} rejected.", "Info");
            OnPropertyChanged(nameof(CanModify));
        }
    }
}
