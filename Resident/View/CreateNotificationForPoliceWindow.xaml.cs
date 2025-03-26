using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class CreateNotificationForPoliceWindow : Window
    {
        public CreateNotificationForPoliceWindow(CreateNotificationForPoliceViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
