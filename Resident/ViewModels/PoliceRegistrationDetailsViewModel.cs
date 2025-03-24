using Resident.Enums;
using Resident.Models;
using Resident.Service;
using Resident.Services; // Note: Ensure IPoliceProcessingService is in this namespace
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class PoliceRegistrationDetailsViewModel : BaseViewModel
    {
        private readonly IPoliceProcessingService _policeProcessingService;
        private Registration _registration;
        public Registration Registration
        {
            get => _registration;
            set { _registration = value; OnPropertyChanged(); }
        }

        // Commands to process or reject the registration.
        public ICommand ProcessCommand { get; }
        public ICommand RejectCommand { get; }

        // The constructor receives the Registration (full details already loaded or passed in)
        // and the police processing service via dependency injection.
        public PoliceRegistrationDetailsViewModel(Registration registration, IPoliceProcessingService policeProcessingService)
        {
            _policeProcessingService = policeProcessingService;
            Registration = registration;

            ProcessCommand = new LocalRelayCommand(async o => await ProcessRegistrationAsync());
            RejectCommand = new LocalRelayCommand(o => RejectRegistration());
        }

        private async Task ProcessRegistrationAsync()
        {
            // Process the registration – for example, update status to "ProcessedByPolice"
            await _policeProcessingService.ProcessRegistrationAsync(Registration);
            MessageBox.Show($"Hồ sơ ID = {Registration.RegistrationId} đã được xử lý bởi Công an.");
            // Optionally, update local state or close the window.
        }

        private void RejectRegistration()
        {
            Registration.Status = Status.Rejected.ToString();
            // If needed, you could update the registration via your service here.
            MessageBox.Show($"Hồ sơ ID = {Registration.RegistrationId} đã bị từ chối bởi Công an.");
        }
    }
}
