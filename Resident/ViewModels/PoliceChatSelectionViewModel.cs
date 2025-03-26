using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class PoliceChatSelectionViewModel : BaseViewModel
    {
        private readonly PrnContext _context;
        private readonly ICurrentUserService _currentUserService;

        private List<User> _allCitizens;
        private List<User> _allAreaLeaders;

        public ObservableCollection<User> Citizens { get; set; }
        public ObservableCollection<User> AreaLeaders { get; set; }

        private User _selectedCitizen;
        public User SelectedCitizen
        {
            get => _selectedCitizen;
            set
            {
                _selectedCitizen = value;
                OnPropertyChanged(nameof(SelectedCitizen));
                (ChatWithCitizenCommand as LocalRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private User _selectedAreaLeader;
        public User SelectedAreaLeader
        {
            get => _selectedAreaLeader;
            set
            {
                _selectedAreaLeader = value;
                OnPropertyChanged(nameof(SelectedAreaLeader));
                (ChatWithAreaLeaderCommand as LocalRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand ChatWithCitizenCommand { get; }
        public ICommand ChatWithAreaLeaderCommand { get; }
        public ICommand CancelCommand { get; }

        // Optional: an Action to close the window (set from code-behind)
        public System.Action CloseAction { get; set; }

        public PoliceChatSelectionViewModel(ICurrentUserService currentUserService, PrnContext context)
        {
            _currentUserService = currentUserService;
            _context = context;

            LoadCitizens();
            LoadAreaLeaders();

            Citizens = new ObservableCollection<User>(_allCitizens);
            AreaLeaders = new ObservableCollection<User>(_allAreaLeaders);

            ChatWithCitizenCommand = new LocalRelayCommand(_ => ChatWithCitizen(), _ => SelectedCitizen != null);
            ChatWithAreaLeaderCommand = new LocalRelayCommand(_ => ChatWithAreaLeader(), _ => SelectedAreaLeader != null);
            CancelCommand = new LocalRelayCommand(_ => CloseAction?.Invoke());
        }

        private void LoadCitizens()
        {
            // Load all citizens from the database.
            _allCitizens = _context.Users
                                   .Where(u => u.Role == "Citizen")
                                   .ToList();
        }

        private void LoadAreaLeaders()
        {
            // Load all area leaders from the database.
            _allAreaLeaders = _context.Users
                                      .Where(u => u.Role == "AreaLeader")
                                      .ToList();
        }

        private void ChatWithCitizen()
        {
            // Create and open the PoliceChatWindow for the selected citizen.
            var chatVM = new PoliceChatViewModel(_currentUserService, SelectedCitizen.UserId);
            var chatWindow = new Resident.View.PoliceChatWindow(chatVM);
            chatWindow.Show();
        }

        private void ChatWithAreaLeader()
        {
            // Create and open the PoliceChatWindow for the selected area leader.
            var chatVM = new PoliceChatViewModel(_currentUserService, SelectedAreaLeader.UserId);
            var chatWindow = new Resident.View.PoliceChatWindow(chatVM);
            chatWindow.Show();
        }
    }
}
