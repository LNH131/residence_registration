using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class RegistrationOverviewViewModel : BaseViewModel
    {
        private RegistrationService _service = new RegistrationService();

        private ObservableCollection<Registration> _allRegistrations;
        public ObservableCollection<Registration> AllRegistrations
        {
            get => _allRegistrations;
            set { _allRegistrations = value; OnPropertyChanged(nameof(AllRegistrations)); }
        }

        public ICommand ViewDetailsCommand { get; }

        public RegistrationOverviewViewModel()
        {
            LoadAllRegistrations();
            ViewDetailsCommand = new RelayCommand(o => ViewDetails(o));
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
                // Open the RegistrationDetailsWindow for the selected registration.
                var detailsWindow = new Resident.View.RegistrationDetailsWindow();
                // If RegistrationDetailsViewModel requires ICurrentUserService, ensure you pass a valid instance.
                detailsWindow.DataContext = new RegistrationDetailsViewModel(registration, new CurrentUserService());
                detailsWindow.Show();
            }
        }
    }
}
