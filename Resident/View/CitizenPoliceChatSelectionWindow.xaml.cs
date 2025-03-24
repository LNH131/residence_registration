using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class CitizenPoliceChatSelectionWindow : Window
    {
        public CitizenPoliceChatSelectionWindow(CitizenPoliceChatSelectionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
