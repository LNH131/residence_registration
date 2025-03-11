// Project.DAO/UserDAO.cs
using Microsoft.EntityFrameworkCore;
using Project.Enums;
using Project.Models;
namespace Project.DAO
{
    public class UserDAO
    {
        private readonly PrnContext _context;

        public UserDAO(PrnContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateUser(string email, string password, Role selectedRole)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }
            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                if (user.Role == selectedRole)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public async Task AddUser(User newUser, string password)
        {
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(password);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }


    }
}