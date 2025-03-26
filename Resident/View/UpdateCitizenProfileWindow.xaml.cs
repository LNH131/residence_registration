using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Resident.DAO;
using Resident.Models;
using Resident.Service;
using Resident.ViewModels;

namespace Resident.View
{
    /// <summary>
    /// Interaction logic for UpdateCitizenProfileWindow.xaml
    /// </summary>
    public partial class UpdateCitizenProfileWindow : Window
    {
        private UpdateCitizenProfileViewModel _viewModel;
        private User _currentUser;
        private Address _currentAddress;
        public UpdateCitizenProfileWindow(UpdateCitizenProfileViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _currentUser = viewModel.CurrentUser;
            _currentAddress = viewModel.CurrentAddress;
            DataContext = _viewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
