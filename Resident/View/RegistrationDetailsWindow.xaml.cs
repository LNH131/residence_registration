using System.Windows;

namespace Resident.View
{
    /// <summary>
    /// Interaction logic for RegistrationDetailsWindow.xaml
    /// </summary>
    public partial class RegistrationDetailsWindow : Window
    {
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
