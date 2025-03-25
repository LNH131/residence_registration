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

        private readonly RegistrationService _registrationService = new RegistrationService();

        /// <summary>
        /// Constructor for the AreaLeaderRegistrationDetailsViewModel.
        /// </summary>
        /// <param name="registration">The registration item to display or edit.</param>
        /// <param name="currentUserService">Injected user service, if needed.</param>
        public AreaLeaderRegistrationDetailsViewModel(Registration registration, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            Registration = _registrationService.GetRegistrationDetails(registration);

            ApproveCommand = new LocalRelayCommand(_ => ApproveRegistration());
            RejectCommand = new LocalRelayCommand(_ => RejectRegistration());
        }

        private void ApproveRegistration()
        {
            try
            {
                // Set status to ApprovedByLeader and record the current user as the approver.
                Registration.Status = Status.ApprovedByLeader.ToString();
                Registration.ApprovedBy = _currentUserService.CurrentUser.UserId; // using the injected CurrentUser
                _registrationService.UpdateRegistration(Registration);
                MessageBox.Show($"Hồ sơ ID = {Registration.RegistrationId} đã được duyệt sơ bộ.", "Approval Success");
                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during approval: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RejectRegistration()
        {
            try
            {
                Registration.Status = Status.Rejected.ToString();
                _registrationService.UpdateRegistration(Registration);
                MessageBox.Show($"Hồ sơ ID = {Registration.RegistrationId} đã bị từ chối.", "Rejection Success");
                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during rejection: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Helper method to close the current details window.
        /// This assumes that the DataContext is set and the window is accessible.
        /// </summary>
        private void CloseWindow()
        {
            // Find the window that has this view model as its DataContext and close it.
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
