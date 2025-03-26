using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class AreaLeaderChatSelectionViewModel : BaseViewModel
    {
        private readonly PrnContext _context;
        private readonly ICurrentUserService _currentUserService;

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
            set
            {
                _availablePolice = value;
                OnPropertyChanged(nameof(AvailablePolice));
            }
        }

        private User _selectedPolice;
        public User SelectedPolice
        {
            get => _selectedPolice;
            set
            {
                _selectedPolice = value;
                OnPropertyChanged(nameof(SelectedPolice));
                (OpenChatCommand as LocalRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public ICommand OpenChatCommand { get; }

        // Constructor receives ICurrentUserService and PrnContext via DI.
        public AreaLeaderChatSelectionViewModel(ICurrentUserService currentUserService, PrnContext context)
        {
            _currentUserService = currentUserService;
            _context = context;

            LoadAreas();
            LoadAllPolice();

            // Display all police initially.
            AvailablePolice = new ObservableCollection<User>(_allPolice);

            // Initialize command to open chat if a police is selected.
            OpenChatCommand = new LocalRelayCommand(
                _ => OpenChat(),
                _ => SelectedPolice != null
            );
        }

        private void LoadAreas()
        {
            var areas = _context.Areas.ToList();
            AvailableAreas = new ObservableCollection<Area>(areas);
        }

        private void LoadAllPolice()
        {
            // Load all police from the database.
            _allPolice = _context.Users
                                 .Where(u => u.Role == "Police")
                                 .Include(u => u.Area)
                                 .ToList();
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
            // Create the chat ViewModel using the logged-in user's ID and the selected police's UserId.
            var chatVM = new AreaLeaderChatViewModel(_currentUserService, SelectedPolice.UserId);
            var chatWindow = new Resident.View.AreaLeaderChatWindow(chatVM);
            chatWindow.Show();
        }
    }
}
