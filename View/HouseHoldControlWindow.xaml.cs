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
using Project.Models;
using Project.DAO;
using System.Collections.ObjectModel;
using Project.ViewModels;

namespace Project.View
{
    /// <summary>
    /// Interaction logic for HouseHoldControlWindow.xaml
    /// </summary>
    public partial class HouseHoldControlWindow : Window
    {
        private readonly CitizenViewModel _citizenViewModel;
        private ObservableCollection<HouseholdMember> _householdMembers;
        private readonly UserDAO _userDAO;
        public HouseHoldControlWindow(User currentUser, UserDAO userDAO)
        {
            InitializeComponent();
            DataContext = currentUser;
            _userDAO = userDAO;

            _householdMembers = new ObservableCollection<HouseholdMember>();
            dgHouseholdMembers.ItemsSource = _householdMembers;
        }
    }
}
