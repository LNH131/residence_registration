using System.Windows;

namespace Resident.View
{
    public partial class RegistrationOverviewWindow : Window
    {
        public RegistrationOverviewWindow(object viewModel)
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
