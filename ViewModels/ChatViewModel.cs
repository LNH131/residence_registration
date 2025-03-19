using Project.Models;
using Project.Service;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Project.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        // ID của người gửi (current user) và đối tác chat (chat partner)
        public int CurrentUserId { get; set; }
        public int ChatPartnerId { get; set; }

        private string _newMessage;
        private readonly ChatMessageService _chatService;

        public ChatViewModel(int currentUserId, int chatPartnerId)
        {
            CurrentUserId = currentUserId;
            ChatPartnerId = chatPartnerId;
            _chatService = new ChatMessageService(new PrnContext()); // Tốt nhất là inject qua DI
            ChatMessages = new ObservableCollection<ChatMessage>();
            // Tải các tin nhắn từ DB
            LoadConversation();
            SendMessageCommand = new RelayCommand(async o => await SendMessageAsync(), o => !string.IsNullOrWhiteSpace(NewMessage));
        }

        private async void LoadConversation()
        {
            var messages = await _chatService.GetConversationAsync(CurrentUserId, ChatPartnerId);
            foreach (var msg in messages)
            {
                ChatMessages.Add(msg);
            }
        }

        private async Task SendMessageAsync()
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
        }
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

        private void SendMessage()
        {
            var newMsg = new ChatMessage
            {
                MessageId = ChatMessages.Count + 1,
                FromUserId = CurrentUserId,
                ToUserId = ChatPartnerId,
                Content = NewMessage,
                SentDate = DateTime.Now,
                IsRead = false
            };

            ChatMessages.Add(newMsg);
            NewMessage = string.Empty;
        }
    }
}
