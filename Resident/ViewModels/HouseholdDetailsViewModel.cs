using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class HouseholdDetailsViewModel : BaseViewModel
    {
        private ObservableCollection<HouseholdDisplay> _households;
        public ObservableCollection<HouseholdDisplay> Households
        {
            get => _households;
            set { _households = value; OnPropertyChanged(nameof(Households)); }
        }

        private HouseholdDisplay _selectedHousehold;
        public HouseholdDisplay SelectedHousehold
        {
            get => _selectedHousehold;
            set { _selectedHousehold = value; OnPropertyChanged(nameof(SelectedHousehold)); }
        }

        public ICommand ViewDetailsCommand { get; }

        public HouseholdDetailsViewModel()
        {
            ViewDetailsCommand = new RelayCommand(o => ViewDetails(o));
            LoadHouseholds();
        }

        private void LoadHouseholds()
        {
            using (var context = new PrnContext())
            {
                var households = context.Households
                    .Include(h => h.Address)
                    .Include(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
                    .ToList();

                Households = new ObservableCollection<HouseholdDisplay>(
                    households.Select(h => new HouseholdDisplay
                    {
                        HouseholdId = h.HouseholdId,
                        FormattedAddress = string.Join(", ", new[] {
                            h.Address.Street, h.Address.Ward, h.Address.District, h.Address.City, h.Address.Country
                        }.Where(s => !string.IsNullOrWhiteSpace(s))),
                        HeadName = h.HeadOfHouseHold?.User?.FullName ?? "N/A",
                        CreatedDate = h.CreatedDate.HasValue ? h.CreatedDate.Value.ToString("yyyy-MM-dd") : ""
                    }));
            }
        }

        private void ViewDetails(object parameter)
        {
            if (parameter is HouseholdDisplay display)
            {
                // Open the HouseholdMemberDetailsWindow with the household id.
                var memberDetailsVM = new HouseholdMemberDetailsViewModel(display.HouseholdId);
                var memberDetailsWindow = new Resident.View.HouseholdMemberDetailsWindow(memberDetailsVM);
                memberDetailsWindow.Show();
            }
        }
    }

    public class HouseholdDisplay
    {
        public int HouseholdId { get; set; }
        public string FormattedAddress { get; set; }
        public string HeadName { get; set; }
        public string CreatedDate { get; set; }
    }
}
