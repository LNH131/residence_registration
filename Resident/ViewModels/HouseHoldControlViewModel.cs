using Microsoft.EntityFrameworkCore; // Cần thêm namespace này
using Microsoft.Extensions.DependencyInjection;
using Resident.Enums;
using Resident.Models;
using Resident.Service;
using Resident.View;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public partial class HouseHoldControlViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrentUserService _currentUserService;

        // Các thuộc tính binding cho địa chỉ chuyển đến
        public string TransferStreet { get; set; }
        public string TransferWard { get; set; }
        public string TransferDistrict { get; set; }
        public string TransferCity { get; set; }
        public string TransferCountry { get; set; }

        private Household _currentHousehold;
        public Household CurrentHousehold
        {
            get => _currentHousehold;
            set
            {
                if (_currentHousehold != value)
                {
                    _currentHousehold = value;
                    OnPropertyChanged(nameof(CurrentHousehold));
                    OnPropertyChanged(nameof(CurrentHouseholdNumber));
                    OnPropertyChanged(nameof(CurrentHouseholdAddress));
                }
            }
        }

        // Thông tin người dùng hiện tại
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

        private bool _isUsingSameAddress;
        public bool IsUsingSameAddress
        {
            get { return _isUsingSameAddress; }
            set
            {
                if (_isUsingSameAddress != value)
                {
                    _isUsingSameAddress = value;
                    OnPropertyChanged(nameof(IsUsingSameAddress));
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

        // Các ICommand
        public ICommand SelectHouseHoldCommand { get; set; }
        public ICommand SeparateHouseholdsCommand { get; set; }
        public ICommand TransferHouseholdCommand { get; set; }

        public HouseHoldControlViewModel(IServiceProvider serviceProvider, ICurrentUserService currentUserService)
        {
            _serviceProvider = serviceProvider;
            _currentUserService = currentUserService;

            // Khởi tạo collection để binding
            HouseholdMembers = new ObservableCollection<HouseholdMemberDisplayInfo>();

            // Load hộ khẩu hiện tại của người dùng
            LoadCurrentHousehold();

            // Khởi tạo các command
            SelectHouseHoldCommand = new LocalRelayCommand(o => SelectHouseHold());
            SeparateHouseholdsCommand = new LocalRelayCommand(o => SeparateHouseholds(), o => CanSeparateHouseholds());
            TransferHouseholdCommand = new LocalRelayCommand(o => TransferHousehold(), o => CanTransferHousehold());
        }

        /// <summary>
        /// Lấy hộ khẩu hiện tại dựa trên currentUserId.
        /// </summary>
        private void LoadCurrentHousehold()
        {
            using (var db = new PrnContext())
            {
                // Giả sử người dùng là thành viên của hộ khẩu, ta lấy hộ khẩu có chứa thành viên có UserId bằng CurrentUser.UserId
                CurrentHousehold = db.Households
                    .Include(h => h.Address)
                    .Include(h => h.HouseholdMembers)
                    .FirstOrDefault(h => h.HouseholdMembers.Any(m => m.UserId == CurrentUser.UserId));
            }
        }

        // Các thuộc tính tính toán cho giao diện hiển thị thông tin hộ khẩu hiện tại
        public int CurrentHouseholdNumber => CurrentHousehold?.HouseholdId ?? 0;

        public string CurrentHouseholdAddress
        {
            get
            {
                if (CurrentHousehold?.Address != null)
                {
                    var addr = CurrentHousehold.Address;
                    return string.Join(", ", new[]
                    {
                        addr.Street,
                        addr.Ward,
                        addr.District,
                        addr.City,
                        addr.Country
                    }.Where(s => !string.IsNullOrWhiteSpace(s)));
                }
                return string.Empty;
            }
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
                Household selectedHousehold = selectHouseHoldWindow.SelectedHousehold;
                var selectedMembers = selectHouseHoldWindow.SelectedHouseholdMembers;
                Debug.WriteLine("Select HouseHold: " + selectedMembers);

                SelectedHousehold = selectedHousehold;

                if (selectedMembers != null)
                {
                    var dataSource = selectedMembers.Select(h =>
                    {
                        User user = GetUserById(h.UserId);
                        return new HouseholdMemberDisplayInfo
                        {
                            FullName = user.FullName,
                            IdentityCard = user.IdentityCard,
                            Relationship = h.Relationship,
                            UserId = h.UserId
                        };
                    }).ToList();
                    HouseholdMembers = new ObservableCollection<HouseholdMemberDisplayInfo>(dataSource);
                }
                else
                {
                    HouseholdMembers.Clear();
                }

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

            using (var db = new PrnContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Debug.WriteLine("SelectedHousehold: " + SelectedHousehold.HouseholdId);
                        int? newHouse = null;
                        if (IsUsingSameAddress)
                        {
                            newHouse = SelectedHousehold.HouseholdId;
                        }
                        HouseholdSeparation householdSeparation = new HouseholdSeparation
                        {
                            OriginalHouseholdId = SelectedHousehold.HouseholdId,
                            NewHouseholdId = newHouse,
                            RequestDate = DateTime.Now,
                            Status = Status.Pending.ToString(),
                            ApprovedBy = null,
                            ApprovalDate = null,
                            Comments = null
                        };

                        db.HouseholdSeparations.Add(householdSeparation);
                        db.SaveChanges();

                        foreach (var selectedMember in selectedMembers)
                        {
                            SeparationMember separationMember = new SeparationMember
                            {
                                SeparationId = householdSeparation.SeparationId,
                                UserId = selectedMember.UserId,
                                NewRelationship = "chu ho",
                                IsNewHeadOfHousehold = selectedMember.IsNewHead
                            };

                            db.SeparationMembers.Add(separationMember);
                        }
                        db.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Debug.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            MessageBox.Show("Đã gửi đơn tách hộ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            foreach (var member in HouseholdMembers)
            {
                member.IsSelected = false;
                member.IsNewHead = false;
            }
        }

        private bool CanSeparateHouseholds()
        {
            return HouseholdMembers.Any(member => member.IsSelected);
        }

        /// <summary>
        /// Xử lý chuyển hộ khẩu:
        /// 1. Tạo mới Address record với thông tin địa chỉ chuyển đến.
        /// 2. Tạo record mới trong bảng HouseholdTransfer.
        /// </summary>
        private void TransferHousehold()
        {
            using (var db = new PrnContext())
            {
                // Tạo mới Address cho địa chỉ chuyển đến
                Address newAddress = new Address
                {
                    Street = TransferStreet,
                    Ward = TransferWard,
                    District = TransferDistrict,
                    City = TransferCity,
                    State = "N/A",      // Điều chỉnh nếu có dữ liệu thực tế
                    ZipCode = "N/A",    // Điều chỉnh nếu có dữ liệu thực tế
                    Country = TransferCountry
                };
                db.Addresses.Add(newAddress);
                db.SaveChanges();

                // Tạo mới HouseholdTransfer với thông tin hộ khẩu hiện tại và địa chỉ chuyển đến
                HouseholdTransfer transfer = new HouseholdTransfer
                {
                    HouseholdId = CurrentHousehold?.HouseholdId ?? 0,
                    FromAddressId = CurrentHousehold != null ? CurrentHousehold.AddressId : 0,
                    ToAddressId = newAddress.AddressId,
                    RequestDate = DateOnly.FromDateTime(DateTime.Now),
                    Status = "Pending",
                    ApprovedBy = null,
                    Comments = null
                };

                db.HouseholdTransfers.Add(transfer);
                db.SaveChanges();
            }

            MessageBox.Show("Đơn chuyển hộ khẩu đã được gửi.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool CanTransferHousehold()
        {
            // Kiểm tra các trường địa chỉ chuyển đến phải được điền ít nhất Số nhà, thành phố và quốc gia
            return !string.IsNullOrWhiteSpace(TransferStreet) &&
                   !string.IsNullOrWhiteSpace(TransferCity) &&
                   !string.IsNullOrWhiteSpace(TransferCountry);
        }
    }
}
