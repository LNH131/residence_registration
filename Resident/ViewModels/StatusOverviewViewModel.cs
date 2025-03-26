using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

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

        private ObservableCollection<HouseholdTransfer> _householdTransfers;
        public ObservableCollection<HouseholdTransfer> HouseholdTransfers
        {
            get => _householdTransfers;
            set { _householdTransfers = value; OnPropertyChanged(nameof(HouseholdTransfers)); }
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
                HouseholdSeparations = new ObservableCollection<HouseholdSeparation>(
                    context.HouseholdSeparations
                           .Include(s => s.OriginalHousehold)
                               .ThenInclude(h => h.HeadOfHouseHold)
                           .Include(s => s.NewHousehold)
                               .ThenInclude(h => h.HeadOfHouseHold)
                           .Where(s =>
                               (s.OriginalHousehold.HeadOfHouseHold != null && s.OriginalHousehold.HeadOfHouseHold.UserId == CurrentUser.UserId)
                               || (s.NewHousehold != null && s.NewHousehold.HeadOfHouseHold != null && s.NewHousehold.HeadOfHouseHold.UserId == CurrentUser.UserId)
                           )
                           .ToList()
                );

                // 3) Load HouseholdTransfers in which the current user là chủ hộ.
                HouseholdTransfers = new ObservableCollection<HouseholdTransfer>(
                    context.HouseholdTransfers
                           .Include(t => t.Household)
                               .ThenInclude(h => h.HeadOfHouseHold)
                           .Where(t => t.Household.HeadOfHouseHold != null && t.Household.HeadOfHouseHold.UserId == CurrentUser.UserId)
                           .ToList()
                );

                Debug.WriteLine($"Loaded {Registrations.Count} registrations, {HouseholdSeparations.Count} household separations, and {HouseholdTransfers.Count} household transfers for user {CurrentUser.UserId}.");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
