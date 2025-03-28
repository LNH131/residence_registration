﻿// Project.DAO/UserDAO.cs
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;
namespace Resident.DAO
{
    public class UserDAO
    {
        private readonly PrnContext _context;

        public UserDAO(PrnContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateUser(string email, string password, string selectedRole)
        {
            Debug.WriteLine("selectedRole: " + selectedRole);
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
        public static List<User> GetAllUsers()
        {
            PrnContext prnContext = new PrnContext();
            return prnContext.Users.ToList();
        }
    }
}