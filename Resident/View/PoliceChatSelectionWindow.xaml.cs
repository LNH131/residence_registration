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
            // Gán hành động đóng cửa sổ cho CancelCommand
            viewModel.CloseAction = () => this.Close();
        }
    }
}
