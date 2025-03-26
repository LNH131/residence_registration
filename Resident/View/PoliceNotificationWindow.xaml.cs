using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class PoliceNotificationWindow : Window
    {
        public PoliceNotificationWindow(PoliceNotificationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // Optionally handle window close via VM
            viewModel.CloseAction = () => this.Close();
        }
    }
}
