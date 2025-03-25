using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;
using Resident.Service;
using Resident.View;
using Resident.ViewModels; // If needed for referencing detail VMs
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

public class AreaLeaderViewModel : BaseViewModel
{
    private readonly ICurrentUserService _currentUserService;
    private readonly RegistrationService _registrationService;
    private readonly PrnContext _context;
    private readonly INotificationService _notificationService;

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

    // Commands
    public ICommand ApproveCommand { get; }
    public ICommand SendCommentCommand { get; }
    public ICommand ViewNotificationsCommand { get; }
    public ICommand ChatCommand { get; }
    public ICommand GenerateReportCommand { get; }
    public ICommand ViewDetailsCommand { get; }
    public ICommand ViewHouseholdCommand { get; }
    public ICommand ViewAllRegistrationsCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ViewApprovalsOverviewCommand { get; }
    public ICommand SendNotificationToCitizensCommand { get; }
    // Possibly: public ICommand LogoutCommand { get; }

    public AreaLeaderViewModel(ICurrentUserService currentUserService, INotificationService notificationService)
    {
        _currentUserService = currentUserService;
        _notificationService = notificationService;
        _registrationService = new RegistrationService();
        _context = new PrnContext();

        // Load initial pending items
        LoadApprovalItems();

        // Initialize commands
        ApproveCommand = new LocalRelayCommand(_ => ApproveRegistration(),
            _ => SelectedApprovalItem != null && SelectedApprovalItem.ItemType == "Registration");

        SendCommentCommand = new LocalRelayCommand(_ => SendComment());
        ViewNotificationsCommand = new LocalRelayCommand(_ => ViewNotifications());
        ChatCommand = new LocalRelayCommand(_ => Chat());
        GenerateReportCommand = new LocalRelayCommand(_ => GenerateReport());
        ViewDetailsCommand = new LocalRelayCommand(o => ViewDetails(o), o => o != null);
        ViewHouseholdCommand = new LocalRelayCommand(_ => ViewHousehold());
        ViewAllRegistrationsCommand = new LocalRelayCommand(_ => ViewAllRegistrations());
        RefreshCommand = new LocalRelayCommand(_ => LoadApprovalItems());
        ViewApprovalsOverviewCommand = new LocalRelayCommand(_ => OpenAllItemsWindow());
        SendNotificationToCitizensCommand = new AsyncRelayCommand(SendNotificationToCitizensAsync, () => true);
        // LogoutCommand = ...
    }

    // Load all pending items
    private void LoadApprovalItems()
    {
        ApprovalItems.Clear();

        // Registrations
        var regs = _registrationService.GetAllRegistrations() // or _context.Registrations
                                                              //.Where(r => r.Status == Status.Pending.ToString()) // remove this filter
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

        // HouseholdTransfers: show all
        var transfers = _context.HouseholdTransfers
            //.Where(t => t.Status == Status.Pending.ToString()) // remove this filter
            .Include(t => t.Household)
                .ThenInclude(h => h.HeadOfHouseHold)
                    .ThenInclude(hh => hh.User)
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

        // HouseholdSeparations: show all
        var separations = _context.HouseholdSeparations
            //.Where(s => s.Status == Status.Pending.ToString()) // remove this filter
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
    }

    // Approve a pending registration
    private void ApproveRegistration()
    {
        if (SelectedApprovalItem?.UnderlyingItem is Registration reg)
        {
            reg.Status = Status.ApprovedByLeader.ToString();
            _registrationService.UpdateRegistration(reg);
            MessageBox.Show($"Hồ sơ {reg.RegistrationId} đã được duyệt sơ bộ: {reg.Status}");
            LoadApprovalItems();
        }
    }

    private void SendComment()
    {
        MessageBox.Show("Gửi nhận xét cho Công an về hồ sơ được chọn...");
    }

    private void ViewNotifications()
    {
        MessageBox.Show("Chức năng xem thông báo cho cư dân đang được phát triển.", "Thông báo");
    }

    private void Chat()
    {
        var selectionVM = new AreaLeaderChatSelectionViewModel(_currentUserService);
        var selectionWindow = new AreaLeaderChatSelectionWindow(selectionVM);
        selectionWindow.ShowDialog();
    }

    private void GenerateReport()
    {
        MessageBox.Show("Chức năng báo cáo tổng hợp khu vực đang được phát triển.", "Báo cáo");
    }

    // Show detail window for the selected item
    private void ViewDetails(object parameter)
    {
        if (parameter is not ApprovalItem item) return;

        if (item.ItemType == "Registration")
        {
            // Pass the VM to the window constructor
            var registration = item.UnderlyingItem as Registration;
            if (registration != null)
            {
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
                detailsWindow.ShowDialog();
            }
        }
    }

    private void ViewHousehold()
    {
        var householdDetailsWindow = new HouseholdDetailsWindow(new HouseholdDetailsViewModel());
        householdDetailsWindow.ShowDialog();
    }

    private void ViewAllRegistrations()
    {
        var overviewVM = new RegistrationOverviewViewModel();
        var overviewWindow = new RegistrationOverviewWindow(overviewVM);
        overviewWindow.Show();
    }

    private void OpenAllItemsWindow()
    {
        // If your AreaLeaderApprovalsOverviewViewModel also needs the current user service,
        // pass it in. For example:
        var vm = new AreaLeaderApprovalsOverviewViewModel(_currentUserService);
        var window = new AreaLeaderApprovalsOverviewWindow(vm);
        window.ShowDialog();
    }

    private async Task SendNotificationToCitizensAsync()
    {
        string message = "Thông báo từ Tổ trưởng khu phố: Vui lòng cập nhật thông tin hộ khẩu mới.";
        var citizens = _context.Users.Where(u => u.Role == "Citizen").ToList();
        foreach (var citizen in citizens)
        {
            await _notificationService.SendInAppNotificationAsync(citizen.UserId, message);
        }
        MessageBox.Show("Thông báo đã được gửi đến tất cả cư dân.",
            "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
