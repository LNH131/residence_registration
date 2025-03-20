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

namespace Resident.View
{
    /// <summary>
    /// Interaction logic for UpdateCitizenProfileWindow.xaml
    /// </summary>
    public partial class UpdateCitizenProfileWindow : Window
    {
        private readonly UserDAO _userDAO;
        private readonly User _currentUser;
        public UpdateCitizenProfileWindow(UserDAO userDAO, User currentUser)
        {
            InitializeComponent();

            _userDAO = userDAO;
            _currentUser = currentUser;
            Debug.WriteLine(_currentUser);

            // Set DataContext for data binding
            DataContext = _currentUser;
        }
    }
}
