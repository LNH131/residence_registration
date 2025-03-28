﻿using Resident.Enums;
using Resident.Models;
using Resident.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace Resident.View
{
    /// <summary>
    /// Interaction logic for HouseHoldControlWindow.xaml
    /// </summary>
    public partial class HouseHoldControlWindow : Window
    {
        private readonly HouseHoldControlViewModel _viewModel;
        private Household _selectedHousehold;
        private readonly User _currentUser;

        public User User => _currentUser;
        private ObservableCollection<HouseholdMember> _householdMembers;

        public HouseHoldControlWindow(HouseHoldControlViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _currentUser = viewModel.CurrentUser;
            DataContext = _viewModel; // Set DataContext cho binding tự động trong XAML

            _householdMembers = new ObservableCollection<HouseholdMember>();
            dgHouseholdMembers.ItemsSource = _householdMembers;

            // Đăng ký sự kiện để theo dõi khi SelectedHousehold thay đổi
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedHousehold))
            {
                _selectedHousehold = _viewModel.SelectedHousehold;
                Debug.WriteLine("Selected Household updated: " + _selectedHousehold?.HouseholdId);
                // Bạn có thể thực hiện các hành động khác khi SelectedHousehold thay đổi tại đây.
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

            // Kiểm tra nếu CCCD trùng với của chủ hộ
            if (user.UserId == _currentUser.UserId)
            {
                MessageBox.Show("Chủ hộ không thể được thêm vào danh sách thành viên.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
            txtMemberIdentityNumber.Clear();
            txtMemberRelationship.Clear();
        }

        private User GetUserById(string identityCard)
        {
            using (var context = new PrnContext())
            {
                return context.Users.FirstOrDefault(u => u.IdentityCard == identityCard);
            }
        }

        // Updated method to get household based on new model
        private Household GetHouseholdByUserId()
        {
            using (var context = new PrnContext())
            {
                // Look up the head record for the current user
                var headOfHouse = context.HeadOfHouseHolds.FirstOrDefault(h => h.UserId == _currentUser.UserId);
                if (headOfHouse == null || headOfHouse.HouseholdId == null)
                    return null;
                // Return the household associated with the head record
                return context.Households.FirstOrDefault(h => h.HouseholdId == headOfHouse.HouseholdId);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra nếu các trường thông tin địa chỉ không được để trống
            if (string.IsNullOrWhiteSpace(StreetTextBox.Text) ||
                string.IsNullOrWhiteSpace(WardTextBox.Text) ||
                string.IsNullOrWhiteSpace(DistrictTextBox.Text) ||
                string.IsNullOrWhiteSpace(CityTextBox.Text) ||
                string.IsNullOrWhiteSpace(CountryTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin địa chỉ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra có thêm thành viên nào trong hộ hay không
            if (_householdMembers.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một thành viên vào hộ khẩu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using (var context = new PrnContext())
            {
                bool isHead = context.HeadOfHouseHolds.Any(h => h.UserId == _currentUser.UserId);
                bool isMember = context.HouseholdMembers.Any(m => m.UserId == _currentUser.UserId);
                if (isHead || isMember)
                {
                    MessageBox.Show("Bạn đã có hộ khẩu hoặc đã là thành viên của hộ khẩu. Mỗi người chỉ được đăng ký một hộ khẩu.",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            using (var context = new PrnContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (_currentUser == null)
                        {
                            MessageBox.Show("Không tìm thấy thông tin người dùng!");
                            return;
                        }

                        // Tạo và lưu địa chỉ
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

                        // Tạo và lưu đăng ký hộ khẩu
                        Registration registration = new Registration
                        {
                            UserId = _currentUser.UserId,
                            AddressId = address.AddressId,
                            RegistrationType = RegistrationType.HouseholdRegistration.ToString(),
                            StartDate = DateOnly.FromDateTime(DateTime.Now),
                            EndDate = null,
                            Status = Status.Pending.ToString(),
                            ApprovedBy = null,
                            Comments = null
                        };

                        context.Registrations.Add(registration);
                        context.SaveChanges();

                        // Thêm các thành viên vào bảng RegistrationMembers dựa trên danh sách _householdMembers
                        foreach (var member in _householdMembers)
                        {
                            var regMember = new RegistrationMember
                            {
                                RegistrationId = registration.RegistrationId,
                                FullName = member.User != null ? member.User.FullName : "",
                                Relationship = member.Relationship,
                                IdentityCard = member.User?.IdentityCard,
                                Birthday = member.User?.Birthday,
                                Sex = member.User?.Sex
                            };

                            context.RegistrationMembers.Add(regMember);
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

        private void Register_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
