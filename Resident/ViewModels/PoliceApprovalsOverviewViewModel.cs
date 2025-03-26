using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;
using Resident.Service;
using Resident.View;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class PoliceApprovalsOverviewViewModel : BaseViewModel
    {
        private readonly PrnContext _context;
        private readonly RegistrationService _registrationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPoliceProcessingService _policeProcessingService;

        public ObservableCollection<ApprovalItem> ApprovalItems { get; set; }
            = new ObservableCollection<ApprovalItem>();

        public ICommand RefreshCommand { get; }
        public ICommand ViewDetailsCommand { get; }

        /// <summary>
        /// Constructor: show all items that have status=Approved and ApprovedBy = current policeman's ID.
        /// </summary>
        public PoliceApprovalsOverviewViewModel(
            ICurrentUserService currentUserService,
            IPoliceProcessingService policeProcessingService)
        {
            _currentUserService = currentUserService;
            _policeProcessingService = policeProcessingService;
            _context = new PrnContext();
            _registrationService = new RegistrationService();

            RefreshCommand = new LocalRelayCommand(_ => LoadAllItems());
            ViewDetailsCommand = new LocalRelayCommand(o => ViewDetails(o), o => o != null);

            LoadAllItems();
        }

        /// <summary>
        /// Load all items (Registrations, HouseholdTransfers, HouseholdSeparations)
        /// that have Status=Approved and ApprovedBy = the currently logged-in policeman.
        /// </summary>
        private void LoadAllItems()
        {
            ApprovalItems.Clear();

            // Current policeman's ID
            int currentPoliceId = _currentUserService.CurrentUser.UserId;

            // 1) Registrations: status=Approved, ApprovedBy = current user ID
            var regs = _context.Registrations
                .Include(r => r.User)
                .Where(r => r.Status == Status.Approved.ToString()
                         && r.ApprovedBy == currentPoliceId)
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

            // 2) HouseholdTransfers: status=Approved, ApprovedBy = current user ID
            var transfers = _context.HouseholdTransfers
                .Include(t => t.Household)
                    .ThenInclude(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
                .Include(t => t.FromAddress)
                .Include(t => t.ToAddress)
                .Where(t => t.Status == Status.Approved.ToString()
                         && t.ApprovedBy == currentPoliceId)
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

            // 3) HouseholdSeparations: status=Approved, ApprovedBy = current user ID
            var separations = _context.HouseholdSeparations
                .Include(s => s.OriginalHousehold)
                    .ThenInclude(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
                .Where(s => s.Status == Status.Approved.ToString()
                         && s.ApprovedBy == currentPoliceId)
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
        /// Opens the details window for the selected item.
        /// </summary>
        private void ViewDetails(object parameter)
        {
            if (parameter is not ApprovalItem item) return;

            switch (item.ItemType)
            {
                case "Registration":
                    if (item.UnderlyingItem is Registration registration)
                    {
                        var detailsVM = new RegistrationDetailsViewModel(registration, _currentUserService);
                        var detailsWindow = new RegistrationDetailsWindow(detailsVM);
                        detailsWindow.ShowDialog();
                    }
                    break;

                case "HouseholdTransfer":
                    if (item.UnderlyingItem is HouseholdTransfer transfer)
                    {
                        var detailsVM = new HouseholdTransferDetailsViewModel(transfer);
                        var detailsWindow = new HouseholdTransferDetailsWindow(detailsVM);
                        detailsWindow.ShowDialog();
                    }
                    break;

                case "HouseholdSeparation":
                    if (item.UnderlyingItem is HouseholdSeparation separation)
                    {
                        var detailsVM = new HouseholdSeparationDetailsViewModel(
                            separation,
                            _policeProcessingService,
                            _currentUserService
                        );
                        var detailsWindow = new HouseholdSeparationDetailsWindow(detailsVM);
                        detailsWindow.ShowDialog();
                    }
                    break;
            }
        }
    }
}
