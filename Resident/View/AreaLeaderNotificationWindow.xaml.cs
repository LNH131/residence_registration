using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class AreaLeaderNotificationWindow : Window
    {
        public AreaLeaderNotificationWindow(AreaLeaderNotificationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // Cho phép ViewModel đóng cửa sổ
            if (viewModel.CloseAction == null)
                viewModel.CloseAction = new Action(() => this.Close());
        }
    }
}
