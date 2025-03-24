using Resident.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Resident.View
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _viewModel;


        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }


        // Các hàm khác (như drag window, close, v.v.) giữ nguyên
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}
