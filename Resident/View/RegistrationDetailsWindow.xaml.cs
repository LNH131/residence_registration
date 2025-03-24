using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    /// <summary>
    /// Interaction logic for RegistrationDetailsWindow.xaml
    /// </summary>
    public partial class RegistrationDetailsWindow : Window
    {
        // Constructor that takes the ViewModel as a single parameter
        public RegistrationDetailsWindow(RegistrationDetailsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        // Optional: you can still keep a parameterless constructor if needed
        public RegistrationDetailsWindow()
        {
            InitializeComponent();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
