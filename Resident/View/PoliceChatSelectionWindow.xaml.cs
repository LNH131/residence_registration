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
            // Gán CloseAction để khi bấm Cancel, cửa sổ sẽ đóng.
            viewModel.CloseAction = new Action(() => this.Close());
        }
    }
}
