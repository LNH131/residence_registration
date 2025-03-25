using Resident.ViewModels; // <-- Adjust namespace as needed
using System.Windows;

namespace Resident.View
{
    public partial class AreaLeaderRegistrationDetailsWindow : Window
    {
        public AreaLeaderRegistrationDetailsWindow(AreaLeaderRegistrationDetailsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
