using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resident.Service;
using Resident.Models;

namespace Resident.ViewModels
{
    public partial class HouseHoldSelectionViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrentUserService _currentUserService;

        public User CurrentUser => _currentUserService.CurrentUser;


        public HouseHoldSelectionViewModel(IServiceProvider serviceProvider, ICurrentUserService currentUserService)
        {
            _serviceProvider = serviceProvider;
            _currentUserService = currentUserService;
        }
    }
}
