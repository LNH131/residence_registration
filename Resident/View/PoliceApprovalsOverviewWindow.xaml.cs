using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class PoliceApprovalsOverviewWindow : Window
    {
        public PoliceApprovalsOverviewWindow()
        {
            InitializeComponent();
        }

        public PoliceApprovalsOverviewWindow(PoliceApprovalsOverviewViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }

}
