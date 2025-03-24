using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class CitizenPoliceChatSelectionViewModel : BaseViewModel
    {
        private readonly PrnContext _context = new PrnContext();
        private readonly ICurrentUserService _currentUserService;

        // List of police officers available in the citizen's area.
        public ObservableCollection<User> AvailablePolice { get; set; }

        private User _selectedPolice;
        public User SelectedPolice
        {
            get => _selectedPolice;
            set { _selectedPolice = value; OnPropertyChanged(nameof(SelectedPolice)); }
        }

        public ICommand OpenChatCommand { get; }
        public ICommand CancelCommand { get; }

        public CitizenPoliceChatSelectionViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            LoadAvailablePolice();
            OpenChatCommand = new LocalRelayCommand(o => OpenChat(), o => SelectedPolice != null);
        }

        private void LoadAvailablePolice()
        {
            // Get the area ID from the current citizen.
            int? areaId = _currentUserService.CurrentUser.AreaId;
            if (areaId == null)
            {
                MessageBox.Show("Không xác định được khu vực của bạn.");
                AvailablePolice = new ObservableCollection<User>();
                return;
            }

            // Option 1: Query Users directly.
            // var policeList = _context.Users.Where(u => u.Role == "Police" && u.AreaId == areaId).ToList();

            // Option 2: Use the Area model.
            var areaRecord = _context.Areas
                .Include(a => a.Users)
                .FirstOrDefault(a => a.AreaId == areaId.Value);
            if (areaRecord == null)
            {
                MessageBox.Show("Không tìm thấy khu vực của bạn.");
                AvailablePolice = new ObservableCollection<User>();
                return;
            }

            var policeList = areaRecord.Users.Where(u => u.Role == "Police").ToList();
            AvailablePolice = new ObservableCollection<User>(policeList);
        }

        private void OpenChat()
        {
            // Create the citizen chat view model using the selected police's UserId.
            var chatViewModel = new CitizenChatViewModel(_currentUserService, SelectedPolice.UserId);
            var chatWindow = new Resident.View.CitizenChatWindow(chatViewModel);
            chatWindow.Show();
        }
    }
}
