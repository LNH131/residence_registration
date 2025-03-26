using Resident.ViewModels;
using System.Windows;

namespace Resident.View
{
    public partial class CreateNotificationForCitizensWindow : Window
    {
        public CreateNotificationForCitizensWindow(CreateNotificationForCitizensViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
