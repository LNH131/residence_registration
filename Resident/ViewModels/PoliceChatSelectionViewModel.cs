using CommunityToolkit.Mvvm.ComponentModel;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class PoliceChatSelectionViewModel : ObservableObject
    {
        private readonly PrnContext _context;
        private readonly ICurrentUserService _currentUserService;

        public ObservableCollection<User> Citizens { get; set; }
        public ObservableCollection<User> AreaLeaders { get; set; }

        private User _selectedCitizen;
        public User SelectedCitizen
        {
            get => _selectedCitizen;
            set
            {
                SetProperty(ref _selectedCitizen, value);
                // Force requery of the command
                (ChatWithCitizenCommand as LocalRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private User _selectedAreaLeader;
        public User SelectedAreaLeader
        {
            get => _selectedAreaLeader;
            set
            {
                SetProperty(ref _selectedAreaLeader, value);
                (ChatWithAreaLeaderCommand as LocalRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        // Commands for opening chat and canceling.
        public ICommand ChatWithCitizenCommand { get; }
        public ICommand ChatWithAreaLeaderCommand { get; }
        public ICommand CancelCommand { get; }

        // Action to close the window (set from code-behind).
        public System.Action CloseAction { get; set; }

        public PoliceChatSelectionViewModel(ICurrentUserService currentUserService)
        {
            _context = new PrnContext();
            _currentUserService = currentUserService;

            LoadCitizens();
            LoadAreaLeaders();

            // Use a RelayCommand implementation that supports RaiseCanExecuteChanged.
            ChatWithCitizenCommand = new LocalRelayCommand(
                _ => ChatWithCitizen(),
                _ => SelectedCitizen != null);
            ChatWithAreaLeaderCommand = new LocalRelayCommand(
                _ => ChatWithAreaLeader(),
                _ => SelectedAreaLeader != null);
            CancelCommand = new LocalRelayCommand(_ => CloseAction?.Invoke());
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
            // Open the PoliceChatWindow with the selected citizen.
            var chatVM = new PoliceChatViewModel(_currentUserService, SelectedCitizen.UserId);
            var chatWindow = new Resident.View.PoliceChatWindow(chatVM);
            chatWindow.Show();
        }

        private void ChatWithAreaLeader()
        {
            // Open the PoliceChatWindow with the selected area leader.
            var chatVM = new PoliceChatViewModel(_currentUserService, SelectedAreaLeader.UserId);
            var chatWindow = new Resident.View.PoliceChatWindow(chatVM);
            chatWindow.Show();
        }
    }
}
