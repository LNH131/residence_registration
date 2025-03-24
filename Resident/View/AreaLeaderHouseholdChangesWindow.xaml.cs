using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class AreaLeaderHouseholdChangesWindow : Window
    {
        public AreaLeaderHouseholdChangesWindow(AreaLeaderHouseholdChangesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
