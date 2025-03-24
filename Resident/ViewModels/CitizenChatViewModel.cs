using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class CitizenChatViewModel : BaseViewModel
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

        public CitizenChatViewModel(ICurrentUserService currentUserService, int chatPartnerId)
        {
            _currentUserService = currentUserService;
            CurrentUserId = _currentUserService.CurrentUser.UserId;
            ChatPartnerId = chatPartnerId;

            _chatService = new ChatMessageService(new PrnContext());
            ChatMessages = new ObservableCollection<ChatMessage>();

            LoadConversation();
            SendMessageCommand = new LocalRelayCommand(async o => await SendMessageAsync(), o => !string.IsNullOrWhiteSpace(NewMessage));
        }

        private async void LoadConversation()
        {
            try
            {
                var messages = await _chatService.GetConversationAsync(CurrentUserId, ChatPartnerId);
                foreach (var msg in messages)
                {
                    ChatMessages.Add(msg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading chat messages: " + ex.Message);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending message: " + ex.Message);
            }
        }
    }
}
