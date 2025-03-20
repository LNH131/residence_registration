using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Resident.DAO;
using Resident.Enums;
using Resident.View;

namespace Resident.ViewModels
{
    public partial class HouseHoldControlViewModel : ObservableObject
    {
        public IAsyncRelayCommand HouseholdControlCommand { get; }
        private readonly IServiceProvider _serviceProvider;
        public HouseHoldControlViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

    }
}
