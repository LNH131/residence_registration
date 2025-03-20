using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resident.Models;

namespace Resident.DAO
{
    public class AdminDAO
    {
        private readonly PrnContext _context;

        public AdminDAO()
        {

        }
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

    }
}
