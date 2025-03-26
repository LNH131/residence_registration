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

public class PoliceViewModel : BaseViewModel
{
    private readonly ICurrentUserService _currentUserService;
    private readonly RegistrationService _registrationService;
    private readonly PrnContext _context;
    private readonly IPoliceProcessingService _policeProcessingService;

    // Collection for approval items (Registrations, HouseholdTransfers, HouseholdSeparations)
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

            // Re-check CanExecute for ProcessCommand
            if (ProcessCommand is Resident.Service.LocalRelayCommand cmd)
            {
                cmd.RaiseCanExecuteChanged();
            }
        }
    }

    // Household monitoring
    private ObservableCollection<Household> _households;
    public ObservableCollection<Household> Households
    {
        get => _households;
        set
        {
            _households = value;
            OnPropertyChanged(nameof(Households));
        }
    }

    private Household _selectedHousehold;
    public Household SelectedHousehold
    {
        get => _selectedHousehold;
        set
        {
            _selectedHousehold = value;
            OnPropertyChanged(nameof(SelectedHousehold));
        }
    }

    // Commands on the Police dashboard
    public ICommand ProcessCommand { get; }
    public ICommand ViewDetailsCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ChatCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand ViewHouseholdDetailCommand { get; }
    public ICommand ViewReportCommand { get; }
    public ICommand NotificationCommand { get; }
    public ICommand ViewAllRegistrationsCommand { get; }

    public PoliceViewModel(
        ICurrentUserService currentUserService,
        IPoliceProcessingService policeProcessingService)
    {
        _currentUserService = currentUserService;
        _registrationService = new RegistrationService();
        _context = new PrnContext();
        _policeProcessingService = policeProcessingService;

        // Process final approvals
        ProcessCommand = new Resident.Service.LocalRelayCommand(
            async _ => await ProcessApprovalAsync(),
            _ => SelectedApprovalItem != null
        );

        // View details of the selected item
        ViewDetailsCommand = new Resident.Service.LocalRelayCommand(
            o => ViewDetails(o),
            o => o != null
        );

        // Refresh both approvals and households
        RefreshCommand = new Resident.Service.LocalRelayCommand(_ =>
        {
            LoadApprovalItems();
            LoadHouseholds();
        });

        // Open chat
        ChatCommand = new Resident.Service.LocalRelayCommand(_ => OpenChat());

        // Possibly implement or set in code-behind
        // LogoutCommand = new Resident.Service.LocalRelayCommand(_ => Logout());

        ViewHouseholdDetailCommand = new Resident.Service.LocalRelayCommand(_ => OpenHouseholdDetail());
        ViewReportCommand = new Resident.Service.LocalRelayCommand(_ => OpenReports());
        NotificationCommand = new Resident.Service.LocalRelayCommand(_ => OpenNotifications());
        ViewAllRegistrationsCommand = new Resident.Service.LocalRelayCommand(_ => OpenAllRegistrations());

        // Initial load
        LoadApprovalItems();
        LoadHouseholds();
    }

    /// <summary>
    /// Loads all ApprovalItems (Registrations, Transfers, Separations) with status ApprovedByLeader.
    /// </summary>
    private void LoadApprovalItems()
    {
        ApprovalItems.Clear();

        // 1) Registrations
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

        // 2) HouseholdTransfers
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

        // 3) HouseholdSeparations
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

    /// <summary>
    /// Loads all households for the "Household Monitoring" section.
    /// </summary>
    private void LoadHouseholds()
    {
        var allHouseholds = _context.Households
            .Include(h => h.Address)
            .ToList();
        Households = new ObservableCollection<Household>(allHouseholds);
    }

    /// <summary>
    /// Processes the selected approval item (final processing by Police).
    /// </summary>
    private async Task ProcessApprovalAsync()
    {
        if (SelectedApprovalItem == null) return;

        int currentPoliceId = _currentUserService.CurrentUser.UserId;

        try
        {
            switch (SelectedApprovalItem.ItemType)
            {
                case "Registration":
                    if (SelectedApprovalItem.UnderlyingItem is Registration reg)
                    {
                        await _policeProcessingService.ProcessRegistrationAsync(reg, currentPoliceId);
                        MessageBox.Show($"Registration ID {reg.RegistrationId} processed by Police.",
                                        "Success", MessageBoxButton.OK);
                    }
                    break;

                case "HouseholdTransfer":
                    if (SelectedApprovalItem.UnderlyingItem is HouseholdTransfer transfer)
                    {
                        await _policeProcessingService.ProcessHouseholdTransferAsync(transfer, currentPoliceId);
                        MessageBox.Show($"Household Transfer ID {transfer.TransferId} processed by Police.",
                                        "Success", MessageBoxButton.OK);
                    }
                    break;

                case "HouseholdSeparation":
                    if (SelectedApprovalItem.UnderlyingItem is HouseholdSeparation separation)
                    {
                        await _policeProcessingService.ProcessHouseholdSeparationAsync(separation, currentPoliceId);
                        MessageBox.Show($"Household Separation ID {separation.SeparationId} processed by Police.",
                                        "Success", MessageBoxButton.OK);
                    }
                    break;
            }
        }
        catch (System.Exception ex)
        {
            MessageBox.Show($"Error processing approval: {ex.Message}",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Refresh the list after processing.
        LoadApprovalItems();
    }

    /// <summary>
    /// Opens the details window for the selected approval item.
    /// </summary>
    private void ViewDetails(object parameter)
    {
        if (parameter is not ApprovalItem item) return;

        switch (item.ItemType)
        {
            case "Registration":
                if (item.UnderlyingItem is Registration reg)
                {
                    var detailsVM = new RegistrationDetailsViewModel(reg, _currentUserService);
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

    /// <summary>
    /// Opens the PoliceChatSelectionWindow.
    /// </summary>
    private void OpenChat()
    {
        var selectionVM = new PoliceChatSelectionViewModel(_currentUserService, _context);
        var selectionWindow = new PoliceChatSelectionWindow(selectionVM);
        selectionWindow.ShowDialog();
    }

    /// <summary>
    /// Opens the HouseholdDetailsWindow for the selected household in the monitoring grid.
    /// </summary>
    private void OpenHouseholdDetail()
    {
        if (SelectedHousehold == null)
        {
            MessageBox.Show("Please select a household first.", "Info");
            return;
        }

        var householdDetailsWindow = new HouseholdDetailsWindow(new HouseholdDetailsViewModel());
        householdDetailsWindow.ShowDialog();
    }

    private void OpenReports()
    {
        MessageBox.Show("Report functionality is under development.", "Reports");
    }

    /// <summary>
    /// Opens the PoliceNotificationWindow from the DI container.
    /// </summary>
    private void OpenNotifications()
    {
        var serviceProvider = ((App)Application.Current).ServiceProvider;
        var notifWindow = serviceProvider.GetRequiredService<PoliceNotificationWindow>();
        notifWindow.ShowDialog();
    }

    /// <summary>
    /// Opens the PoliceApprovalsOverviewWindow for the current policeman.
    /// </summary>
    private void OpenAllRegistrations()
    {
        var vm = new PoliceApprovalsOverviewViewModel(_currentUserService, _policeProcessingService);
        var window = new PoliceApprovalsOverviewWindow(vm);
        window.ShowDialog();
    }

    // If you want a logout command in MVVM style, you can do something like:
    // private void Logout()
    // {
    //     var serviceProvider = ((App)Application.Current).ServiceProvider;
    //     var loginWindow = serviceProvider.GetRequiredService<LoginWindow>();
    //     loginWindow.Show();
    //     Application.Current.MainWindow.Close();
    // }
}
