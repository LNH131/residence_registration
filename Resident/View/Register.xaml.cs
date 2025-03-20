using Resident.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Resident.View
{
    public partial class Register : Window
    {
        public Register(RegisterViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm)
            {
                vm.Password = (sender as PasswordBox)?.Password ?? string.Empty;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm)
            {
                vm.ConfirmPassword = (sender as PasswordBox)?.Password ?? string.Empty;
            }
        }
    }
}
