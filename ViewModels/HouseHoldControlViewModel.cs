using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Project.DAO;
using Project.Enums;
using Project.View;

namespace Project.ViewModels
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
