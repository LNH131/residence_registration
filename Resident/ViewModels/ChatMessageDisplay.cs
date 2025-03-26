using Resident.Models;

namespace Resident.ViewModels
{
    // This helper class wraps a ChatMessage and adds a property for the sender's full name.
    public class ChatMessageDisplay : ChatMessage
    {
        public string FromUserFullName { get; set; }
    }
}
