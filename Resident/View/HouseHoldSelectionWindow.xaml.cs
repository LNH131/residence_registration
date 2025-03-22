using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using Resident.ViewModels;

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

        public HouseHoldSelectionWindow(HouseHoldSelectionViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _currentUser = viewModel.CurrentUser;
            DataContext = _viewModel;

            Debug.WriteLine("test: " + _currentUser.UserId);

            LoadHouseholds();
        }

        private void LoadHouseholds()
        {
            List<Household> households = GetHouseholds(); // Lấy danh sách hộ khẩu của người dùng hiện tại.
            Address address = GetAddress(_currentUser.CurrentAddressId);

            Debug.WriteLine("Address: " + address.AddressId);
            string formattedAddress = string.Join(", ", new[]
            {
                address.Street,
                address.Ward,
                address.District,
                address.City,
                address.Country
            }.Where(s => !string.IsNullOrWhiteSpace(s)));
            Debug.WriteLine("Households: " + households.Count);

            // Tạo danh sách mới gộp thông tin Household và Address.
            var dataSource = households.Select(h => new
            {
                h.HouseholdId,
                // Các thuộc tính khác của Household nếu cần,
                FormattedAddress = formattedAddress       // Ví dụ: Thành phố
                                                          // Thêm các thuộc tính khác từ Address nếu cần.
            }).ToList();

            dgHouseholds.ItemsSource = dataSource;

        }

        private List<Household> GetHouseholds()
        {
            HeadOfHouseHold headOfHouseHold = GetHeadOfHouseHold(_currentUser.UserId);
            using (var context = new PrnContext()) // Thay bằng DbContext của bạn.
            {
                // Lấy danh sách hộ khẩu của người dùng hiện tại. Cần join để lấy thông tin địa chỉ
                var households = context.Households
                    .Where(h => h.HeadId == headOfHouseHold.HeadOfHouseHoldId) // Lọc theo người dùng hiện tại
                    .ToList();
                return households;
            }
        }

        private HeadOfHouseHold GetHeadOfHouseHold(int headId)
        {
            using (var context = new PrnContext())
            {
                return context.HeadOfHouseHolds.FirstOrDefault(h => h.UserId == headId);
            }
        }

        private Address GetAddress(int addressId)
        {
            using (var context = new PrnContext())
            {
                return context.Addresses.FirstOrDefault(a => a.AddressId == addressId);
            }
        }

        private List<HouseholdMember> GetHouseholdMember(int householdId)
        {
            using (var context = new PrnContext())
            {
                return context.HouseholdMembers
                              .Where(h => h.HouseholdId == householdId)
                              .ToList();
            }
        }


        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có mục nào được chọn trong DataGrid không.
            if (dgHouseholds.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một hộ khẩu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Vì ItemsSource của DataGrid là danh sách kiểu ẩn danh chứa thuộc tính HouseholdId,
            // ta dùng dynamic để truy xuất thuộc tính này.
            dynamic selectedItem = dgHouseholds.SelectedItem;
            int selectedHouseholdId = selectedItem.HouseholdId;

            // Sử dụng DbContext để truy xuất thông tin chi tiết của hộ khẩu đã chọn, bao gồm cả thông tin địa chỉ.
            using (var context = new PrnContext())
            {
                // Sử dụng Include nếu bạn muốn load kèm theo thông tin liên quan (ví dụ: Address)
                SelectedHousehold = context.Households
                    .Include("Address")
                    .FirstOrDefault(h => h.HouseholdId == selectedHouseholdId);
            }

            if (SelectedHousehold != null)
            {
                List<HouseholdMember> householdMember = GetHouseholdMember(SelectedHousehold.HouseholdId);
                SelectedHouseholdMembers = householdMember;
                DialogResult = true;
                string formattedAddress = string.Join(", ", new[]
                {
                    SelectedHousehold.Address.Street,
                    SelectedHousehold.Address.Ward,
                    SelectedHousehold.Address.District,
                    SelectedHousehold.Address.City,
                    SelectedHousehold.Address.Country
                }.Where(s => !string.IsNullOrWhiteSpace(s)));
                Close();
            }
            else
            {
                MessageBox.Show("Không tìm thấy hộ khẩu đã chọn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
