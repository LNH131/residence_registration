using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;
using Resident.Service;
using Resident.View;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class AreaLeaderApprovalsOverviewViewModel : BaseViewModel
    {
        private readonly PrnContext _context;
        private readonly RegistrationService _registrationService;
        private readonly ICurrentUserService _currentUserService;

        public ObservableCollection<ApprovalItem> ApprovalItems { get; set; } = new ObservableCollection<ApprovalItem>();

        public ICommand RefreshCommand { get; }
        public ICommand ViewDetailsCommand { get; }

        /// <summary>
        /// Constructor for the AreaLeaderApprovalsOverviewViewModel.
        /// Inject ICurrentUserService so we can pass the current user to detail view models if needed.
        /// </summary>
        public AreaLeaderApprovalsOverviewViewModel(ICurrentUserService currentUserService)
        {
            _context = new PrnContext();
            _registrationService = new RegistrationService();
            _currentUserService = currentUserService;

            RefreshCommand = new LocalRelayCommand(_ => LoadApprovalItems());
            ViewDetailsCommand = new LocalRelayCommand(o => ViewDetails(o), o => o != null);

            LoadApprovalItems();
        }

        /// <summary>
        /// Load all pending items from Registration, HouseholdTransfer, and HouseholdSeparation.
        /// </summary>
        private void LoadApprovalItems()
        {
            ApprovalItems.Clear();

            // Load Registration items with status Pending.
            var regs = _registrationService.GetPendingRegistrations()
                        .Where(r => r.Status == Status.Pending.ToString())
                        .ToList();
            foreach (var reg in regs)
            {
                ApprovalItems.Add(new ApprovalItem
                {
                    ItemId = reg.RegistrationId,
                    ItemType = "Registration",
                    CreatorName = reg.User?.FullName ?? "N/A",
                    Status = reg.Status,
                    UnderlyingItem = reg
                });
            }

            // Load HouseholdTransfer items with status Pending.
            var transfers = _context.HouseholdTransfers
                .Include(t => t.Household)
                    .ThenInclude(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
                .Where(t => t.Status == Status.Pending.ToString())
                .ToList();
            foreach (var transfer in transfers)
            {
                string creator = transfer.Household?.HeadOfHouseHold?.User?.FullName ?? "N/A";
                ApprovalItems.Add(new ApprovalItem
                {
                    ItemId = transfer.TransferId,
                    ItemType = "HouseholdTransfer",
                    CreatorName = creator,
                    Status = transfer.Status,
                    UnderlyingItem = transfer
                });
            }

            // Load HouseholdSeparation items with status Pending.
            var separations = _context.HouseholdSeparations
                .Include(s => s.OriginalHousehold)
                    .ThenInclude(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
                .Where(s => s.Status == Status.Pending.ToString())
                .ToList();
            foreach (var sep in separations)
            {
                string creator = sep.OriginalHousehold?.HeadOfHouseHold?.User?.FullName ?? "N/A";
                ApprovalItems.Add(new ApprovalItem
                {
                    ItemId = sep.SeparationId,
                    ItemType = "HouseholdSeparation",
                    CreatorName = creator,
                    Status = sep.Status,
                    UnderlyingItem = sep
                });
            }

            OnPropertyChanged(nameof(ApprovalItems));
        }

        /// <summary>
        /// Inspects the type of the selected item and opens the corresponding details window.
        /// </summary>
        private void ViewDetails(object parameter)
        {
            if (parameter is not ApprovalItem item) return;

            if (item.ItemType == "Registration")
            {
                var registration = item.UnderlyingItem as Registration;
                if (registration != null)
                {
                    // Pass both the Registration object and the current user service
                    var detailsVM = new AreaLeaderRegistrationDetailsViewModel(registration, _currentUserService);

                    var detailsWindow = new AreaLeaderRegistrationDetailsWindow(detailsVM);
                    detailsWindow.ShowDialog();

                }
            }
            else if (item.ItemType == "HouseholdTransfer")
            {
                var transfer = item.UnderlyingItem as HouseholdTransfer;
                if (transfer != null)
                {
                    var detailsVM = new HouseholdTransferDetailsViewModel(transfer);
                    var detailsWindow = new HouseholdTransferDetailsWindow(detailsVM);
                    detailsWindow.DataContext = detailsVM;
                    detailsWindow.ShowDialog();
                }
            }
            else if (item.ItemType == "HouseholdSeparation")
            {
                var separation = item.UnderlyingItem as HouseholdSeparation;
                if (separation != null)
                {
                    var detailsVM = new HouseholdSeparationDetailsViewModel(separation);
                    var detailsWindow = new HouseholdSeparationDetailsWindow(detailsVM);
                    detailsWindow.DataContext = detailsVM;
                    detailsWindow.ShowDialog();
                }
            }
        }
    }
}
