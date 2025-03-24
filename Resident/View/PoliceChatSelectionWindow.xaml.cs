using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class PoliceChatSelectionWindow : Window
    {
        public PoliceChatSelectionWindow(PoliceChatSelectionViewModel viewModel)
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
