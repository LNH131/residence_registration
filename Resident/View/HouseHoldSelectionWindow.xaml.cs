using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace Resident.View
{
    /// <summary>
    /// Interaction logic for HouseHoldSelectionWindow.xaml
    /// </summary>
    public partial class HouseHoldSelectionWindow : Window
    {
        public Household SelectedHousehold { get; private set; }
        public List<HouseholdMember> SelectedHouseholdMembers { get; private set; }

        private User _currentUser;
        private HouseHoldSelectionViewModel _viewModel;
        private ObservableCollection<HouseholdMember> _householdMembers;

        public HouseHoldSelectionWindow(HouseHoldSelectionViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _currentUser = viewModel.CurrentUser;
            DataContext = _viewModel; // Enable XAML binding

            Debug.WriteLine("Current User Id: " + _currentUser.UserId);

            _householdMembers = new ObservableCollection<HouseholdMember>();
            dgHouseholds.ItemsSource = null;  // Initially no selection

            // Load households for which the current user is head
            LoadHouseholds();
        }

        // Loads households where the current user is the head.
        private void LoadHouseholds()
        {
            List<Household> households = GetHouseholds();

            if (households == null || households.Count == 0)
            {
                Debug.WriteLine("No households found for the current user.");
                dgHouseholds.ItemsSource = null;
                return;
            }

            // For display, use the Address from each household to create a formatted address.
            var dataSource = households.Select(h => new
            {
                h.HouseholdId,
                FormattedAddress = string.Join(", ", new[]
                {
                    h.Address.Street,
                    h.Address.Ward,
                    h.Address.District,
                    h.Address.City,
                    h.Address.Country
                }.Where(s => !string.IsNullOrWhiteSpace(s)))
            }).ToList();

            dgHouseholds.ItemsSource = dataSource;
        }

        // Retrieves households where the current user is the head.
        private List<Household> GetHouseholds()
        {
            using (var context = new PrnContext())
            {
                return context.Households
                    .Include(h => h.Address)
                    .Include(h => h.HeadOfHouseHold)
                    .Where(h => h.HeadOfHouseHold != null && h.HeadOfHouseHold.UserId == _currentUser.UserId)
                    .ToList();
            }
        }

        // Retrieves the address by Id.
        private Address GetAddress(int addressId)
        {
            using (var context = new PrnContext())
            {
                return context.Addresses.FirstOrDefault(a => a.AddressId == addressId);
            }
        }

        // Retrieves HouseholdMembers for a given HouseholdId.
        private List<HouseholdMember> GetHouseholdMember(int householdId)
        {
            using (var context = new PrnContext())
            {
                return context.HouseholdMembers
                              .Where(h => h.HouseholdId == householdId)
                              .Include(h => h.User)
                              .ToList();
            }
        }

        // Handler for the "Chọn" (Select) button.
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a household is selected.
            if (dgHouseholds.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một hộ khẩu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Since the ItemsSource is a list of anonymous objects, we use dynamic to access the HouseholdId.
            dynamic selectedItem = dgHouseholds.SelectedItem;
            int selectedHouseholdId = selectedItem.HouseholdId;

            // Retrieve the full household record including its Address and head info.
            using (var context = new PrnContext())
            {
                SelectedHousehold = context.Households
                    .Include(h => h.Address)
                    .Include(h => h.HeadOfHouseHold)
                    .FirstOrDefault(h => h.HouseholdId == selectedHouseholdId);
            }

            if (SelectedHousehold != null)
            {
                SelectedHouseholdMembers = GetHouseholdMember(SelectedHousehold.HouseholdId);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Không tìm thấy hộ khẩu đã chọn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Handler for the "Hủy" (Cancel) button.
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
