using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;
using Resident.Service;
using Resident.View;
using Resident.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

public class AreaLeaderApprovalsOverviewViewModel : BaseViewModel
{
    private readonly PrnContext _context;
    private readonly RegistrationService _registrationService;

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
    // Preliminary approval for Registration items (if you still want it).
    public ICommand ApproveCommand { get; }

    public AreaLeaderApprovalsOverviewViewModel()
    {
        _context = new PrnContext();
        _registrationService = new RegistrationService();

        RefreshCommand = new LocalRelayCommand(o => LoadAllItems());
        ViewDetailsCommand = new LocalRelayCommand(o => ViewDetails(o), o => o != null);
        ApproveCommand = new LocalRelayCommand(o => ApproveRegistration(),
            o => SelectedApprovalItem != null && SelectedApprovalItem.ItemType == "Registration");

        LoadAllItems(); // Load everything on construction
    }

    private void LoadAllItems()
    {
        ApprovalItems.Clear();

        // 1. Load ALL Registrations (no status filter).
        var allRegs = _context.Registrations
            .Include(r => r.User)
            .ToList();
        // Or you could call your RegistrationService if it provides a method to get all:
        // var allRegs = _registrationService.GetAllRegistrations();

        foreach (var reg in allRegs)
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

        // 2. Load ALL HouseholdTransfers (no status filter).
        var allTransfers = _context.HouseholdTransfers
            .Include(t => t.Household)
                .ThenInclude(h => h.HeadOfHouseHold)
                    .ThenInclude(hh => hh.User)
            .Include(t => t.FromAddress)
            .Include(t => t.ToAddress)
            .ToList();

        foreach (var transfer in allTransfers)
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

        // 3. Load ALL HouseholdSeparations (no status filter).
        var allSeparations = _context.HouseholdSeparations
            .Include(s => s.OriginalHousehold)
                .ThenInclude(h => h.HeadOfHouseHold)
                    .ThenInclude(hh => hh.User)
            .Include(s => s.NewHousehold)
            .ToList();

        foreach (var sep in allSeparations)
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

    private void ApproveRegistration()
    {
        if (SelectedApprovalItem == null)
        {
            MessageBox.Show("Vui lòng chọn hồ sơ đăng ký để duyệt sơ bộ!");
            return;
        }

        var reg = SelectedApprovalItem.UnderlyingItem as Registration;
        if (reg != null)
        {
            reg.Status = Status.ApprovedByLeader.ToString();
            _registrationService.UpdateRegistration(reg);
            MessageBox.Show($"Hồ sơ ID = {reg.RegistrationId} đã được duyệt sơ bộ.\nChờ xử lý từ phía Công an.");
            LoadAllItems(); // refresh
        }
    }

    private void ViewDetails(object parameter)
    {
        if (parameter is ApprovalItem item)
        {
            if (item.ItemType == "Registration")
            {
                var detailsWindow = new AreaLeaderRegistrationDetailsWindow(
                    new AreaLeaderRegistrationDetailsViewModel(item.UnderlyingItem as Registration, null /* pass ICurrentUserService if needed */));
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
