using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Service
{
    public interface ICurrentUserService
    {
        User CurrentUser { get; set; }
    }

    public class CurrentUserService : ICurrentUserService
    {
        public User CurrentUser { get; set; }
    }
}
