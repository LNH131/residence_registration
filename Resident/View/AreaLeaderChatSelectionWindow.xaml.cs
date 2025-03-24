using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class AreaLeaderChatSelectionWindow : Window
    {
        public AreaLeaderChatSelectionWindow(AreaLeaderChatSelectionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
