using Resident.DAO;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Resident.ViewModels
{
    public class StatusOverviewViewModel : INotifyPropertyChanged
    {
        private readonly ICurrentUserService _currentUserService;

        public User CurrentUser => _currentUserService.CurrentUser;

        private ObservableCollection<Registration> _registrations;
        public ObservableCollection<Registration> Registrations
        {
            get { return _registrations; }
            set { _registrations = value; OnPropertyChanged(nameof(Registrations)); }
        }

        private ObservableCollection<HouseholdSeparation> _householdSeparations;
        public ObservableCollection<HouseholdSeparation> HouseholdSeparations
        {
            get { return _householdSeparations; }
            set { _householdSeparations = value; OnPropertyChanged(nameof(HouseholdSeparations)); }
        }

        // Constructor nhận userId của người dùng hiện tại
        public StatusOverviewViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            using (var context = new PrnContext())
            {
                // Lấy danh sách hồ sơ đăng ký hộ khẩu theo userId
                Registrations = new ObservableCollection<Registration>(
                    context.Registrations.Where(r => r.UserId == CurrentUser.UserId).ToList());

                // Lấy danh sách đơn tách hộ có liên quan đến userId.
                // Ví dụ: nếu hộ ban đầu có HeadId trùng với userId, hoặc nếu hộ mới được tạo và có HeadId trùng.
                HouseholdSeparations = new ObservableCollection<HouseholdSeparation>(
                    context.HouseholdSeparations
                           .Where(s => s.OriginalHousehold.HeadId == CurrentUser.UserId ||
                                       (s.NewHousehold != null && s.NewHousehold.HeadId == CurrentUser.UserId))
                           .ToList());
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
