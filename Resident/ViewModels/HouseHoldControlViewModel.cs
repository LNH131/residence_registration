﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Resident.DAO;
using Resident.Enums;
using Resident.Models;
using Resident.Service;
using Resident.View;

namespace Resident.ViewModels
{
    public partial class HouseHoldControlViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrentUserService _currentUserService;


        private bool _isSelected;
        private bool _isNewHead;
        private HouseholdMember _member;

        public User CurrentUser => _currentUserService.CurrentUser;

        private Household _selectedHousehold;
        public Household SelectedHousehold
        {
            get => _selectedHousehold;
            set
            {
                if (_selectedHousehold != value)
                {
                    _selectedHousehold = value;
                    OnPropertyChanged(nameof(SelectedHousehold));
                }
            }
        }

        private ObservableCollection<HouseholdMemberDisplayInfo> _householdMembers;
        public ObservableCollection<HouseholdMemberDisplayInfo> HouseholdMembers
        {
            get => _householdMembers;
            set
            {
                _householdMembers = value;
                OnPropertyChanged(nameof(HouseholdMembers));
            }
        }

        private string _householdAddress;
        public string HouseholdAddress
        {
            get => _householdAddress;
            set
            {
                if (_householdAddress != value)
                {
                    _householdAddress = value;
                    OnPropertyChanged(nameof(HouseholdAddress));
                }
            }
        }


        private ObservableCollection<HouseholdMember> _membersToSeparate;
        public ObservableCollection<HouseholdMember> MembersToSeparate
        {
            get => _membersToSeparate;
            set
            {
                _membersToSeparate = value;
                OnPropertyChanged(nameof(MembersToSeparate));
            }
        }


        public ICommand SelectHouseHoldCommand { get; set; }
        public ICommand SeparateHouseholdsCommand { get; set; }

        public HouseHoldControlViewModel(IServiceProvider serviceProvider, ICurrentUserService currentUserService)
        {
            _serviceProvider = serviceProvider;
            _currentUserService = currentUserService;

            // Khởi tạo collection để binding
            HouseholdMembers = new ObservableCollection<HouseholdMemberDisplayInfo>();

            SelectHouseHoldCommand = new RelayCommand(o => SelectHouseHold());

            SeparateHouseholdsCommand = new RelayCommand(o => SeparateHouseholds(), o => CanSeparateHouseholds());
        }

        private User GetUserById(int id)
        {
            using (var db = new PrnContext())
            {
                return db.Users.Find(id);
            }
        }

            private void SelectHouseHold()
            {
            
                var selectHouseHoldWindow = _serviceProvider.GetRequiredService<HouseHoldSelectionWindow>();
                bool? result = selectHouseHoldWindow.ShowDialog();
                if (result == true)
                {
                    // Sau khi cửa sổ đóng, lấy SelectedHousehold từ cửa sổ chọn và gán vào ViewModel.
                    Household selectedHousehold = selectHouseHoldWindow.SelectedHousehold;
                    // Giả sử cửa sổ chọn có property HouseholdMembers trả về danh sách các thành viên của hộ khẩu.
                    List<HouseholdMember> selectedMembers = selectHouseHoldWindow.SelectedHouseholdMembers;
                    Debug.WriteLine("Select HouseHold: asd: " + selectedMembers);

                SelectedHousehold = selectedHousehold;

                // Cập nhật danh sách thành viên
                if (selectedMembers != null)
                {
                    // Tạo danh sách mới gộp thông tin Household và Address.
                    var dataSource = selectedMembers.Select(h => 
                    {
                        User user = GetUserById(h.UserId);
                        return new HouseholdMemberDisplayInfo
                        {

                            FullName = user.FullName,
                            IdentityCard = user.IdentityCard,
                            Relationship = h.Relationship,
                            UserId = h.UserId // Lưu UserId
                        };

                    }).ToList();
                    HouseholdMembers = new ObservableCollection<HouseholdMemberDisplayInfo>(dataSource);
                }
                else
                {
                    HouseholdMembers.Clear();
                }

                // Cập nhật địa chỉ của hộ khẩu dưới dạng chuỗi đã format.
                if (selectedHousehold?.Address != null)
                {
                    HouseholdAddress = string.Join(", ", new[]
                    {
                        selectedHousehold.Address.Street,
                        selectedHousehold.Address.Ward,
                        selectedHousehold.Address.District,
                        selectedHousehold.Address.City,
                        selectedHousehold.Address.Country
                    }.Where(s => !string.IsNullOrWhiteSpace(s)));
                }
                else
                {
                    HouseholdAddress = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("Không có hộ khẩu nào được chọn.");
            }
        }
        private void SeparateHouseholds()
        {
            var selectedMembers = HouseholdMembers.Where(member => member.IsSelected).ToList();

            int newHeadCount = selectedMembers.Count(member => member.IsNewHead);
            if (newHeadCount > 1)
            {
                MessageBox.Show("Chỉ có thể chọn một chủ hộ mới.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (newHeadCount == 0 && selectedMembers.Count > 0)
            {
                MessageBox.Show("Phải chọn một chủ hộ mới.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Xử lý logic tách hộ
            foreach (var selectedMember in selectedMembers)
            {
                // Lấy UserId (đã lưu trữ trong HouseholdMemberDisplayInfo)
                int userId = selectedMember.UserId;

                Debug.WriteLine($"Selected Member: {selectedMember.FullName}, IsNewHead: {selectedMember.IsNewHead}, UserId: {userId}");
                // Thực hiện các hành động tách hộ, ví dụ:
                // 1. Tạo Household mới.
                // 2. Tạo HouseholdMember mới với Relationship = "Chủ hộ" nếu IsNewHead là true,
                //    hoặc với Relationship như cũ nếu IsNewHead là false.
                // 3. Thêm HouseholdMember mới vào Household mới.
                // 4. Cập nhật cơ sở dữ liệu.
                // ...

                Debug.WriteLine($"Selected Member: {selectedMember.FullName}, IsNewHead: {selectedMember.IsNewHead}, UserId: {userId}");
            }

            MessageBox.Show("Tách hộ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            // Clear selection and IsNewHead sau khi tách
            foreach (var member in HouseholdMembers)
            {
                member.IsSelected = false;
                member.IsNewHead = false;
            }
        }

        private bool CanSeparateHouseholds()
        {
            // Kiểm tra xem có thành viên nào được chọn không
            return HouseholdMembers.Any(member => member.IsSelected);
        }
    }
}
