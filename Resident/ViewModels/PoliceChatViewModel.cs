using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class PoliceChatViewModel : BaseViewModel
    {
        public int CurrentUserId { get; set; }
        public int ChatPartnerId { get; set; }

        private string _newMessage;
        public string NewMessage
        {
            get => _newMessage;
            set
            {
                _newMessage = value;
                OnPropertyChanged();
                (SendMessageCommand as LocalRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<ChatMessage> _chatMessages;
        public ObservableCollection<ChatMessage> ChatMessages
        {
            get => _chatMessages;
            set { _chatMessages = value; OnPropertyChanged(); }
        }

        public ICommand SendMessageCommand { get; }

        private readonly ChatMessageService _chatService;
        private readonly ICurrentUserService _currentUserService;

        public PoliceChatViewModel(ICurrentUserService currentUserService, int chatPartnerId)
        {
            _currentUserService = currentUserService;

            // Ensure that CurrentUser is already set. If not, it will throw an exception.
            if (_currentUserService.CurrentUser == null)
            {
                throw new InvalidOperationException("Current user is not set. Ensure the user is properly authenticated and that ICurrentUserService is registered as a singleton.");
            }

            CurrentUserId = _currentUserService.CurrentUser.UserId;
            ChatPartnerId = chatPartnerId;

            Debug.WriteLine($"PoliceChatViewModel: CurrentUserId = {CurrentUserId}, ChatPartnerId = {ChatPartnerId}");

            // Create a ChatMessageService instance (ideally via DI)
            _chatService = new ChatMessageService(new PrnContext());
            ChatMessages = new ObservableCollection<ChatMessage>();

            // Load the conversation asynchronously.
            LoadConversationAsync();

            // Initialize SendMessageCommand with a command that re-checks its CanExecute state whenever NewMessage changes.
            SendMessageCommand = new LocalRelayCommand(
                async o => await SendMessageAsync(),
                o => !string.IsNullOrWhiteSpace(NewMessage));
        }

        private async void LoadConversationAsync()
        {
            try
            {
                var messages = await _chatService.GetConversationAsync(CurrentUserId, ChatPartnerId);
                Debug.WriteLine($"Loaded {messages.Count} messages.");
                foreach (var msg in messages)
                {
                    ChatMessages.Add(msg);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error loading chat messages: " + ex.Message);
                Debug.WriteLine("Error loading chat messages: " + ex);
            }
        }

        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(NewMessage))
                return;

            try
            {
                var newMsg = new ChatMessage
                {
                    FromUserId = CurrentUserId,
                    ToUserId = ChatPartnerId,
                    Content = NewMessage,
                    SentDate = System.DateTime.Now,
                    IsRead = false
                };

                await _chatService.InsertMessageAsync(newMsg);
                ChatMessages.Add(newMsg);
                NewMessage = string.Empty;
                Debug.WriteLine("Message sent successfully.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error sending message: " + ex.Message);
                Debug.WriteLine("Error sending message: " + ex);
            }
        }
    }
}
