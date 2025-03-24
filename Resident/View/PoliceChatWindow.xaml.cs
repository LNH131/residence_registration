using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class PoliceChatWindow : Window
    {
        public PoliceChatWindow(PoliceChatViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
