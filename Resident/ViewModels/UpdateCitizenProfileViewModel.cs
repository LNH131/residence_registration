using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resident.Service;
using Resident.Models;

namespace Resident.ViewModels
{
    public partial class UpdateCitizenProfileViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrentUserService _currentUserService;
        private readonly PrnContext _context;

        public User CurrentUser => _currentUserService.CurrentUser;
        public Address CurrentAddress => _context.Addresses.FirstOrDefault(a => a.AddressId == CurrentUser.CurrentAddressId);


        public UpdateCitizenProfileViewModel(IServiceProvider serviceProvider, ICurrentUserService currentUserService, PrnContext context)
        {
            _serviceProvider = serviceProvider;
            _currentUserService = currentUserService;
            _context = context;
        }

        
    }
}
