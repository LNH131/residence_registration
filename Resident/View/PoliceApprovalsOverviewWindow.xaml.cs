using System.Windows;

namespace Resident.View
{
    public partial class PoliceApprovalsOverviewWindow : Window
    {
        public PoliceApprovalsOverviewWindow(PoliceApprovalsOverviewViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
