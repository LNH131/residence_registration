using Microsoft.EntityFrameworkCore;
using Resident.Models;

namespace Resident.Service
{
    public class ChatMessageService
    {
        private readonly PrnContext _context;
        public ChatMessageService(PrnContext context)
        {
            _context = context;
        }

        public async Task<List<ChatMessage>> GetConversationAsync(int userA, int userB)
        {
            return await _context.ChatMessages
                .Where(m => (m.FromUserId == userA && m.ToUserId == userB) ||
                            (m.FromUserId == userB && m.ToUserId == userA))
                .OrderBy(m => m.SentDate)
                .ToListAsync();
        }

        public async Task InsertMessageAsync(ChatMessage message)
        {
            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMessageAsync(ChatMessage message)
        {
            _context.ChatMessages.Update(message);
            await _context.SaveChangesAsync();
        }
    }
}
