using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;
using Resident.Service;
using Resident.View;
using Resident.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

public class PoliceViewModel : BaseViewModel
{
    private readonly ICurrentUserService _currentUserService;
    private readonly RegistrationService _registrationService;
    private readonly PrnContext _context;
    private readonly IPoliceProcessingService _policeProcessingService;

    // Danh sách hồ sơ đã được tổ trưởng duyệt
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

    // Các command cho dashboard
    public ICommand ProcessCommand { get; }
    public ICommand ViewDetailsCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ChatCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand ViewHouseholdDetailCommand { get; }
    public ICommand ViewReportCommand { get; }
    public ICommand NotificationCommand { get; }
    public ICommand ViewAllRegistrationsCommand { get; }

    public PoliceViewModel(ICurrentUserService currentUserService, IPoliceProcessingService policeProcessingService)
    {
        _currentUserService = currentUserService;
        _registrationService = new RegistrationService();
        _context = new PrnContext();
        _policeProcessingService = policeProcessingService;

        // Khởi tạo các command với LocalRelayCommand (đảm bảo RelayCommand của bạn hỗ trợ LocalRelayCommand)
        ProcessCommand = new LocalRelayCommand(async o => await ProcessApprovalAsync(), o => SelectedApprovalItem != null);
        ViewDetailsCommand = new LocalRelayCommand(o => ViewDetails(o), o => o != null);
        RefreshCommand = new LocalRelayCommand(o => LoadApprovalItems());
        ChatCommand = new LocalRelayCommand(o => OpenChat());
        ViewHouseholdDetailCommand = new LocalRelayCommand(o => OpenHouseholdDetail());
        ViewReportCommand = new LocalRelayCommand(o => OpenReports());
        NotificationCommand = new LocalRelayCommand(o => OpenNotifications());
        ViewAllRegistrationsCommand = new LocalRelayCommand(o => OpenAllRegistrations());

        LoadApprovalItems();
    }

    // Tải danh sách các hồ sơ được duyệt bởi Tổ trưởng (ApprovedByLeader)
    private void LoadApprovalItems()
    {
        ApprovalItems.Clear();

        // Tải hồ sơ đăng ký với trạng thái ApprovedByLeader
        var regs = _context.Registrations
            .Include(r => r.User)
            .Where(r => r.Status == Status.ApprovedByLeader.ToString())
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

        // Tải các yêu cầu chuyển hộ với trạng thái ApprovedByLeader
        var transfers = _context.HouseholdTransfers
            .Include(t => t.Household)
                .ThenInclude(h => h.HeadOfHouseHold)
                    .ThenInclude(hh => hh.User)
            .Include(t => t.FromAddress)
            .Include(t => t.ToAddress)
            .Where(t => t.Status == Status.ApprovedByLeader.ToString())
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

        // Tải các yêu cầu tách hộ với trạng thái ApprovedByLeader
        var separations = _context.HouseholdSeparations
            .Include(s => s.OriginalHousehold)
                .ThenInclude(h => h.HeadOfHouseHold)
                    .ThenInclude(hh => hh.User)
            .Where(s => s.Status == Status.ApprovedByLeader.ToString())
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

    private async Task ProcessApprovalAsync()
    {
        if (SelectedApprovalItem == null) return;

        int currentPoliceId = _currentUserService.CurrentUser.UserId;

        try
        {
            if (SelectedApprovalItem.ItemType == "Registration")
            {
                var reg = SelectedApprovalItem.UnderlyingItem as Registration;
                if (reg != null)
                {
                    await _policeProcessingService.ProcessRegistrationAsync(reg, currentPoliceId);
                    MessageBox.Show($"Registration ID {reg.RegistrationId} processed by Police.",
                                    "Success", MessageBoxButton.OK);
                }
            }
            else if (SelectedApprovalItem.ItemType == "HouseholdTransfer")
            {
                var transfer = SelectedApprovalItem.UnderlyingItem as HouseholdTransfer;
                if (transfer != null)
                {
                    await _policeProcessingService.ProcessHouseholdTransferAsync(transfer, currentPoliceId);
                    MessageBox.Show($"Household Transfer ID {transfer.TransferId} processed by Police.",
                                    "Success", MessageBoxButton.OK);
                }
            }
            else if (SelectedApprovalItem.ItemType == "HouseholdSeparation")
            {
                var separation = SelectedApprovalItem.UnderlyingItem as HouseholdSeparation;
                if (separation != null)
                {
                    await _policeProcessingService.ProcessHouseholdSeparationAsync(separation, currentPoliceId);
                    MessageBox.Show($"Household Separation ID {separation.SeparationId} processed by Police.",
                                    "Success", MessageBoxButton.OK);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error processing approval: {ex.Message}",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Refresh the list so the processed item disappears from the queue
        LoadApprovalItems();
    }

    private void ViewDetails(object parameter)
    {
        if (parameter is ApprovalItem item)
        {
            if (item.ItemType == "Registration")
            {
                var detailsWindow = new RegistrationDetailsWindow(
                    new RegistrationDetailsViewModel(item.UnderlyingItem as Registration, _currentUserService)
                );
                detailsWindow.ShowDialog();
            }
            else if (item.ItemType == "HouseholdTransfer")
            {
                var transferDetailsWindow = new HouseholdTransferDetailsWindow(
                    new HouseholdTransferDetailsViewModel(item.UnderlyingItem as HouseholdTransfer)
                );
                transferDetailsWindow.ShowDialog();
            }
            else if (item.ItemType == "HouseholdSeparation")
            {
                var separationDetailsWindow = new HouseholdSeparationDetailsWindow(
                    new HouseholdSeparationDetailsViewModel(item.UnderlyingItem as HouseholdSeparation)
                );
                separationDetailsWindow.ShowDialog();
            }
        }
    }

    private void OpenChat()
    {
        var selectionVM = new PoliceChatSelectionViewModel(_currentUserService);
        var selectionWindow = new PoliceChatSelectionWindow(selectionVM);
        selectionWindow.ShowDialog();
    }

    private void OpenHouseholdDetail()
    {
        var householdDetailsWindow = new HouseholdDetailsWindow(new HouseholdDetailsViewModel());
        householdDetailsWindow.ShowDialog();
    }

    private void OpenReports()
    {
        MessageBox.Show("Report functionality is under development.", "Reports");
    }

    private void OpenNotifications()
    {
        MessageBox.Show("Notification functionality is under development.", "Notifications");
    }

    private void OpenAllRegistrations()
    {
        var overviewVM = new RegistrationOverviewViewModel();
        var overviewWindow = new RegistrationOverviewWindow(overviewVM);
        overviewWindow.Show();
    }


}
