using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Resident.DAO;
using Resident.Models;

namespace Resident.View
{
    /// <summary>
    /// Interaction logic for HouseHoldControlWindow.xaml
    /// </summary>
    public partial class HouseHoldControlWindow : Window
    {
        private ObservableCollection<HouseholdMember> _householdMembers;

        public HouseHoldControlWindow(User currentUser, UserDAO userDAO)
        {
            InitializeComponent();
            DataContext = currentUser; // Set the DataContext to the current user

            _householdMembers = new ObservableCollection<HouseholdMember>();
            dgHouseholdMembers.ItemsSource = _householdMembers;
        }

        private void AddMember_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtMemberFullName.Text.Trim();
            string identityCard = txtMemberIdentityNumber.Text.Trim();
            string relationship = txtMemberRelationship.Text.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(relationship))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin thành viên.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(identityCard))
            {
                MessageBox.Show("Vui lòng nhập CMND/CCCD.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User user = GetUserById(identityCard);

            if (user == null)
            {
                MessageBox.Show("Không tìm thấy user trong database.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMemberFullName.Clear();
                txtMemberIdentityNumber.Clear();
                txtMemberRelationship.Clear();
                return;
            }
            if (_householdMembers.Any(m => m.UserId == user.UserId))
            {
                MessageBox.Show("Thành viên này đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var member = new HouseholdMember
            {
                UserId = user.UserId,
                Relationship = relationship,
                User = user
            };

            _householdMembers.Add(member);

            txtMemberFullName.Clear();
            txtMemberRelationship.Clear();
        }



        private User GetUserById(string identityCard)
        {
            using (var context = new PrnContext())
            {
                return context.Users.FirstOrDefault(u => u.IdentityCard == identityCard);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StreetTextBox.Text) ||
                string.IsNullOrWhiteSpace(WardTextBox.Text) ||
                string.IsNullOrWhiteSpace(DistrictTextBox.Text) ||
                string.IsNullOrWhiteSpace(CityTextBox.Text) ||
                string.IsNullOrWhiteSpace(CountryTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin địa chỉ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_householdMembers.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một thành viên vào hộ khẩu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new PrnContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var currentUser = this.DataContext as User;
                        if (currentUser == null)
                        {
                            MessageBox.Show("Không tìm thấy thông tin người dùng!");
                            return;
                        }

                        Address address = new Address
                        {
                            Street = StreetTextBox.Text,
                            Ward = WardTextBox.Text,
                            District = DistrictTextBox.Text,
                            City = CityTextBox.Text,
                            State = string.Empty,
                            ZipCode = string.Empty,
                            Country = CountryTextBox.Text
                        };
                        context.Addresses.Add(address);
                        context.SaveChanges();

                        HeadOfHouseHold head = new HeadOfHouseHold
                        {
                            UserId = currentUser.UserId,
                            RegisteredDate = DateTime.Now,
                        };
                        context.HeadOfHouseHolds.Add(head);
                        context.SaveChanges();

                        Household household = new Household
                        {
                            HeadId = head.HeadOfHouseHoldId,
                            AddressId = address.AddressId,
                            CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                        };
                        context.Households.Add(household);
                        context.SaveChanges();

                        head.HouseholdId = household.HouseholdId;
                        context.SaveChanges();

                        foreach (var member in _householdMembers)
                        {
                            member.HouseholdId = household.HouseholdId;
                            context.Users.Attach(member.User); // Attach existing User
                            context.HouseholdMembers.Add(member);
                        }
                        context.SaveChanges();

                        transaction.Commit();
                        MessageBox.Show("Đăng ký hộ khẩu thành công!");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message + "\n\n" + ex.InnerException?.Message + "\n\n" + ex.StackTrace);
                    }
                }
            }
        }
        private void DeleteMember_Click(object sender, RoutedEventArgs e)
        {
            if (dgHouseholdMembers.SelectedItem is HouseholdMember selectedMember)
            {
                _householdMembers.Remove(selectedMember);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một thành viên để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}