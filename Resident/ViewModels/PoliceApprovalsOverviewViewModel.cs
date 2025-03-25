using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;
using Resident.Service;
using Resident.View;
using Resident.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class PoliceApprovalsOverviewViewModel : BaseViewModel
{
    private readonly PrnContext _context;
    private readonly RegistrationService _registrationService;

    public ObservableCollection<ApprovalItem> ApprovalItems { get; set; }
        = new ObservableCollection<ApprovalItem>();

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

        RefreshCommand = new LocalRelayCommand(_ => LoadAllItems());
        ViewDetailsCommand = new LocalRelayCommand(o => ViewDetails(o), o => o != null);

        LoadAllItems();
    }

    private void LoadAllItems()
    {
        ApprovalItems.Clear();

        // Registration => hiển thị những hồ sơ có status = ApprovedByLeader (Police xử lý final)
        var regs = _context.Registrations
                           .Include(r => r.User)
                           .Where(r => r.Status == Status.ApprovedByLeader.ToString()
                                    || r.Status == Status.Pending.ToString()) // tuỳ logic
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

        // HouseholdTransfer => hiển thị Transfer mà area leader có lẽ phê duyệt sơ bộ?
        var transfers = _context.HouseholdTransfers
            .Include(t => t.Household)
                .ThenInclude(h => h.HeadOfHouseHold)
                    .ThenInclude(hh => hh.User)
            .Where(t => t.Status == Status.ApprovedByLeader.ToString()
                     || t.Status == Status.Pending.ToString())
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

        // HouseholdSeparation => hiển thị Separation chờ final
        var seps = _context.HouseholdSeparations
            .Include(s => s.OriginalHousehold)
                .ThenInclude(h => h.HeadOfHouseHold)
                    .ThenInclude(hh => hh.User)
            .Where(s => s.Status == Status.ApprovedByLeader.ToString()
                     || s.Status == Status.Pending.ToString())
            .ToList();
        foreach (var s in seps)
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
    }

    private void ViewDetails(object parameter)
    {
        if (parameter is ApprovalItem item)
        {
            if (item.ItemType == "Registration")
            {
                // Police -> final Approve => RegistrationDetailsWindow
                var window = new RegistrationDetailsWindow(
                    new RegistrationDetailsViewModel(item.UnderlyingItem as Registration,
                        /* ICurrentUserService? */ null)
                );
                window.ShowDialog();
            }
            else if (item.ItemType == "HouseholdTransfer")
            {
                var window = new HouseholdTransferDetailsWindow(
                    new HouseholdTransferDetailsViewModel(item.UnderlyingItem as HouseholdTransfer)
                );
                window.ShowDialog();
            }
            else if (item.ItemType == "HouseholdSeparation")
            {
                var window = new HouseholdSeparationDetailsWindow(
                    new HouseholdSeparationDetailsViewModel(item.UnderlyingItem as HouseholdSeparation)
                );
                window.ShowDialog();
            }
        }
    }
}
