using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Models;

namespace Project.DAO
{
    
    public class HouseHoldDAO
    {
        private readonly PrnContext _context;

        public HouseHoldDAO(PrnContext context)
        {
            _context = context;
        }

    }
}
