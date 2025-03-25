using CommunityToolkit.Mvvm.ComponentModel;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class CitizenPoliceChatSelectionViewModel : ObservableObject
    {
        private readonly PrnContext _context;
        private readonly ICurrentUserService _currentUserService;

        // Danh sách công an có thể chat
        private ObservableCollection<User> _availablePolice;
        public ObservableCollection<User> AvailablePolice
        {
            get => _availablePolice;
            set
            {
                _availablePolice = value;
                OnPropertyChanged();
            }
        }

        private User _selectedPolice;
        public User SelectedPolice
        {
            get => _selectedPolice;
            set
            {
                _selectedPolice = value;
                OnPropertyChanged();
            }
        }

        // Command để mở chat và đóng (cancel) cửa sổ
        public ICommand StartChatCommand { get; }
        public ICommand CancelCommand { get; }

        // Được gán từ code-behind để đóng cửa sổ (nếu cần)
        public Action CloseAction { get; set; }

        public CitizenPoliceChatSelectionViewModel(ICurrentUserService currentUserService)
        {
            _context = new PrnContext();
            _currentUserService = currentUserService;

            LoadPoliceInArea();

            StartChatCommand = new LocalRelayCommand(
                _ => OpenChatWindow(),
                _ => SelectedPolice != null
            );

            CancelCommand = new LocalRelayCommand(
                _ => CloseAction?.Invoke()
            );
        }

        /// <summary>
        /// Nạp danh sách công an trong cùng khu vực với Citizen.
        /// </summary>
        private void LoadPoliceInArea()
        {
            // Lấy AreaId từ CurrentUser (Citizen).
            int? areaId = _currentUserService.CurrentUser?.AreaId;
            if (areaId == null)
            {
                // Nếu user không có area, thì không thể tải danh sách police
                AvailablePolice = new ObservableCollection<User>();
                return;
            }

            // Truy vấn danh sách Police trong area này
            var policeInArea = _context.Users
                                       .Where(u => u.Role == "Police" && u.AreaId == areaId.Value)
                                       .ToList();

            AvailablePolice = new ObservableCollection<User>(policeInArea);
        }

        /// <summary>
        /// Mở cửa sổ chat với công an được chọn.
        /// </summary>
        private void OpenChatWindow()
        {
            if (SelectedPolice == null)
            {
                // Phòng trường hợp CanExecute không kịp cập nhật
                return;
            }

            var chatViewModel = new CitizenChatViewModel(_currentUserService, SelectedPolice.UserId);
            var chatWindow = new Resident.View.CitizenChatWindow(chatViewModel);
            chatWindow.Show();
        }
    }
}
