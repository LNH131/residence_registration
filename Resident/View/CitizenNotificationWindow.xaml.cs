using System.Windows;

namespace Resident.View
{
    public partial class CitizenNotificationWindow : Window
    {
        public CitizenNotificationWindow(object viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
