using System.Windows;

namespace Resident.View
{
    /// <summary>
    /// Interaction logic for PoliceApprovalsOverviewWindow.xaml
    /// </summary>
    public partial class PoliceApprovalsOverviewWindow : Window
    {
        public PoliceApprovalsOverviewWindow(Resident.ViewModels.PoliceApprovalsOverviewViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
