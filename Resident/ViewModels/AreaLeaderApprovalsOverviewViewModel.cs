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
        private readonly IPoliceProcessingService _policeProcessingService;

        public ObservableCollection<ApprovalItem> ApprovalItems { get; set; }
            = new ObservableCollection<ApprovalItem>();

        public ICommand RefreshCommand { get; }
        public ICommand ViewDetailsCommand { get; }

        /// <summary>
        /// Constructor for AreaLeaderApprovalsOverviewViewModel.
        /// Injects ICurrentUserService and IPoliceProcessingService so that the current user and processing logic can be passed
        /// to detail view models.
        /// </summary>
        public AreaLeaderApprovalsOverviewViewModel(
            ICurrentUserService currentUserService,
            IPoliceProcessingService policeProcessingService)
        {
            _context = new PrnContext();
            _registrationService = new RegistrationService();
            _currentUserService = currentUserService;
            _policeProcessingService = policeProcessingService;

            RefreshCommand = new LocalRelayCommand(_ => LoadApprovalItems());
            ViewDetailsCommand = new LocalRelayCommand(o => ViewDetails(o), o => o != null);

            LoadApprovalItems();
        }

        /// <summary>
        /// Loads all ApprovalItems from Registrations, HouseholdTransfers, and HouseholdSeparations
        /// whose status is either Pending or ApprovedByLeader.
        /// </summary>
        private void LoadApprovalItems()
        {
            ApprovalItems.Clear();

            // 1) Load Registrations (status Pending or ApprovedByLeader)
            var regs = _context.Registrations
                               .Include(r => r.User)
                               .Where(r => r.Status == Status.Pending.ToString() ||
                                           r.Status == Status.ApprovedByLeader.ToString())
                               .ToList();
            foreach (var r in regs)
            {
                ApprovalItems.Add(new ApprovalItem
                {
                    ItemId = r.RegistrationId,
                    ItemType = "Registration",
                    CreatorName = r.User?.FullName ?? "N/A",
                    Status = r.Status,
                    UnderlyingItem = r
                });
            }

            // 2) Load HouseholdTransfers (status Pending or ApprovedByLeader)
            var transfers = _context.HouseholdTransfers
                .Include(t => t.Household)
                    .ThenInclude(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
                .Include(t => t.FromAddress)
                .Include(t => t.ToAddress)
                .Where(t => t.Status == Status.Pending.ToString() ||
                            t.Status == Status.ApprovedByLeader.ToString())
                .ToList();
            foreach (var t in transfers)
            {
                string creator = t.Household?.HeadOfHouseHold?.User?.FullName ?? "N/A";
                ApprovalItems.Add(new ApprovalItem
                {
                    ItemId = t.TransferId,
                    ItemType = "HouseholdTransfer",
                    CreatorName = creator,
                    Status = t.Status,
                    UnderlyingItem = t
                });
            }

            // 3) Load HouseholdSeparations (status Pending or ApprovedByLeader)
            var separations = _context.HouseholdSeparations
                .Include(s => s.OriginalHousehold)
                    .ThenInclude(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
                .Where(s => s.Status == Status.Pending.ToString() ||
                            s.Status == Status.ApprovedByLeader.ToString())
                .ToList();
            foreach (var s in separations)
            {
                string creator = s.OriginalHousehold?.HeadOfHouseHold?.User?.FullName ?? "N/A";
                ApprovalItems.Add(new ApprovalItem
                {
                    ItemId = s.SeparationId,
                    ItemType = "HouseholdSeparation",
                    CreatorName = creator,
                    Status = s.Status,
                    UnderlyingItem = s
                });
            }

            OnPropertyChanged(nameof(ApprovalItems));
        }

        /// <summary>
        /// Opens the corresponding details window based on the ItemType of the selected item.
        /// </summary>
        /// <param name="parameter">The ApprovalItem passed from the view.</param>
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
