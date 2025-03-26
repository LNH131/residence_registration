using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class AreaLeaderChatViewModel : BaseViewModel
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
                (SendMessageCommand as LocalRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        // Observable collection of ChatMessageDisplay for binding.
        private ObservableCollection<ChatMessageDisplay> _chatMessages;
        public ObservableCollection<ChatMessageDisplay> ChatMessages
        {
            get => _chatMessages;
            set { _chatMessages = value; OnPropertyChanged(); }
        }

        public ICommand SendMessageCommand { get; }

        private readonly ChatMessageService _chatService;
        private readonly ICurrentUserService _currentUserService;

        public AreaLeaderChatViewModel(ICurrentUserService currentUserService, int chatPartnerId)
        {
            _currentUserService = currentUserService;
            if (_currentUserService.CurrentUser == null)
            {
                throw new Exception("No logged-in user found. Please login first.");
            }
            CurrentUserId = _currentUserService.CurrentUser.UserId;
            ChatPartnerId = chatPartnerId;

            Debug.WriteLine($"AreaLeaderChatViewModel: CurrentUserId = {CurrentUserId}, ChatPartnerId = {ChatPartnerId}");

            // Ideally, inject ChatMessageService via DI; for now we instantiate it with a new PrnContext.
            _chatService = new ChatMessageService(new PrnContext());
            ChatMessages = new ObservableCollection<ChatMessageDisplay>();

            // Load conversation and chat partner's name.
            LoadConversationAsync();
            LoadChatPartnerName();

            SendMessageCommand = new LocalRelayCommand(async _ => await SendMessageAsync(),
                _ => !string.IsNullOrWhiteSpace(NewMessage));
        }

        private async void LoadChatPartnerName()
        {
            try
            {
                // Using the merged ICurrentUserService to retrieve the chat partner's full name.
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
                Debug.WriteLine($"Loaded {messages.Count} messages.");
                ChatMessages.Clear();
                foreach (var msg in messages)
                {
                    // Here we check if FromUser is already loaded via Include; if not, we perform a lookup.
                    string senderName = msg.FromUser?.FullName;
                    if (string.IsNullOrWhiteSpace(senderName))
                    {
                        var sender = await _currentUserService.GetUserByIdAsync(msg.FromUserId ?? 0);
                        senderName = sender?.FullName ?? msg.FromUserId?.ToString() ?? "Unknown";
                    }

                    var displayMsg = new ChatMessageDisplay
                    {
                        MessageId = msg.MessageId,
                        FromUserId = msg.FromUserId,
                        ToUserId = msg.ToUserId,
                        Content = msg.Content,
                        SentDate = msg.SentDate,
                        IsRead = msg.IsRead,
                        FromUserFullName = senderName
                    };
                    ChatMessages.Add(displayMsg);
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

                // Lookup the sender's full name.
                string senderName;
                if (newMsg.FromUserId != null)
                {
                    var sender = await _currentUserService.GetUserByIdAsync(newMsg.FromUserId.Value);
                    senderName = sender?.FullName ?? newMsg.FromUserId.ToString();
                }
                else
                {
                    senderName = "Unknown";
                }

                var displayMsg = new ChatMessageDisplay
                {
                    MessageId = newMsg.MessageId,
                    FromUserId = newMsg.FromUserId,
                    ToUserId = newMsg.ToUserId,
                    Content = newMsg.Content,
                    SentDate = newMsg.SentDate,
                    IsRead = newMsg.IsRead,
                    FromUserFullName = senderName
                };

                ChatMessages.Add(displayMsg);
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
