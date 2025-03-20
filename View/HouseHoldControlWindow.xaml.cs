using System;
using System.Collections.Generic;
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
using Project.Models;
using Project.DAO;
using System.Collections.ObjectModel;
using Project.ViewModels;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Project.View
{
    /// <summary>
    /// Interaction logic for HouseHoldControlWindow.xaml
    /// </summary>
    public partial class HouseHoldControlWindow : Window
    {
        private readonly CitizenViewModel _citizenViewModel;
        private ObservableCollection<HouseholdMember> _householdMembers;
        private readonly UserDAO _userDAO;
        private PrnContext PrnContext = new PrnContext();

        public HouseHoldControlWindow(User currentUser, UserDAO userDAO)
        {
            InitializeComponent();
            DataContext = currentUser;
            _userDAO = userDAO;

            _householdMembers = new ObservableCollection<HouseholdMember>();
            dgHouseholdMembers.ItemsSource = _householdMembers;
        }

        private void AddMember_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input values
            string fullName = txtMemberFullName.Text.Trim();
            string identityCard = txtMemberIdentityNumber.Text.Trim();
            string relationship = txtMemberRelationship.Text.Trim();

            Debug.WriteLine("Add member: " + fullName + ", " + identityCard + ", " + relationship);

            // Nếu chưa có thành viên nào, yêu cầu nhập CCCD
            if (_householdMembers.Count == 0 && string.IsNullOrEmpty(identityCard))
            {
                MessageBox.Show("Vui lòng nhập CMND/CCCD.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Nếu đã có thành viên, có thể bỏ qua validate CCCD hoặc sử dụng giá trị mặc định (tùy theo logic của bạn)
            // Ví dụ: nếu chưa nhập CCCD thì dùng giá trị của thành viên đầu tiên
            if (_householdMembers.Count > 0 && string.IsNullOrEmpty(identityCard))
            {
                identityCard = _householdMembers[0].User.IdentityCard;
            }

            // Call your method to get the user by identity card
            User user = GetUserById(identityCard);
            Debug.WriteLine("Userff: " + user);

            // Check if user exists
            if (user == null)
            {
                MessageBox.Show("Không tìm thấy user trong database.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMemberFullName.Clear();
                // Nếu chưa có thành viên, clear luôn CCCD; nếu đã có, giữ lại giá trị đó có thể giúp thêm dễ dàng
                if (_householdMembers.Count == 0)
                {
                    txtMemberIdentityNumber.Clear();
                }
                txtMemberRelationship.Clear();
            }
            else
            {
                // Tạo đối tượng HouseholdMember với thông tin từ user
                var member = new HouseholdMember
                {
                    UserId = user.UserId,
                    Relationship = relationship,
                    User = user  // Gán navigation property để hiển thị thông tin từ User
                                 // HouseholdId sẽ được cập nhật sau khi tạo Household
                };

                // Thêm đối tượng vừa tạo vào collection
                _householdMembers.Add(member);

                // Sau đó xóa các trường nhập liệu nếu cần. 
                // Nếu bạn muốn giữ lại CCCD cho các lần thêm sau, chỉ clear FullName và Relationship.
                txtMemberFullName.Clear();
                txtMemberRelationship.Clear();
            }
        }


        private User GetUserById(string identityCard)
        {
            return PrnContext.Users.FirstOrDefault(u => u.IdentityCard == identityCard);
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new PrnContext())
            {
                // Sử dụng transaction để đảm bảo tính toàn vẹn của dữ liệu
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Lấy thông tin người dùng hiện tại (giả sử DataContext của form là đối tượng User)
                        var currentUser = this.DataContext as User;
                        if (currentUser == null)
                        {
                            MessageBox.Show("Không tìm thấy thông tin người dùng!");
                            return;
                        }

                        // 2. Tạo đối tượng Address từ thông tin trên form
                        Address address = new Address
                        {
                            Street = StreetTextBox.Text,
                            Ward = WardTextBox.Text,          // Sử dụng WardTextBox cho "Phường/Xã"
                            District = DistrictTextBox.Text,  // Sử dụng DistrictTextBox cho "Quận/Huyện"
                            City = CityTextBox.Text,          // Sử dụng CityTextBox cho "Tỉnh/Thành phố"
                            State = string.Empty,             // Nếu không có thông tin State, gán mặc định
                            ZipCode = string.Empty,           // Nếu không có thông tin ZipCode, gán mặc định
                            Country = CountryTextBox.Text
                        };
                        context.Addresses.Add(address);
                        context.SaveChanges(); // Lưu để có address.AddressId

                        // 3. Tạo đối tượng HeadOfHouseHold với thông tin người dùng hiện tại
                        HeadOfHouseHold head = new HeadOfHouseHold
                        {
                            UserId = currentUser.UserId,
                            RegisteredDate = DateTime.Now,
                            // HouseholdId sẽ được cập nhật sau khi tạo Household
                        };
                        context.HeadOfHouseHolds.Add(head);
                        context.SaveChanges(); // Lưu để có head.HeadId

                        // 4. Tạo đối tượng Household liên kết HeadOfHouseHold và Address
                        Household household = new Household
                        {
                            HeadId = head.HeadOfHouseHoldId,
                            AddressId = address.AddressId,
                            CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                        };
                        Debug.WriteLine("Household: " + household.HeadId);
                        Debug.WriteLine("AddressId: " + household.AddressId);
                        Debug.WriteLine("CreatedDate: " + household.CreatedDate);

                        context.Households.Add(household);
                        context.SaveChanges(); // Lưu để có household.HouseholdId

                        //5.Cập nhật lại HeadOfHouseHold nếu cần(ví dụ: lưu HouseholdId cho chủ hộ)
                        head.HouseholdId = household.HouseholdId;
                        context.SaveChanges();

                        //6.Xử lý thêm danh sách các thành viên trong hộ từ DataGrid
                        //Giả sử bạn có các đối tượng HouseholdMember được binding vào DataGrid dgHouseholdMembers
                        foreach (var item in dgHouseholdMembers.Items)
                        {
                            if (item is HouseholdMember member)
                            {
                                // Gán HouseholdId cho từng thành viên
                                // (Nếu trong model HouseholdMember có thuộc tính HouseholdId, bạn nên cập nhật nó)
                                member.HouseholdId = household.HouseholdId;
                                context.HouseholdMembers.Add(member);
                            }
                        }
                        context.SaveChanges();

                        // Commit giao dịch khi tất cả các thao tác thành công
                        transaction.Commit();
                        MessageBox.Show("Đăng ký hộ khẩu thành công!");
                    }
                    catch (Exception ex)
                    {
                        // Rollback nếu có lỗi xảy ra
                        transaction.Rollback();
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                    }
                }
            }
        }

    }
}




