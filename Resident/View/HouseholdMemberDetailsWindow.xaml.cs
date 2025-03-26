using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class HouseholdMemberDetailsWindow : Window
    {
        public HouseholdMemberDetailsWindow(HouseholdMemberDetailsViewModel viewModel)
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
