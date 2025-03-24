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
            set { _newMessage = value; OnPropertyChanged(); }
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

            // Fallback for testing if CurrentUser is not set.
            if (_currentUserService.CurrentUser == null)
            {
                Debug.WriteLine("Warning: Current user is not set. Using fallback test user.");
                _currentUserService.CurrentUser = new User
                {
                    UserId = 1016,
                    FullName = "Test Area Leader",
                    Role = "AreaLeader",
                    AreaId = 1
                };
            }

            CurrentUserId = _currentUserService.CurrentUser.UserId;
            ChatPartnerId = chatPartnerId;

            Debug.WriteLine($"PoliceChatViewModel: CurrentUserId = {CurrentUserId}, ChatPartnerId = {ChatPartnerId}");

            // Ideally inject ChatMessageService via DI.
            _chatService = new ChatMessageService(new PrnContext());
            ChatMessages = new ObservableCollection<ChatMessage>();

            LoadConversationAsync();

            SendMessageCommand = new RelayCommand(async o => await SendMessageAsync(), o => !string.IsNullOrWhiteSpace(NewMessage));
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
            catch (Exception ex)
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
                    SentDate = DateTime.Now,
                    IsRead = false
                };

                await _chatService.InsertMessageAsync(newMsg);
                ChatMessages.Add(newMsg);
                NewMessage = string.Empty;
                Debug.WriteLine("Message sent successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending message: " + ex.Message);
                Debug.WriteLine("Error sending message: " + ex);
            }
        }
    }
}
