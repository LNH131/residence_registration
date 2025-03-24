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

        // Danh sách các hồ sơ từ Registrations, HouseholdTransfers, và HouseholdSeparations.
        public ObservableCollection<ApprovalItem> ApprovalItems { get; set; } = new ObservableCollection<ApprovalItem>();

        private ApprovalItem _selectedApprovalItem;
        public ApprovalItem SelectedApprovalItem
        {
            get => _selectedApprovalItem;
            set
            {
                _selectedApprovalItem = value;
                OnPropertyChanged(nameof(SelectedApprovalItem));
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand ViewDetailsCommand { get; }

        public PoliceApprovalsOverviewViewModel()
        {
            _context = new PrnContext();
            _registrationService = new RegistrationService();

            RefreshCommand = new LocalRelayCommand(o => LoadAllItems());
            ViewDetailsCommand = new LocalRelayCommand(o => ViewDetails(o), o => o != null);

            LoadAllItems();
        }

        private void LoadAllItems()
        {
            ApprovalItems.Clear();

            // 1) Load tất cả các Registration (loại trừ nếu cần, ví dụ: loại trừ Rejected)
            var regs = _context.Registrations
                        .Include(r => r.User)
                        .Where(r => r.Status != Status.Rejected.ToString())
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

            // 2) Load tất cả các HouseholdTransfer (không lọc theo trạng thái)
            var transfers = _context.HouseholdTransfers
                .Include(t => t.Household)
                    .ThenInclude(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
                .Include(t => t.FromAddress)
                .Include(t => t.ToAddress)
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

            // 3) Load tất cả các HouseholdSeparation (không lọc theo trạng thái)
            var separations = _context.HouseholdSeparations
                .Include(s => s.OriginalHousehold)
                    .ThenInclude(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
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

        private void ViewDetails(object parameter)
        {
            if (parameter is ApprovalItem item)
            {
                if (item.ItemType == "Registration")
                {
                    var detailsWindow = new RegistrationDetailsWindow();
                    // Ở đây bạn có thể truyền một instance ICurrentUserService nếu cần.
                    detailsWindow.DataContext = new RegistrationDetailsViewModel(item.UnderlyingItem as Registration, null);
                    detailsWindow.ShowDialog();
                }
                else if (item.ItemType == "HouseholdTransfer")
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
