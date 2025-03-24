using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Resident;
using Resident.Enums;
using Resident.Models;
using Resident.Service;
using Resident.Services;
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

    // ================== APPROVAL ITEMS (for the top DataGrid) ==================
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

    // ================== HOUSEHOLD MONITORING (for the bottom DataGrid) ==================
    private ObservableCollection<Household> _households;
    public ObservableCollection<Household> Households
    {
        get => _households;
        set
        {
            _households = value;
            OnPropertyChanged();
        }
    }

    private Household _selectedHousehold;
    public Household SelectedHousehold
    {
        get => _selectedHousehold;
        set
        {
            _selectedHousehold = value;
            OnPropertyChanged();
        }
    }

    // ================== COMMANDS ==================
    public ICommand ProcessCommand { get; }
    public ICommand ViewDetailsCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ChatCommand { get; }
    public ICommand LogoutCommand { get; }

    // Các lệnh bổ sung cho các nút khác.
    public ICommand ViewHouseholdDetailCommand { get; }
    public ICommand ViewReportCommand { get; }
    public ICommand NotificationCommand { get; }
    public ICommand ViewAllApprovalsCommand { get; }

    public PoliceViewModel(ICurrentUserService currentUserService,
                           IPoliceProcessingService policeProcessingService)
    {
        _currentUserService = currentUserService;
        _registrationService = new RegistrationService();
        _context = new PrnContext();
        _policeProcessingService = policeProcessingService;

        // ================== COMMAND INITIALIZATION ==================
        ProcessCommand = new LocalRelayCommand(async o => await ProcessApprovalAsync(),
                                          o => SelectedApprovalItem != null);
        ViewDetailsCommand = new LocalRelayCommand(o => ViewDetails(o), o => o != null);
        RefreshCommand = new LocalRelayCommand(o => RefreshAll());
        ChatCommand = new LocalRelayCommand(o => OpenChat());
        LogoutCommand = new LocalRelayCommand(o => Logout());

        ViewHouseholdDetailCommand = new LocalRelayCommand(o => OpenHouseholdDetail());
        ViewReportCommand = new LocalRelayCommand(o => OpenReports());
        NotificationCommand = new LocalRelayCommand(o => OpenNotifications());
        ViewAllApprovalsCommand = new LocalRelayCommand(o => OpenAllApprovals());

        // ================== INITIAL LOAD ==================
        LoadApprovalItems();
        LoadHouseholds();
    }

    // ================== LOAD APPROVAL ITEMS (ApprovedByLeader) ==================
    private void LoadApprovalItems()
    {
        ApprovalItems.Clear();

        // Đăng ký
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

        // Chuyển hộ
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

        // Tách hộ
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

    // ================== LOAD HOUSEHOLDS (for Household Monitoring) ==================
    private void LoadHouseholds()
    {
        var allHouseholds = _context.Households
            .Include(h => h.Address)
            .ToList();

        Households = new ObservableCollection<Household>(allHouseholds);
    }

    // ================== PROCESS APPROVAL ITEM ==================
    private async Task ProcessApprovalAsync()
    {
        if (SelectedApprovalItem == null) return;

        if (SelectedApprovalItem.ItemType == "Registration")
        {
            var reg = SelectedApprovalItem.UnderlyingItem as Registration;
            if (reg != null)
            {
                await _policeProcessingService.ProcessRegistrationAsync(reg);
                MessageBox.Show($"Registration ID {reg.RegistrationId} has been processed by Police.");
            }
        }
        else if (SelectedApprovalItem.ItemType == "HouseholdTransfer")
        {
            var transfer = SelectedApprovalItem.UnderlyingItem as HouseholdTransfer;
            if (transfer != null)
            {
                await _policeProcessingService.ProcessHouseholdTransferAsync(transfer);
                MessageBox.Show($"Household Transfer ID {transfer.TransferId} has been processed by Police.");
            }
        }
        else if (SelectedApprovalItem.ItemType == "HouseholdSeparation")
        {
            var separation = SelectedApprovalItem.UnderlyingItem as HouseholdSeparation;
            if (separation != null)
            {
                await _policeProcessingService.ProcessHouseholdSeparationAsync(separation);
                MessageBox.Show($"Household Separation ID {separation.SeparationId} has been processed by Police.");
            }
        }

        // Refresh sau khi xử lý
        LoadApprovalItems();
        LoadHouseholds();
    }

    // ================== VIEW DETAILS ==================
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

    // ================== OPEN HOUSEHOLD DETAIL ==================
    private void OpenHouseholdDetail()
    {
        // Nếu bạn đã có HouseholdDetailsWindow và HouseholdDetailsViewModel
        var householdDetailsWindow = new HouseholdDetailsWindow(new HouseholdDetailsViewModel());
        householdDetailsWindow.ShowDialog();
    }

    // ================== OPEN REPORTS ==================
    private void OpenReports()
    {
        MessageBox.Show("Chức năng báo cáo đang được phát triển.", "Reports");
    }

    // ================== OPEN NOTIFICATIONS ==================
    private void OpenNotifications()
    {
        MessageBox.Show("Chức năng thông báo đang được phát triển.", "Notifications");
    }

    // ================== OPEN ALL APPROVALS ==================
    private void OpenAllApprovals()
    {
        var approvalsOverviewVM = new PoliceApprovalsOverviewViewModel();
        var approvalsOverviewWindow = new PoliceApprovalsOverviewWindow(approvalsOverviewVM);
        approvalsOverviewWindow.ShowDialog();
    }

    // ================== OPEN CHAT ==================
    private void OpenChat()
    {
        var selectionVM = new PoliceChatSelectionViewModel(_currentUserService);
        var selectionWindow = new PoliceChatSelectionWindow(selectionVM);
        selectionWindow.ShowDialog();
    }

    // ================== LOGOUT ==================
    private void Logout()
    {
        var serviceProvider = ((App)Application.Current).ServiceProvider;
        var loginWindow = serviceProvider.GetRequiredService<LoginWindow>();
        loginWindow.Show();
        Application.Current.MainWindow.Close();
    }

    // ================== REFRESH ALL (cả approvals lẫn households) ==================
    private void RefreshAll()
    {
        LoadApprovalItems();
        LoadHouseholds();
    }
}
