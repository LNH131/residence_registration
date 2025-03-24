using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Resident;
using Resident.Enums;
using Resident.Models;
using Resident.Service;
using Resident.View;
using Resident.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

public class AreaLeaderViewModel : BaseViewModel
{
    private readonly ICurrentUserService _currentUserService;
    private readonly RegistrationService _registrationService;
    private readonly PrnContext _context;
    private readonly INotificationService? _notificationService; // Có thể null

    // Unified list for items from Registrations, HouseholdTransfers, and HouseholdSeparations.
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

    public ICommand OpenNotificationWindowCommand { get; }
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

    // NEW: Command gửi thông báo đến tất cả các Citizen.
    public ICommand SendNotificationToCitizensCommand { get; }

    // Constructor với notificationService là tùy chọn (default = null)
    public AreaLeaderViewModel(ICurrentUserService currentUserService, INotificationService? notificationService = null)
    {
        _currentUserService = currentUserService;
        _notificationService = notificationService;
        _registrationService = new RegistrationService();
        _context = new PrnContext();

        LoadApprovalItems();

        ApproveCommand = new LocalRelayCommand(o => ApproveRegistration(),
            o => SelectedApprovalItem != null && SelectedApprovalItem.ItemType == "Registration");
        SendCommentCommand = new LocalRelayCommand(o => SendComment());
        ViewNotificationsCommand = new LocalRelayCommand(o => ViewNotifications());
        ChatCommand = new LocalRelayCommand(o => Chat());
        GenerateReportCommand = new LocalRelayCommand(o => GenerateReport());
        ViewDetailsCommand = new LocalRelayCommand(o => ViewDetails(o), o => o != null);
        ViewHouseholdCommand = new LocalRelayCommand(o => ViewHousehold());
        ViewAllRegistrationsCommand = new LocalRelayCommand(o => ViewAllRegistrations());
        RefreshCommand = new LocalRelayCommand(o => LoadApprovalItems());
        ViewApprovalsOverviewCommand = new LocalRelayCommand(o => OpenAllItemsWindow());
        OpenNotificationWindowCommand = new LocalRelayCommand(o => OpenNotificationWindow());


        // Nếu có notificationService, khởi tạo command gửi thông báo; ngược lại, gán một command dummy.
        if (_notificationService != null)
            SendNotificationToCitizensCommand = new AsyncRelayCommand(SendNotificationToCitizensAsync);
        else
            SendNotificationToCitizensCommand = new LocalRelayCommand(o =>
            {
                MessageBox.Show("NotificationService is not available!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            });
    }
    private void OpenNotificationWindow()
    {
        // Lấy notificationService từ DI nếu cần. Ở đây, ta giả sử _notificationService đã được khởi tạo (có thể null).
        // Nếu chưa có, bạn có thể cảnh báo.
        if (_notificationService == null)
        {
            MessageBox.Show("NotificationService chưa được cung cấp.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Tạo ViewModel cho Notification Window.
        var notificationVM = new AreaLeaderNotificationViewModel(_notificationService);
        var notificationWindow = new AreaLeaderNotificationWindow(notificationVM);
        notificationWindow.ShowDialog();
    }
    // Loads only pending items.
    private void LoadApprovalItems()
    {
        ApprovalItems.Clear();

        // Load Registration items with status "Pending".
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

        // Load HouseholdTransfer items with status "Pending".
        var transfers = _context.HouseholdTransfers
            .Include(t => t.Household)
                .ThenInclude(h => h.HeadOfHouseHold)
                    .ThenInclude(hh => hh.User)
            .Where(t => t.Status == "Pending")
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

        // Load HouseholdSeparation items with status "Pending".
        var separations = _context.HouseholdSeparations
            .Include(s => s.OriginalHousehold)
                .ThenInclude(h => h.HeadOfHouseHold)
                    .ThenInclude(hh => hh.User)
            .Where(s => s.Status == "Pending")
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
            MessageBox.Show($"Hồ sơ ID = {reg.RegistrationId} đã được duyệt sơ bộ bởi Tổ trưởng khu phố.\nChờ xử lý từ phía Công an.");
            LoadApprovalItems(); // Refresh the list after update.
        }
    }

    private void SendComment()
    {
        MessageBox.Show("Gửi nhận xét cho Công an về hồ sơ được chọn...");
    }

    private void ViewNotifications()
    {
        MessageBox.Show("Xem thông báo cho cư dân...");
    }

    private void Chat()
    {
        var selectionVM = new AreaLeaderChatSelectionViewModel(_currentUserService);
        var selectionWindow = new AreaLeaderChatSelectionWindow(selectionVM);
        selectionWindow.ShowDialog();
    }

    private void GenerateReport()
    {
        MessageBox.Show("Tạo báo cáo tổng hợp khu vực...");
    }

    private void ViewDetails(object parameter)
    {
        if (parameter is ApprovalItem item)
        {
            if (item.ItemType == "Registration")
            {
                var detailsWindow = new AreaLeaderRegistrationDetailsWindow(
                    new AreaLeaderRegistrationDetailsViewModel(item.UnderlyingItem as Registration, _currentUserService));
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

    private void ViewHousehold()
    {
        var householdDetailsWindow = new HouseholdDetailsWindow(new HouseholdDetailsViewModel());
        householdDetailsWindow.Show();
    }

    private void ViewAllRegistrations()
    {
        var overviewVM = new RegistrationOverviewViewModel();
        var overviewWindow = new RegistrationOverviewWindow(overviewVM);
        overviewWindow.Show();
    }

    private void OpenAllItemsWindow()
    {
        var vm = new AreaLeaderApprovalsOverviewViewModel();
        var window = new AreaLeaderApprovalsOverviewWindow(vm);
        window.ShowDialog();
    }

    private async Task SendNotificationToCitizensAsync()
    {
        if (_notificationService == null)
        {
            MessageBox.Show("NotificationService chưa được truyền vào constructor!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        string message = "Thông báo từ Tổ trưởng khu phố: Vui lòng cập nhật thông tin hộ khẩu mới.";
        var citizens = _context.Users.Where(u => u.Role == "Citizen").ToList();
        foreach (var citizen in citizens)
        {
            await _notificationService.SendInAppNotificationAsync(citizen.UserId, message);
        }
        MessageBox.Show("Thông báo đã được gửi đến tất cả cư dân.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void OpenReports()
    {
        MessageBox.Show("Chức năng báo cáo đang được phát triển.", "Reports");
    }

    private void OpenNotifications()
    {
        MessageBox.Show("Chức năng thông báo đang được phát triển.", "Notifications");
    }

    private void OpenAllApprovals()
    {
        var approvalsOverviewVM = new PoliceApprovalsOverviewViewModel();
        var approvalsOverviewWindow = new PoliceApprovalsOverviewWindow(approvalsOverviewVM);
        approvalsOverviewWindow.ShowDialog();
    }

    private void OpenChat()
    {
        var selectionVM = new PoliceChatSelectionViewModel(_currentUserService);
        var selectionWindow = new PoliceChatSelectionWindow(selectionVM);
        selectionWindow.ShowDialog();
    }

    private void Logout()
    {
        var serviceProvider = ((App)Application.Current).ServiceProvider;
        var loginWindow = serviceProvider.GetRequiredService<LoginWindow>();
        loginWindow.Show();
        Application.Current.MainWindow.Close();
    }
}
