using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class HouseholdDetailsViewModel : BaseViewModel
    {
        // Collection of households for display.
        private ObservableCollection<HouseholdDisplay> _households;
        public ObservableCollection<HouseholdDisplay> Households
        {
            get => _households;
            set { _households = value; OnPropertyChanged(nameof(Households)); }
        }

        // The currently selected household in the DataGrid.
        private HouseholdDisplay _selectedHousehold;
        public HouseholdDisplay SelectedHousehold
        {
            get => _selectedHousehold;
            set { _selectedHousehold = value; OnPropertyChanged(nameof(SelectedHousehold)); }
        }

        // Command to open details for a selected household.
        public ICommand ViewDetailsCommand { get; }

        // Constructor: Initializes the command and loads household data.
        public HouseholdDetailsViewModel()
        {
            ViewDetailsCommand = new LocalRelayCommand(o => OpenHouseholdMemberDetails(o));
            LoadHouseholds();
        }

        // Load households from the database and map them to HouseholdDisplay objects.
        private void LoadHouseholds()
        {
            using (var context = new PrnContext())
            {
                var households = context.Households
                    .Include(h => h.Address)
                    .Include(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
                    .ToList();

                // Create a collection of HouseholdDisplay with a formatted address.
                Households = new ObservableCollection<HouseholdDisplay>(
                    households.Select(h => new HouseholdDisplay
                    {
                        HouseholdId = h.HouseholdId,
                        FormattedAddress = string.Join(", ", new[]
                        {
                            h.Address?.Street,
                            h.Address?.Ward,
                            h.Address?.District,
                            h.Address?.City,
                            h.Address?.Country
                        }.Where(s => !string.IsNullOrWhiteSpace(s))),

                        HeadName = h.HeadOfHouseHold?.User?.FullName ?? "N/A",
                        CreatedDate = h.CreatedDate.HasValue
                            ? h.CreatedDate.Value.ToString("yyyy-MM-dd")
                            : "N/A"
                    }));
            }
        }

        // Open the HouseholdMemberDetailsWindow for the selected household.
        private void OpenHouseholdMemberDetails(object parameter)
        {
            if (parameter is HouseholdDisplay display)
            {
                // Pass the household id to the details view model.
                var memberDetailsVM = new HouseholdMemberDetailsViewModel(display.HouseholdId);
                var memberDetailsWindow = new Resident.View.HouseholdMemberDetailsWindow(memberDetailsVM);
                memberDetailsWindow.Show();
            }
        }
    }

    // Helper class used for displaying household information.
    public class HouseholdDisplay
    {
        public int HouseholdId { get; set; }
        public string FormattedAddress { get; set; }
        public string HeadName { get; set; }
        public string CreatedDate { get; set; }
    }
}
