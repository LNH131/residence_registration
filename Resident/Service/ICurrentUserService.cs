using Resident.Models;

namespace Resident.Service
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
