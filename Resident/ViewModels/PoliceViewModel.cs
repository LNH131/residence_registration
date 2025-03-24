using Microsoft.Extensions.DependencyInjection;
using Resident.Models;
using Resident.Service;
using Resident.Services; // Assuming HouseholdService is here
using Resident.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class PoliceViewModel : BaseViewModel
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IHouseholdService _householdService;
        private HouseholdDetailViewModel _selectedHousehold;

        public ObservableCollection<HouseholdDetailViewModel> Households { get; set; }
        public ObservableCollection<Registration> PendingApprovals { get; set; }

        // Commands
        public ICommand ChatCommand { get; }
        public ICommand ViewHouseholdDetailCommand { get; }
        public ICommand ViewReportCommand { get; }
        public ICommand NotificationCommand { get; }
        public ICommand LogoutCommand { get; }

        public HouseholdDetailViewModel SelectedHousehold
        {
            get => _selectedHousehold;
            set { _selectedHousehold = value; OnPropertyChanged(); }
        }

        public PoliceViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            //_householdService = householdService;

            Households = new ObservableCollection<HouseholdDetailViewModel>();
            PendingApprovals = new ObservableCollection<Registration>();

            ChatCommand = new RelayCommand(o => OpenChatSelection());
            ViewHouseholdDetailCommand = new RelayCommand(o => OpenHouseholdDetail(), o => SelectedHousehold != null);
            ViewReportCommand = new RelayCommand(o => OpenReports());
            NotificationCommand = new RelayCommand(o => OpenNotifications());
            LogoutCommand = new RelayCommand(o => Logout());

            LoadHouseholdsAsync();
        }

        private async void LoadHouseholdsAsync()
        {
            // For demonstration, we load all households.
            // In a real app, you might filter based on pending registrations or police area.
            var households = await _householdService.GetAllHouseholdsAsync();
            Households.Clear();
            foreach (var household in households)
            {
                Households.Add(household);
            }
        }

        private void OpenChatSelection()
        {
            // Open the selection window for the police to choose a chat partner.
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var selectionVM = serviceProvider.GetRequiredService<PoliceChatSelectionViewModel>();
            var selectionWindow = new PoliceChatSelectionWindow(selectionVM);
            selectionWindow.ShowDialog();
        }

        private void OpenHouseholdDetail()
        {
            // You could open a detailed window or simply display the details in a side panel.
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var detailVM = serviceProvider.GetRequiredService<HouseholdDetailViewModel>(); // Or pass SelectedHousehold
            // For simplicity, display details in a MessageBox. Replace this with your custom detail view.
            MessageBox.Show($"Household ID: {SelectedHousehold.HouseholdId}\n" +
                            $"Address: {SelectedHousehold.Address.Street}, {SelectedHousehold.Address.City}",
                            "Household Detail");
        }

        private void OpenReports()
        {
            // Implement your report viewing functionality here.
            MessageBox.Show("Report view is not yet implemented.", "Reports");
        }

        private void OpenNotifications()
        {
            // Implement your notifications view functionality here.
            MessageBox.Show("Notifications view is not yet implemented.", "Notifications");
        }

        private void Logout()
        {
            // Resolve the LoginWindow via DI.
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var loginWindow = serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
            Application.Current.MainWindow.Close();
        }
    }
}
