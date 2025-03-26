using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class CitizenChatViewModel : BaseViewModel
    {
        public int CurrentUserId { get; set; }
        public int ChatPartnerId { get; set; }

        private string _chatPartnerFullName;
        public string ChatPartnerFullName
        {
            get => _chatPartnerFullName;
            set { _chatPartnerFullName = value; OnPropertyChanged(); }
        }

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

        private ObservableCollection<ChatMessageDisplaycitizen> _chatMessages;
        public ObservableCollection<ChatMessageDisplaycitizen> ChatMessages
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

            if (_currentUserService.CurrentUser == null)
            {
                throw new InvalidOperationException("No logged-in user found.");
            }

            CurrentUserId = _currentUserService.CurrentUser.UserId;
            ChatPartnerId = chatPartnerId;

            _chatService = new ChatMessageService(new PrnContext());
            ChatMessages = new ObservableCollection<ChatMessageDisplaycitizen>();

            LoadConversationAsync();

            SendMessageCommand = new LocalRelayCommand(
                async _ => await SendMessageAsync(),
                _ => !string.IsNullOrWhiteSpace(NewMessage)
            );

            LoadChatPartnerName();
        }

        private async void LoadChatPartnerName()
        {
            try
            {
                var chatPartner = await _currentUserService.GetUserByIdAsync(ChatPartnerId);
                ChatPartnerFullName = chatPartner?.FullName ?? "Unknown User";
            }
            catch (Exception ex)
            {
                ChatPartnerFullName = "Unknown User";
                Debug.WriteLine("Error loading chat partner name: " + ex);
            }
        }

        private async void LoadConversationAsync()
        {
            try
            {
                var messages = await _chatService.GetConversationAsync(CurrentUserId, ChatPartnerId);
                ChatMessages.Clear();

                foreach (var msg in messages)
                {
                    User sender = null;
                    if (msg.FromUserId.HasValue)
                        sender = await _currentUserService.GetUserByIdAsync(msg.FromUserId.Value);

                    ChatMessages.Add(new ChatMessageDisplaycitizen
                    {
                        ChatMessageId = msg.MessageId,
                        FromUserId = msg.FromUserId,
                        ToUserId = msg.ToUserId,
                        Content = msg.Content,
                        SentDate = msg.SentDate,
                        IsRead = msg.IsRead,
                        FromUserFullName = sender?.FullName ?? (msg.FromUserId.HasValue ? msg.FromUserId.Value.ToString() : "Unknown")
                    });
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

                User sender = null;
                if (newMsg.FromUserId.HasValue)
                    sender = await _currentUserService.GetUserByIdAsync(newMsg.FromUserId.Value);

                ChatMessages.Add(new ChatMessageDisplaycitizen
                {
                    ChatMessageId = newMsg.MessageId,
                    FromUserId = newMsg.FromUserId,
                    ToUserId = newMsg.ToUserId,
                    Content = newMsg.Content,
                    SentDate = newMsg.SentDate,
                    IsRead = newMsg.IsRead,
                    FromUserFullName = sender?.FullName ?? (newMsg.FromUserId.HasValue ? newMsg.FromUserId.Value.ToString() : "Unknown")
                });

                NewMessage = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending message: " + ex.Message);
            }
        }
    }

    // Define ChatMessageDisplaycitizen only once.
    public class ChatMessageDisplaycitizen
    {
        public int ChatMessageId { get; set; }
        public int? FromUserId { get; set; }
        public int? ToUserId { get; set; }
        public string Content { get; set; }
        public DateTime? SentDate { get; set; }
        public bool? IsRead { get; set; }
        public string FromUserFullName { get; set; }
    }
}
