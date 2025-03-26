using CommunityToolkit.Mvvm.ComponentModel;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class CitizenPoliceChatSelectionViewModel : ObservableObject
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly PrnContext _context;

        private ObservableCollection<User> _availablePolice;
        public ObservableCollection<User> AvailablePolice
        {
            get => _availablePolice;
            set => SetProperty(ref _availablePolice, value);
        }

        private User _selectedPolice;
        public User SelectedPolice
        {
            get => _selectedPolice;
            set
            {
                SetProperty(ref _selectedPolice, value);
                (StartChatCommand as LocalRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand StartChatCommand { get; }
        public ICommand CancelCommand { get; }

        public System.Action CloseAction { get; set; }

        public CitizenPoliceChatSelectionViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _context = new PrnContext(); // Consider injecting this via DI for better testability.

            LoadPoliceInArea();

            StartChatCommand = new LocalRelayCommand(
                _ => OpenChatWindow(),
                _ => SelectedPolice != null
            );

            CancelCommand = new LocalRelayCommand(
                _ => CloseAction?.Invoke()
            );
        }

        private void LoadPoliceInArea()
        {
            // Assuming the citizen's AreaId is in CurrentUser
            int? areaId = _currentUserService.CurrentUser?.AreaId;
            if (areaId == null)
            {
                AvailablePolice = new ObservableCollection<User>();
                return;
            }
            var policeInArea = _context.Users
                                       .Where(u => u.Role == "Police" && u.AreaId == areaId.Value)
                                       .ToList();
            AvailablePolice = new ObservableCollection<User>(policeInArea);
        }

        private void OpenChatWindow()
        {
            if (SelectedPolice == null)
                return;

            var chatViewModel = new CitizenChatViewModel(_currentUserService, SelectedPolice.UserId);
            var chatWindow = new Resident.View.CitizenChatWindow(chatViewModel);
            chatWindow.Show();
        }
    }
}
