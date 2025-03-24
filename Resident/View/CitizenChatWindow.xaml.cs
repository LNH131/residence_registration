using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class CitizenChatWindow : Window
    {
        public CitizenChatWindow(CitizenChatViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
