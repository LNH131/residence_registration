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
            // Set the CloseCommand action to close the window
            viewModel.CloseCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => this.Close());
        }
    }
}
