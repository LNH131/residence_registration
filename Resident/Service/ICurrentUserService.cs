using Microsoft.EntityFrameworkCore;
using Resident.Models;

namespace Resident.Service
{
    public interface ICurrentUserService
    {
        User CurrentUser { get; set; }
        Task<User> GetUserByIdAsync(int userId);
    }

    public class CurrentUserService : ICurrentUserService
    {
        public User CurrentUser { get; set; }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            using (var context = new PrnContext())
            {
                // Returns null if no user is found.
                return await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            }
        }
    }
}
