using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Resident.ViewModels
{
    public class StatusOverviewViewModel : INotifyPropertyChanged
    {
        private readonly ICurrentUserService _currentUserService;

        public User CurrentUser => _currentUserService.CurrentUser;

        private ObservableCollection<Registration> _registrations;
        public ObservableCollection<Registration> Registrations
        {
            get => _registrations;
            set { _registrations = value; OnPropertyChanged(nameof(Registrations)); }
        }

        private ObservableCollection<HouseholdSeparation> _householdSeparations;
        public ObservableCollection<HouseholdSeparation> HouseholdSeparations
        {
            get => _householdSeparations;
            set { _householdSeparations = value; OnPropertyChanged(nameof(HouseholdSeparations)); }
        }

        public StatusOverviewViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            using (var context = new PrnContext())
            {
                // 1) Load registrations for the current user.
                Registrations = new ObservableCollection<Registration>(
                    context.Registrations
                           .Where(r => r.UserId == CurrentUser.UserId)
                           .ToList()
                );

                // 2) Load HouseholdSeparations in which the current user is head.
                //    We include the related households and their HeadOfHouseHold.
                HouseholdSeparations = new ObservableCollection<HouseholdSeparation>(
                    context.HouseholdSeparations
                           .Include(s => s.OriginalHousehold)
                               .ThenInclude(h => h.HeadOfHouseHold)
                           .Include(s => s.NewHousehold)
                               .ThenInclude(h => h.HeadOfHouseHold)
                           .Where(s =>
                               // Check if the current user is head in the OriginalHousehold
                               (s.OriginalHousehold.HeadOfHouseHold != null && s.OriginalHousehold.HeadOfHouseHold.UserId == CurrentUser.UserId)
                               // OR if the current user is head in the NewHousehold
                               || (s.NewHousehold != null && s.NewHousehold.HeadOfHouseHold != null && s.NewHousehold.HeadOfHouseHold.UserId == CurrentUser.UserId)
                           )
                           .ToList()
                );
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
