using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class CreateNotificationWindow : Window
    {
        public CreateNotificationWindow(CreateNotificationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
