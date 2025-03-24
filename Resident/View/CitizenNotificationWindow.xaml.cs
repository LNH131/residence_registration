using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class CitizenNotificationWindow : Window
    {
        public CitizenNotificationWindow(CitizenNotificationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            if (viewModel.CloseAction == null)
                viewModel.CloseAction = new Action(() => this.Close());
        }
    }
}
