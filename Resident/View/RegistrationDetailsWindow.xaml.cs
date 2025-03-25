using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class RegistrationDetailsWindow : Window
    {
        // Constructor không tham số (paramless)
        public RegistrationDetailsWindow()
        {
            InitializeComponent();
        }

        // Constructor có tham số, nhận ViewModel
        public RegistrationDetailsWindow(RegistrationDetailsViewModel viewModel) : this()
        {
            // Gọi constructor không tham số để InitializeComponent()
            // Rồi gán DataContext
            DataContext = viewModel;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
