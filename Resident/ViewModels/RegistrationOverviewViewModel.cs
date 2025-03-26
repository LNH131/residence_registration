using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class RegistrationOverviewViewModel : BaseViewModel
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly RegistrationService _registrationService;

        private ObservableCollection<Registration> _allRegistrations;
        public ObservableCollection<Registration> AllRegistrations
        {
            get => _allRegistrations;
            set { _allRegistrations = value; OnPropertyChanged(nameof(AllRegistrations)); }
        }

        public ICommand ViewDetailsCommand { get; }

        // Constructor uses DI to obtain the necessary services.
        public RegistrationOverviewViewModel(ICurrentUserService currentUserService, RegistrationService registrationService)
        {
            _currentUserService = currentUserService;
            _registrationService = registrationService;
            LoadAllRegistrations();
            ViewDetailsCommand = new LocalRelayCommand(o => ViewDetails(o), o => o != null);
        }

        private void LoadAllRegistrations()
        {
            using (var context = new PrnContext())
            {
                // Load all registrations and include the associated User.
                var regs = context.Registrations
                                  .Include(r => r.User)
                                  .ToList();
                AllRegistrations = new ObservableCollection<Registration>(regs);
            }
        }

        private void ViewDetails(object parameter)
        {
            if (parameter is Registration registration)
            {
                var detailsVM = new RegistrationDetailsViewModel(registration, _currentUserService);
                var detailsWindow = new Resident.View.RegistrationDetailsWindow(detailsVM);
                detailsWindow.ShowDialog();
            }
        }
    }
}
