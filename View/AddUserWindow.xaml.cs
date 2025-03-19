using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Project.ViewModels;

namespace Project.View
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private readonly AddUserViewModel _viewModel;

        public AddUserWindow(AddUserViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddUserViewModel vm)
            {
                vm.Password = (sender as PasswordBox)?.Password ?? string.Empty;
            }
        }
    }
}
