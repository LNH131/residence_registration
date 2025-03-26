using Resident.Enums;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class HouseholdSeparationDetailsViewModel : BaseViewModel
    {
        private readonly IPoliceProcessingService _policeProcessingService;
        private readonly ICurrentUserService _currentUserService;

        private HouseholdSeparation _separation;
        public HouseholdSeparation Separation
        {
            get => _separation;
            set
            {
                _separation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanModify));
            }
        }

        // We also want to load OriginalHousehold's info, NewHousehold's info, or members, if needed.
        // Example: we store them in separate properties, or load them directly from Separation. 
        // If you want a list of the members that will be separated, you can load them into an ObservableCollection.

        // For example, let's show the members of the OriginalHousehold:
        private ObservableCollection<User> _originalHouseholdMembers;
        public ObservableCollection<User> OriginalHouseholdMembers
        {
            get => _originalHouseholdMembers;
            set { _originalHouseholdMembers = value; OnPropertyChanged(); }
        }

        // If there's a new household created, we can show those members too:
        private ObservableCollection<User> _newHouseholdMembers;
        public ObservableCollection<User> NewHouseholdMembers
        {
            get => _newHouseholdMembers;
            set { _newHouseholdMembers = value; OnPropertyChanged(); }
        }

        // Only allow Approve/Reject when status is Pending.
        public bool CanModify => Separation.Status == Status.Pending.ToString();

        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        public HouseholdSeparationDetailsViewModel(
            HouseholdSeparation separation,
            IPoliceProcessingService policeProcessingService,
            ICurrentUserService currentUserService)
        {
            _policeProcessingService = policeProcessingService;
            _currentUserService = currentUserService;
            Separation = separation;

            // Load the household members (optional).
            LoadHouseholdMembers();

            ApproveCommand = new LocalRelayCommand(
                async _ => await ApproveSeparationAsync(),
                _ => CanModify
            );
            RejectCommand = new LocalRelayCommand(
                async _ => await RejectSeparationAsync(),
                _ => CanModify
            );
        }

        private void LoadHouseholdMembers()
        {
            // Example logic: load members from the OriginalHousehold 
            // and any existing members from the NewHousehold (if created).
            var context = new PrnContext();

            // 1. Original household members
            if (Separation.OriginalHouseholdId > 0)
            {
                var originalHousehold = context.Households
                    .Find(Separation.OriginalHouseholdId);
                if (originalHousehold != null)
                {
                    // Include user info
                    var members = context.HouseholdMembers
                        .Join(context.Users,
                              hm => hm.UserId,
                              u => u.UserId,
                              (hm, u) => new { hm, u })
                        .Where(x => x.hm.HouseholdId == originalHousehold.HouseholdId)
                        .Select(x => x.u)
                        .ToList();

                    OriginalHouseholdMembers = new ObservableCollection<User>(members);
                }
            }

            // 2. If the new household has already been created (NewHouseholdId != null):
            if (Separation.NewHouseholdId.HasValue && Separation.NewHouseholdId.Value > 0)
            {
                var newHousehold = context.Households
                    .Find(Separation.NewHouseholdId.Value);
                if (newHousehold != null)
                {
                    var members = context.HouseholdMembers
                        .Join(context.Users,
                              hm => hm.UserId,
                              u => u.UserId,
                              (hm, u) => new { hm, u })
                        .Where(x => x.hm.HouseholdId == newHousehold.HouseholdId)
                        .Select(x => x.u)
                        .ToList();

                    NewHouseholdMembers = new ObservableCollection<User>(members);
                }
            }
        }

        private async Task ApproveSeparationAsync()
        {
            try
            {
                int currentPoliceId = _currentUserService.CurrentUser.UserId;
                await _policeProcessingService.ProcessHouseholdSeparationAsync(Separation, currentPoliceId);
                MessageBox.Show($"Household Separation ID {Separation.SeparationId} approved.", "Success");
                OnPropertyChanged(nameof(CanModify));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error approving separation: {ex.Message}",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private async Task RejectSeparationAsync()
        {
            try
            {
                // Example of "rejecting" a separation:
                Separation.Status = Status.Rejected.ToString();

                // If you have a separation service to handle rejections:
                var separationService = new HouseholdSeparationService();
                separationService.UpdateHouseholdSeparation(Separation);

                MessageBox.Show($"Household Separation ID {Separation.SeparationId} rejected.", "Info");
                OnPropertyChanged(nameof(CanModify));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error rejecting separation: {ex.Message}",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}
