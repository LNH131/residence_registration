using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using Resident.View;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class AreaLeaderHouseholdChangesViewModel : BaseViewModel
    {
        private readonly PrnContext _context;

        // Unified list for HouseholdTransfer and HouseholdSeparation items.
        public ObservableCollection<ApprovalItem> ChangeItems { get; set; } = new ObservableCollection<ApprovalItem>();

        private ApprovalItem _selectedChangeItem;
        public ApprovalItem SelectedChangeItem
        {
            get => _selectedChangeItem;
            set
            {
                _selectedChangeItem = value;
                OnPropertyChanged(nameof(SelectedChangeItem));
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand ViewDetailsCommand { get; }

        public AreaLeaderHouseholdChangesViewModel()
        {
            _context = new PrnContext();
            RefreshCommand = new LocalRelayCommand(o => LoadChangeItems());
            ViewDetailsCommand = new LocalRelayCommand(o => ViewDetails(o), o => o != null);
            LoadChangeItems();
        }

        private void LoadChangeItems()
        {
            ChangeItems.Clear();

            // Load HouseholdTransfer items with status "Pending"
            var transfers = _context.HouseholdTransfers
                .Include(t => t.Household)
                    .ThenInclude(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
                .Where(t => t.Status == "Pending")
                .ToList();
            foreach (var transfer in transfers)
            {
                string creator = transfer.Household?.HeadOfHouseHold?.User?.FullName ?? "N/A";
                ChangeItems.Add(new ApprovalItem
                {
                    ItemId = transfer.TransferId,
                    ItemType = "HouseholdTransfer",
                    CreatorName = creator,
                    Status = transfer.Status,
                    UnderlyingItem = transfer
                });
            }

            // Load HouseholdSeparation items with status "Pending"
            var separations = _context.HouseholdSeparations
                .Include(s => s.OriginalHousehold)
                    .ThenInclude(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
                .Where(s => s.Status == "Pending")
                .ToList();
            foreach (var sep in separations)
            {
                string creator = sep.OriginalHousehold?.HeadOfHouseHold?.User?.FullName ?? "N/A";
                ChangeItems.Add(new ApprovalItem
                {
                    ItemId = sep.SeparationId,
                    ItemType = "HouseholdSeparation",
                    CreatorName = creator,
                    Status = sep.Status,
                    UnderlyingItem = sep
                });
            }

            OnPropertyChanged(nameof(ChangeItems));
        }

        private void ViewDetails(object parameter)
        {
            if (parameter is ApprovalItem item)
            {
                if (item.ItemType == "HouseholdTransfer")
                {
                    var transferDetailsWindow = new HouseholdTransferDetailsWindow(
                        new HouseholdTransferDetailsViewModel(item.UnderlyingItem as HouseholdTransfer));
                    transferDetailsWindow.ShowDialog();
                }
                else if (item.ItemType == "HouseholdSeparation")
                {
                    var separationDetailsWindow = new HouseholdSeparationDetailsWindow(
                        new HouseholdSeparationDetailsViewModel(item.UnderlyingItem as HouseholdSeparation));
                    separationDetailsWindow.ShowDialog();
                }
            }
        }
    }
}
