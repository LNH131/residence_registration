using System.Windows;
namespace Resident.View
{
    public partial class AreaLeaderApprovalsOverviewWindow : Window
    {
        public AreaLeaderApprovalsOverviewWindow(AreaLeaderApprovalsOverviewViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
