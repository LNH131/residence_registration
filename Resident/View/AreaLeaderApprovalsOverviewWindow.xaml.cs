using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class AreaLeaderApprovalsOverviewWindow : Window
    {
        // Constructor không tham số (nếu muốn gọi XAML Designer)
        public AreaLeaderApprovalsOverviewWindow()
        {
            InitializeComponent();
        }

        // Constructor có tham số
        public AreaLeaderApprovalsOverviewWindow(AreaLeaderApprovalsOverviewViewModel vm) : this()
        {
            DataContext = vm;
        }
    }
}
