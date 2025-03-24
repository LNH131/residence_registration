using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class AreaLeaderChatSelectionViewModel : BaseViewModel
    {
        private readonly PrnContext _context = new PrnContext();
        private readonly ICurrentUserService _currentUserService;

        // Store all police from the database.
        private List<User> _allPolice;

        public ObservableCollection<Area> AvailableAreas { get; set; }

        private Area _selectedArea;
        public Area SelectedArea
        {
            get => _selectedArea;
            set
            {
                _selectedArea = value;
                OnPropertyChanged(nameof(SelectedArea));
                FilterPoliceByArea();
            }
        }

        private ObservableCollection<User> _availablePolice;
        public ObservableCollection<User> AvailablePolice
        {
            get => _availablePolice;
            set { _availablePolice = value; OnPropertyChanged(nameof(AvailablePolice)); }
        }

        private User _selectedPolice;
        public User SelectedPolice
        {
            get => _selectedPolice;
            set { _selectedPolice = value; OnPropertyChanged(nameof(SelectedPolice)); }
        }

        public ICommand OpenChatCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor: inject ICurrentUserService so the leader's info is available.
        public AreaLeaderChatSelectionViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            LoadAreas();
            LoadAllPolice(); // Load all police first.
            // Initially display all police.
            AvailablePolice = new ObservableCollection<User>(_allPolice);

            OpenChatCommand = new LocalRelayCommand(o => OpenChat(), o => SelectedPolice != null);
        }

        private void LoadAreas()
        {
            var areas = _context.Areas.ToList();
            AvailableAreas = new ObservableCollection<Area>(areas);
        }

        private void LoadAllPolice()
        {
            // Load all police from the DB.
            _allPolice = _context.Users.Where(u => u.Role == "Police").Include(u => u.Area).ToList();
        }

        // Filter the police list based on the selected area.
        private void FilterPoliceByArea()
        {
            if (SelectedArea != null)
            {
                var filtered = _allPolice.Where(p => p.AreaId == SelectedArea.AreaId).ToList();
                AvailablePolice = new ObservableCollection<User>(filtered);
            }
            else
            {
                AvailablePolice = new ObservableCollection<User>(_allPolice);
            }
        }

        private void OpenChat()
        {
            // Create the AreaLeaderChatViewModel with the current leader and the selected police.
            var chatVM = new AreaLeaderChatViewModel(_currentUserService, SelectedPolice.UserId);
            var chatWindow = new Resident.View.AreaLeaderChatWindow(chatVM);
            chatWindow.Show();
        }
    }
}
