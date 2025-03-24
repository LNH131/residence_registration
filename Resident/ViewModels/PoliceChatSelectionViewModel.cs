using CommunityToolkit.Mvvm.ComponentModel;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class PoliceChatSelectionViewModel : ObservableObject
    {
        private readonly PrnContext _context = new PrnContext();
        private readonly ICurrentUserService _currentUserService;

        public ObservableCollection<User> Citizens { get; set; }
        public ObservableCollection<User> AreaLeaders { get; set; }

        private User _selectedCitizen;
        public User SelectedCitizen
        {
            get => _selectedCitizen;
            set { _selectedCitizen = value; OnPropertyChanged(nameof(SelectedCitizen)); }
        }

        private User _selectedAreaLeader;
        public User SelectedAreaLeader
        {
            get => _selectedAreaLeader;
            set { _selectedAreaLeader = value; OnPropertyChanged(nameof(SelectedAreaLeader)); }
        }

        public ICommand ChatWithCitizenCommand { get; }
        public ICommand ChatWithAreaLeaderCommand { get; }
        public ICommand CancelCommand { get; }

        // Action để đóng cửa sổ, sẽ được gán từ code-behind.
        public System.Action? CloseAction { get; set; }

        public PoliceChatSelectionViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            LoadCitizens();
            LoadAreaLeaders();

            ChatWithCitizenCommand = new LocalRelayCommand(o => ChatWithCitizen(), o => SelectedCitizen != null);
            ChatWithAreaLeaderCommand = new LocalRelayCommand(o => ChatWithAreaLeader(), o => SelectedAreaLeader != null);
            CancelCommand = new LocalRelayCommand(o => CloseAction?.Invoke());
        }

        private void LoadCitizens()
        {
            var citizens = _context.Users.Where(u => u.Role == "Citizen").ToList();
            Citizens = new ObservableCollection<User>(citizens);
        }

        private void LoadAreaLeaders()
        {
            var leaders = _context.Users.Where(u => u.Role == "AreaLeader").ToList();
            AreaLeaders = new ObservableCollection<User>(leaders);
        }

        private void ChatWithCitizen()
        {
            // Tạo PoliceChatViewModel để chat với người dân được chọn.
            var chatVM = new PoliceChatViewModel(_currentUserService, SelectedCitizen.UserId);
            var chatWindow = new Resident.View.PoliceChatWindow(chatVM);
            chatWindow.Show();
        }

        private void ChatWithAreaLeader()
        {
            // Tạo PoliceChatViewModel để chat với Tổ trưởng được chọn.
            var chatVM = new PoliceChatViewModel(_currentUserService, SelectedAreaLeader.UserId);
            var chatWindow = new Resident.View.PoliceChatWindow(chatVM);
            chatWindow.Show();
        }
    }
}
