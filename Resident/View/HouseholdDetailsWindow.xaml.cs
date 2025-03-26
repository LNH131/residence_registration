using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class HouseholdDetailsWindow : Window
    {
        // Parameterless constructor for XAML design-time or if you open the window without arguments.
        public HouseholdDetailsWindow()
        {
            InitializeComponent();
        }

        // Constructor that accepts a HouseholdDetailsViewModel.
        public HouseholdDetailsWindow(HouseholdDetailsViewModel viewModel)
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
