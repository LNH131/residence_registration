using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class AreaLeaderChatWindow : Window
    {
        public AreaLeaderChatWindow(AreaLeaderChatViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
