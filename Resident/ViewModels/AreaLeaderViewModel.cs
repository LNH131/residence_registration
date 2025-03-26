using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;
using Resident.Service;
using Resident.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class AreaLeaderViewModel : BaseViewModel
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly RegistrationService _registrationService;
        private readonly PrnContext _context;
        private readonly INotificationService _notificationService;
        private readonly IPoliceProcessingService _policeProcessingService;

        // Collection of approval items (Registration, HouseholdTransfer, HouseholdSeparation)
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

        // Dashboard commands
        public ICommand ViewDetailsCommand { get; }
        public ICommand ViewNotificationsCommand { get; }
        public ICommand ChatCommand { get; }
        public ICommand ViewAllRegistrationsCommand { get; }
        public ICommand ViewApprovalsOverviewCommand { get; }
        public IAsyncRelayCommand SendNotificationToCitizensCommand { get; }
        public ICommand OpenNotificationWindowCommand { get; }

        public AreaLeaderViewModel(
            ICurrentUserService currentUserService,
            INotificationService notificationService,
            IPoliceProcessingService policeProcessingService)
        {
            _currentUserService = currentUserService;
            _notificationService = notificationService;
            _policeProcessingService = policeProcessingService;
            _registrationService = new RegistrationService();
            _context = new PrnContext();

            // Load initial approval items
            LoadApprovalItems();

            // Initialize commands
            ViewDetailsCommand = new LocalRelayCommand(o => ViewDetails(o), o => o != null);
            ViewNotificationsCommand = new LocalRelayCommand(_ => ViewNotifications());
            ChatCommand = new LocalRelayCommand(_ => Chat());
            ViewAllRegistrationsCommand = new LocalRelayCommand(_ => ViewAllRegistrations());
            ViewApprovalsOverviewCommand = new LocalRelayCommand(_ => OpenAllItemsWindow());
            SendNotificationToCitizensCommand = new AsyncRelayCommand(SendNotificationToCitizensAsync, () => true);
            OpenNotificationWindowCommand = new LocalRelayCommand(_ => OpenNotificationWindow());
        }

        /// <summary>
        /// Loads all pending approval items from Registrations, HouseholdTransfers, and HouseholdSeparations.
        /// </summary>
        private void LoadApprovalItems()
        {
            ApprovalItems.Clear();

            // Load Registrations with status Pending
            var regs = _registrationService.GetPendingRegistrations();
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

            // Load HouseholdTransfers with status Pending
            var transfers = _context.HouseholdTransfers
                .Include(t => t.Household)
                    .ThenInclude(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
                .Include(t => t.FromAddress)
                .Include(t => t.ToAddress)
                .Where(t => t.Status == Status.Pending.ToString())
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

            // Load HouseholdSeparations with status Pending
            var separations = _context.HouseholdSeparations
                .Include(s => s.OriginalHousehold)
                    .ThenInclude(h => h.HeadOfHouseHold)
                        .ThenInclude(hh => hh.User)
                .Where(s => s.Status == Status.Pending.ToString())
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
        /// Opens the details window for the selected approval item.
        /// </summary>
        private void ViewDetails(object parameter)
        {
            if (parameter is not ApprovalItem item) return;

            switch (item.ItemType)
            {
                case "Registration":
                    if (item.UnderlyingItem is Registration registration)
                    {
                        var detailsVM = new AreaLeaderRegistrationDetailsViewModel(registration, _currentUserService);
                        var detailsWindow = new AreaLeaderRegistrationDetailsWindow(detailsVM);
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
        /// Displays a placeholder message for notifications.
        /// </summary>
        private void ViewNotifications()
        {
            MessageBox.Show("Notification functionality is under development.", "Notifications");
        }

        /// <summary>
        /// Opens the chat selection window.
        /// </summary>
        private void Chat()
        {
            var selectionVM = new AreaLeaderChatSelectionViewModel(_currentUserService, _context);
            var selectionWindow = new AreaLeaderChatSelectionWindow(selectionVM);
            selectionWindow.ShowDialog();
        }

        /// <summary>
        /// Opens a window showing all registrations.
        /// </summary>
        private void ViewAllRegistrations()
        {
            var overviewVM = new RegistrationOverviewViewModel(_currentUserService, _registrationService);
            var overviewWindow = new RegistrationOverviewWindow(overviewVM);
            overviewWindow.Show();
        }

        /// <summary>
        /// Opens a window that shows an overview of all approval items.
        /// </summary>
        private void OpenAllItemsWindow()
        {
            var approvalsOverviewVM = new AreaLeaderApprovalsOverviewViewModel(_currentUserService, _policeProcessingService);
            var approvalsOverviewWindow = new AreaLeaderApprovalsOverviewWindow(approvalsOverviewVM);
            approvalsOverviewWindow.ShowDialog();
        }

        /// <summary>
        /// Sends an in-app notification to all citizens.
        /// </summary>
        private async Task SendNotificationToCitizensAsync()
        {
            // Define the notification message.
            string message = "Thông báo từ Tổ trưởng khu phố: Vui lòng cập nhật thông tin hộ khẩu mới.";

            // Retrieve all users with role "Citizen"
            var citizens = _context.Users.Where(u => u.Role == "Citizen").ToList();
            foreach (var citizen in citizens)
            {
                await _notificationService.SendNotificationAsync(citizen.UserId, message);
            }

            MessageBox.Show("Thông báo đã được gửi đến tất cả cư dân.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Opens a window for creating a new notification.
        /// </summary>
        private void OpenNotificationWindow()
        {
            // Create the VM
            var createNotificationVM = new CreateNotificationViewModel(_notificationService, new PrnContext());
            // Create the window, pass in the VM
            var createNotificationWindow = new CreateNotificationWindow(createNotificationVM);
            // Show it modally
            createNotificationWindow.ShowDialog();
        }
    }
}
