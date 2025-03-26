using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class AreaLeaderChatWindow : Window
    {
        // Parameterless constructor for design-time support.
        public AreaLeaderChatWindow()
        {
            InitializeComponent();
        }

        // Constructor that accepts a ViewModel.
        public AreaLeaderChatWindow(AreaLeaderChatViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
